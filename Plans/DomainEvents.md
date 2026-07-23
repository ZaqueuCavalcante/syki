# Domain Events

Plano para introduzir **eventos de domínio** no backend: uma entidade emite eventos que são
persistidos na tabela `domain_events` **na mesma transação** em que a entidade é salva, e um
`DomainEventsProcessor` (espelho do `CommandsProcessor`) processa esses eventos criando
**comandos**.

Base atual: `Back/Commands/` (`ICommand`, `ICommandHandler<T>`, `CommandsProcessor`,
`CommandConfigs`), `Back/Domain/Commands/Command.cs`, `Back/Database/EstudDbContext.Commands.cs`,
`Back/Middlewares/CommandsProcessorTriggerMiddleware.cs`.

## Motivação

Hoje o serviço mistura duas responsabilidades e paga isso com dois `SaveChangesAsync`
(`CreateClassActivityService.cs:42` e `:45`): primeiro salva a entidade (para obter o `Id` gerado
pelo banco), depois cria o comando de efeito colateral. São duas transações — se a segunda falhar,
a atividade existe sem a notificação.

Com eventos de domínio:

- o serviço faz **um único** `await ctx.SaveChangesAsync()`;
- a entidade declara *o que aconteceu* (`ClassActivityCreated`), não *o que fazer*;
- os efeitos colaterais saem do serviço e passam a estar em handlers desacoplados — vários
  handlers podem reagir ao mesmo evento sem tocar no serviço;
- entidade + evento caem juntos ou não caem (outbox transacional).

## Fluxo alvo

```
Service                        SaveChangesAsync (1 batch)          DomainEventsProcessor        CommandsProcessor
  │                                      │                                   │                          │
  ├─ activity.Raise(...)                 │                                   │                          │
  └─ await ctx.SaveChangesAsync() ──────▶ INSERT class_activities            │                          │
                                          INSERT domain_events (pending) ──▶ handlers do evento         │
                                                                             └─ ctx.AddCommand(...) ──▶ INSERT commands ──▶ handler do comando
```

Regra fixa: **handler de evento de domínio nunca faz nada mutável** — só lê dados e cria comandos.
Toda mutação acontece dentro de um comando, que tem erro, retry e backoff próprios
(`CommandBackoffStrategies`), analisáveis individualmente.

## Decisão central: `string Uid` (ULID) nas entidades emissoras

O obstáculo do desenho é que o `Id` de uma entidade nova só existe **depois** do `INSERT` — é
exatamente por isso que hoje há dois `SaveChangesAsync`. Se o evento for construído no momento em
que é emitido, `ClassActivityCreated(activity.Id)` serializa `Id = 0`.

**Solução:** toda entidade que emite eventos ganha uma coluna obrigatória `string Uid`, gerada no
construtor com `Ulid.NewUlid().ToString()`. O identificador passa a ser conhecido **antes** do
`INSERT`, e o evento referencia o `Uid`; os handlers chegam na entidade por ele.

O que isso destrava, em cascata:

- o evento vira um `record` comum, construído na hora do `Raise` — sem factory, sem avaliação
  adiada, sem acoplamento temporal;
- os `INSERT`s da entidade e dos eventos saem no **mesmo batch** de um único `SaveChanges`, e a
  transação implícita do EF já garante atomicidade;
- o interceptor de persistência precisa de **um único hook** (`SavingChangesAsync`) — sem
  transação explícita, sem controle de posse de transação, sem guard de reentrância.

Alternativas descartadas:

| Alternativa | Por que não |
|---|---|
| Duas idas ao banco com transação explícita e evento como `Func<IDomainEvent>` | Zero custo de schema, mas deixa para sempre a parte sutil: posse de transação, reentrância do `SaveChanges` interno e uma lambda cuja avaliação tardia é invisível em quem lê |
| `UseHiLo()` — Ids gerados no cliente via sequence | Mesmo ganho, mas exige migrar colunas `identity` para sequence em produção (semear a sequence acima do `max(id)`, errar = colisão de PK) |

### Escopo da regra

Não é "toda entidade que emite eventos" — é **toda entidade cujo identificador gerado pelo banco
aparece num payload de evento**. Se um dia `ClassActivityCreated` carregar os identificadores dos
`ClassActivityWork` criados junto, os filhos também precisam de `Uid`.

O caso comum fica auto-enforçado pela interface `IHasDomainEvents`, que exige o `Uid` (ver
*Emissão*). O caso amplo é convenção — registrar no `CLAUDE.md`.

### Fronteira `Uid` × `int Id`

O `Uid` é o contrato **do evento**. O `int Id` continua sendo a identidade interna: FKs, joins,
DTOs de API, comandos. O handler do evento traduz `Uid → Id` e é aí que a fronteira termina —
`CreateNewClassActivityNotificationCommand(int ClassActivityId)` não muda.

## ULID como `string`

Pacote [`Cysharp/Ulid`](https://github.com/Cysharp/Ulid) (`<PackageReference Include="Ulid" />`),
usado **só como gerador**: `Ulid.NewUlid().ToString()`. O tipo da propriedade é `string`.

ULID em vez de Guid porque é **ordenado por tempo**: 128 bits em 26 caracteres Crockford base32,
onde os 10 primeiros codificam o timestamp de 48 bits e os 16 restantes são aleatórios. O alfabeto
(`0123456789ABCDEFGHJKMNPQRSTVWXYZ`, sem I/L/O/U) está em ordem ASCII ascendente, então ordem
lexicográfica = ordem cronológica: o índice único cresce por inserção sequencial em vez de
fragmentar como Guid v4. O projeto já tem preferência declarada por identificadores ordenáveis —
commit `3e10f644e` *"Upgrade all guids to V7"* e `ResetPasswordToken` com `Guid.CreateVersion7()`.

### Por que `string` e não o struct `Ulid`

Com a propriedade tipada como `Ulid`, cada camada exigiria um conversor: `ValueConverter` no EF
(registrado num `ConfigureConventions`), `JsonConverter<Ulid>` no Newtonsoft — que é o que
`object.Serialize()` usa (`StringExtensions.cs:96`) para gravar `DomainEvent.Data` — e outro no
System.Text.Json, que é o que o invoker usa para ler o payload de volta. Com `string`, nenhum: EF,
Dapper, Newtonsoft e System.Text.Json tratam tudo nativamente.

Dois efeitos colaterais reforçam a escolha:

- **Falha alto quando esquecido.** `string` não atribuído é `null` → violação de `NOT NULL` já no
  primeiro insert. `Ulid` não atribuído é `Ulid.Empty` → grava `00000000000000000000000000` sem
  reclamar e só quebra na *segunda* entidade, pelo índice único, com erro que não aponta para a
  causa.
- **Sem surpresa de tradução no LINQ.** Propriedade com `ValueConverter` tem limitações reais de
  tradução no EF; com `string` puro, qualquer predicado traduz.

O que se perde é validação e type safety no boundary: `string Uid` aceita qualquer string. No v1 é
irrelevante — o `Uid` nunca vem de input externo, o caminho é entidade → record do evento → query
do handler, tudo interno. **Gatilho para reavaliar:** se o `Uid` virar a identidade pública da API
(decisão em aberto abaixo), validar entrada externa passa a ser trabalho de verdade e o tipo forte
se paga.

### Coluna

Duas linhas no `DbConfig` de cada entidade emissora, espelhando o que
`SsoConfigurationDbConfig.cs:18` já faz para o `PublicId`:

```csharp
entity.Property(e => e.Uid).HasMaxLength(26);
entity.HasIndex(e => e.Uid).IsUnique();
```

Gera `varchar(26)`. Sem o `HasMaxLength` viraria `text`, que em Postgres não tem diferença de
performance — o limite é documentação e constraint barata, não otimização. Descartados:

- **`char(26)`** — a doc do Postgres é explícita que `char(n)` costuma ser o mais lento dos três
  por causa do blank-padding, e ainda traz a semântica de ignorar espaços à direita na comparação.
- **`uuid`** — 16 bytes contra 27, mas `Ulid.ToGuid()` reinterpreta os bytes no layout interno do
  `Guid` do .NET (mixed-endian nos 3 primeiros campos) enquanto o Postgres compara `uuid` em ordem
  de rede: o índice **perde a monotonicidade**, que é a razão de usar ULID. Dá para contornar com
  `new Guid(bytes, bigEndian: true)`, mas aí volta o converter que essa decisão veio eliminar.

Em disco: 26 bytes + 1 de header varlena. Refinamento opcional se o índice pesar: declarar a coluna
`COLLATE "C"`, trocando as regras de collation da locale por comparação byte a byte.

## Peças novas

| Arquivo | Papel |
|---|---|
| `Back/DomainEvents/IDomainEvent.cs` | Marker interface (espelho de `ICommand`) |
| `Back/DomainEvents/IDomainEventHandler.cs` | `IDomainEventHandler<T>` + `IDomainEventInvoker` + `DomainEventInvoker<T>` |
| `Back/DomainEvents/IHasDomainEvents.cs` | Contrato da entidade emissora + extension `Raise` |
| `Back/DomainEvents/DomainEventDescriptionAttribute.cs` | Descrição em pt-BR (espelho de `CommandDescriptionAttribute`) |
| `Back/DomainEvents/DomainEventToDescriptionMapper.cs` | Espelho de `CommandToDescriptionMapper` |
| `Back/DomainEvents/DomainEventsProcessor.cs` | `IJob` Quartz — espelho de `CommandsProcessor` |
| `Back/Domain/DomainEvents/DomainEvent.cs` | Entidade persistida |
| `Back/Domain/Enums/DomainEventStatus.cs` | `Pending/Processing/Success/Error` |
| `Back/Database/DomainEvents/DomainEventDbConfig.cs` | `IEntityTypeConfiguration<DomainEvent>` |
| `Back/Database/Interceptors/DomainEventsSaveChangesInterceptor.cs` | Materializa os eventos no mesmo `SaveChanges` |
| `Back/Database/EstudDbContext.DomainEvents.cs` | `DbSet`, `ConfigureDomainEvents`, `HasPendingDomainEvents` |
| `Back/Configs/DomainEventConfigs.cs` | Registro por reflection dos handlers + registry de tipos |
| `Back/Extensions/DomainEventsExtensions.cs` | `scheduler.TriggerDomainEventsProcessorJob()` |

Alterados: `Back/Back.csproj` (pacote `Ulid`), `Back/Database/EstudDbContext.cs` (registro do
interceptor), `Back/Configs/QuartzConfigs.cs`, `Back/Program.cs`,
`Back/Middlewares/CommandsProcessorTriggerMiddleware.cs`,
`Back/Domain/Classes/ClassActivity.cs`, `Back/Features/Teachers/CreateClassActivity/*`,
`Tests/Base/BackFactory.Background.cs`.

## Modelo de dados

`DomainEvent` é uma versão **enxuta** de `Command` — sem `ParentId`, `BatchId`, `NotBefore`,
`MaxRetries`, `BackoffStrategy` (ver *Decisões*). A PK continua `int`, como em `Command`; o `Uid`
existe nas entidades de domínio, não aqui.

```csharp
namespace Estud.Back.Domain.DomainEvents;

public class DomainEvent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DomainEventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Duration { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public string? ActivityId { get; set; }
    public List<string> Logs { get; set; } = [];

    public Institution? Institution { get; set; }

    public DomainEvent() { }

    public DomainEvent(int institutionId, object data, string? activityId = null)
    {
        InstitutionId = institutionId;
        Type = data.GetType().Name;
        Data = data.Serialize();
        CreatedAt = DateTime.UtcNow;
        ActivityId = activityId;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.UtcNow;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? DomainEventStatus.Error : DomainEventStatus.Success;
    }

    public ActivityContext GetParentActivityContext() { ... }   // igual a Command
}
```

`DomainEventDbConfig` espelha `CommandDbConfig` (`ToTable("domain_events", DbSchemas.Estud)` +
`HasKey`). Índice para o polling: `(processor_id, status, created_at)`.

## Emissão

```csharp
public interface IDomainEvent;

public interface IHasDomainEvents
{
    string Uid { get; }
    List<IDomainEvent> DomainEvents { get; }
}

public static class DomainEventsExtensions
{
    extension(IHasDomainEvents entity)
    {
        public void Raise(IDomainEvent @event) => entity.DomainEvents.Add(@event);
    }
}
```

A interface exigir o `Uid` é o que torna a regra auto-enforçada: não dá para emitir evento sem ter
identificador estável. Na entidade:

```csharp
public class ClassActivity : IHasDomainEvents
{
    public int Id { get; set; }
    public string Uid { get; set; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = [];

    private ClassActivity(...)
    {
        Uid = Ulid.NewUlid().ToString();
        ...
        Raise(new ClassActivityCreated(Uid));
    }
}
```

O evento em si segue o formato dos comandos (record + atributo de descrição), vizinho da feature:

```csharp
namespace Estud.Back.Features.Teachers.CreateClassActivity;

[DomainEventDescription("Atividade de turma criada")]
public record ClassActivityCreated(string ClassActivityUid) : IDomainEvent;
```

## Persistência: `ISaveChangesInterceptor`

A persistência fica num **interceptor do EF Core**, não numa override de `SaveChangesAsync`. O
projeto já tem o precedente (`AuditSaveChangesInterceptor`, `EstudDbContext.cs:28`), e o interceptor
cobre todos os caminhos de save — a override exigiria acertar a sobrecarga certa
(`SaveChangesAsync(bool, CancellationToken)`; a de só `ct` delega para ela, então sobrescrever a
errada é silenciosamente ignorada) e deixaria o `SaveChanges()` síncrono de fora.

Com o `Uid` conhecido de antemão, basta o hook de **antes** do save: entidades adicionadas ao
`ChangeTracker` dentro de `SavingChangesAsync` entram no mesmo batch — mesmo padrão que
interceptors de auditoria usam.

```csharp
public class DomainEventsSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        MaterializeDomainEvents((EstudDbContext)eventData.Context!);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        MaterializeDomainEvents((EstudDbContext)eventData.Context!);
        return ValueTask.FromResult(result);
    }

    private static void MaterializeDomainEvents(EstudDbContext ctx)
    {
        var emitters = ctx.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        if (emitters.Count == 0) return;

        var institutionId = ctx.RequestUser.InstitutionId;
        if (institutionId == 0)
            throw new InvalidOperationException("Domain events require a resolved InstitutionId.");

        var activityId = Activity.Current?.Id;

        foreach (var emitter in emitters)
        {
            foreach (var @event in emitter.DomainEvents)
            {
                ctx.Add(new DomainEvent(institutionId, @event, activityId));
            }
            emitter.DomainEvents.Clear();
        }

        ctx.HasPendingDomainEvents = true;
    }
}
```

Registro junto do de auditoria, em `EstudDbContext.OnConfiguring`:

```csharp
optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor(), new DomainEventsSaveChangesInterceptor());
```

### Pontos de atenção

- **Atomicidade é de graça.** Entidade e eventos vão no mesmo batch; o EF abre uma transação
  implícita quando há mais de um comando. Nada de transação explícita, posse de transação ou
  rollback manual.
- **Sem estado entre chamadas.** O interceptor é stateless — não precisa de flag de reentrância
  porque não dispara um segundo `SaveChanges`.
- **Transação alheia.** Quando o `CommandsProcessor` já abriu transação
  (`CommandsProcessor.cs:47`), o batch simplesmente participa dela. Nada a tratar.
- **Não depende de `EntityState`.** A coleta olha `DomainEvents.Count > 0`, não o estado da
  entrada — então a ordem entre o hook e o `DetectChanges` do EF é irrelevante.
- **`InstitutionId`.** Vem de `ctx.RequestUser.InstitutionId`, como manda a convenção do projeto
  (nunca passar institution id pela cadeia de chamadas). Em contexto de background `RequestUser`
  não é populado — daí o fail fast em vez de gravar `0`. Se aparecer a necessidade real de emitir
  eventos de dentro de um handler de comando, aí sim uma sobrecarga `Raise(institutionId, ...)`.
- **`HasPendingDomainEvents` otimista.** É marcado antes do save; se o save falhar, o middleware
  dispara o job à toa e ele não encontra nada. Inofensivo, e mais simples do que um hook a mais.
- **Auditoria.** O `AuditSaveChangesInterceptor` passa a ver as linhas de `domain_events` no mesmo
  save — verificar se a config de auditoria deve ignorar a tabela.

## `DomainEventsProcessor`

Espelho quase literal do `CommandsProcessor`, com o mesmo SQL de polling
(`FOR UPDATE SKIP LOCKED LIMIT 10`), a mesma `ActivitySource`/tracing e o mesmo tratamento de erro:

```csharp
private static readonly string Sql = @"
    UPDATE estud.domain_events
    SET processor_id = {0}, status = 1
    WHERE ctid IN (
        SELECT ctid
        FROM estud.domain_events
        WHERE processor_id IS NULL AND status = 0
        ORDER BY created_at
        FOR UPDATE SKIP LOCKED
        LIMIT 10
    )
    RETURNING *;
";
```

Diferenças em relação ao de comandos:

1. **N handlers por evento.** O invoker resolve `GetServices<IDomainEventHandler<T>>()` e executa
   todos. É o ponto do padrão: assinar um evento novo não exige tocar em quem emite.
   Zero handlers é legítimo (evento só registrado) — diferente de `CommandConfigs.cs:37`, que
   estoura para comando sem handler.
2. **Sem retry.** Se o evento falha, fica `Error` para análise e reprocessamento manual; o retry
   automático é responsabilidade dos comandos gerados. Ver *Decisões*.
3. **Tudo-ou-nada por evento.** Todos os handlers do mesmo evento rodam na mesma transação: se um
   falhar, nenhum comando é criado e o reprocessamento do evento é seguro (sem duplicar os
   comandos dos handlers que já tinham passado).
4. **Encadeamento.** Ao final do lote, se `ctx.HasPendingCommands`, dispara o `CommandsProcessor`
   (`scheduler.TriggerCommandsProcessorJob()`) — como o `CommandsProcessor` já faz consigo mesmo em
   `CommandsProcessor.cs:82`.

### Guarda da regra "handler não muta nada"

Antes de commitar, inspecionar o `ChangeTracker` e falhar se qualquer entrada
`Added/Modified/Deleted` não for `Command`, `CommandBatch` ou o próprio `DomainEvent`:

```csharp
var forbidden = ctx.ChangeTracker.Entries()
    .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
    .Where(e => e.Entity is not (Command or CommandBatch or DomainEvent))
    .Select(e => e.Entity.GetType().Name)
    .Distinct().ToList();

if (forbidden.Count > 0)
    throw new InvalidOperationException(
        $"Domain event handlers must only create commands. Mutated: {string.Join(", ", forbidden)}");
```

Transforma a convenção em erro visível no evento (campo `Error`) em vez de comentário no
`CLAUDE.md`. Handlers devem usar `AsNoTracking()` nas leituras.

## Wiring

- **`DomainEventConfigs.AddDomainEventConfigs()`** — espelha `CommandConfigs`: varre os assemblies
  `Back*`, popula `_domainEventTypes` (`GetDomainEventType(name)`) e registra **todos** os
  `IDomainEventHandler<>` com `AddTransient` (vários por evento). Chamado em `Program.cs` logo após
  `builder.AddCommandConfigs()`.
- **`QuartzConfigs`** — novo job `DomainEventsProcessor` com trigger próprio e intervalo
  `Jobs.DomainEventsPollingIntervalInSeconds` (novo campo de settings, default igual ao de comandos).
- **Middleware** — estender o `CommandsProcessorTriggerMiddleware` para disparar os dois jobs
  (renomear para `BackgroundProcessorsTriggerMiddleware`): se `ctx.HasPendingDomainEvents`, dispara
  o de eventos; se `ctx.HasPendingCommands`, o de comandos.

## Migração do `CreateClassActivity` (feature-piloto)

**Antes** (`CreateClassActivityService.cs:41-45`):

```csharp
var activity = result.Success;
await ctx.SaveChangesAsync(activity);

ctx.AddCommand(institutionId, new CreateNewClassActivityNotificationCommand(activity.Id));
await ctx.SaveChangesAsync();
```

**Depois:**

```csharp
var activity = result.Success;
await ctx.SaveChangesAsync(activity);
```

O `Raise(new ClassActivityCreated(Uid))` fica no construtor privado de `ClassActivity`, e o handler
novo (`ClassActivityCreatedHandler`, ao lado da feature) só cria o comando:

```csharp
public class ClassActivityCreatedHandler(EstudDbContext ctx) : IDomainEventHandler<ClassActivityCreated>
{
    public async Task Handle(int eventId, ClassActivityCreated @event)
    {
        var activity = await ctx.ClassActivities.AsNoTracking()
            .Where(x => x.Uid == @event.ClassActivityUid)
            .Select(x => new { x.Id, x.Class!.InstitutionId })
            .FirstAsync();

        ctx.AddCommand(activity.InstitutionId, new CreateNewClassActivityNotificationCommand(activity.Id));
    }
}
```

`CreateNewClassActivityNotificationCommand` e seu handler **não mudam** — continuam sendo a mutação
(criar `Notification` + `UserNotification`), com retry próprio.

> Nota: o handler precisa da navigation `ClassActivity.Class`, que hoje não existe
> (`Back/Domain/Classes/ClassActivity.cs` só tem `ClassId`). Criar a navigation no entity/config
> conforme a convenção do `CLAUDE.md` (nunca `join` em query syntax).

### Migração da coluna `uid`

Aditiva e sem pegadinha de leitura: como a coluna é `string`, valor fora do formato não derruba
query nenhuma (simplesmente não casa). Ainda assim o backfill deve gerar ULIDs de verdade, para
manter a ordenação — Postgres não tem função nativa de ULID e `gen_random_uuid()` não produz
Crockford base32, então vem do C#.

1. `ALTER TABLE ... ADD COLUMN uid varchar(26) NULL`
2. Backfill em lotes por script único (Dapper), gerando `Ulid.NewUlid().ToString()` por linha
3. `SET NOT NULL` + `CREATE UNIQUE INDEX CONCURRENTLY`

Nos testes de integração nada disso aparece: o schema é criado do zero via
`Database.EnsureCreatedAsync()` (`Tests/Base/IntegrationTestBase.cs:54`).

## Testes

- **Unit** (`Tests/DomainEvents/DomainEventsUnitTests.cs`) — espelho de `CommandsUnitTests`:
  - todo `IDomainEvent` tem `[DomainEventDescription]` não vazia;
  - **round-trip de serialização**: cada `IDomainEvent` sobrevive a `Serialize()` (Newtonsoft) +
    desserialização (System.Text.Json). O `Uid` sendo `string` já não corre risco, mas a assimetria
    entre os dois serializadores continua valendo para enums, `DateOnly` e afins.
- **Integração** — no `CreateClassActivityIntegrationTests`, região *Happy path*:
  - criar atividade → existe 1 linha em `domain_events` do tipo `ClassActivityCreated`, com o `Uid`
    da atividade no payload, e **zero** comandos;
  - após `await _back.AwaitCommandsProcessing()` → evento `Success`, comando criado e processado,
    notificações dos alunos criadas (asserção que já existe hoje, deve continuar passando).
- **`Tests/Base/BackFactory.Background.cs`** — novo `AwaitDomainEventsProcessing()` e, dentro de
  `AwaitCommandsProcessing()`, processar **primeiro** os eventos pendentes e depois os comandos, em
  loop até os dois zerarem. Assim os testes existentes que chamam `AwaitCommandsProcessing()`
  seguem verdes sem alteração.
- **Guarda de mutação** — handler de evento fake que tenta alterar uma entidade qualquer: evento
  termina `Error` com a mensagem da guarda e nada é persistido.
- **Atomicidade** — forçar falha no save e verificar que nem a entidade nem o evento existem.
- **Unicidade do `Uid`** — duas atividades criadas na mesma requisição têm `Uid` distintos e
  ordenados (ULID monotônico).

## Decisões tomadas

| Decisão | Resolução |
|---|---|
| Onde os eventos são persistidos | Tabela `domain_events` própria (outbox), no mesmo `SaveChanges` da entidade |
| Como o evento referencia a entidade | Coluna obrigatória `string Uid`, gerada no construtor — identificador conhecido antes do `INSERT` |
| ULID × Guid | ULID (`Cysharp/Ulid`, só como gerador): ordenado por tempo, índice sem fragmentação, 26 chars legíveis |
| Tipo da propriedade | `string`, não o struct `Ulid` — dispensa `ValueConverter` do EF e os dois `JsonConverter`, e falha alto quando esquecido |
| Armazenamento do `Uid` | `varchar(26)` + índice único, declarados por `DbConfig`; `char(26)` e `uuid` descartados |
| Quantos `SaveChangesAsync` no serviço | Um, e uma ida só ao banco |
| Onde mora a persistência dos eventos | `ISaveChangesInterceptor`, hook `SavingChanges`/`SavingChangesAsync` apenas — stateless |
| Atomicidade | Transação implícita do EF sobre o batch; nada explícito |
| Handlers por evento | N (0..N), todos na mesma transação; tudo-ou-nada por evento |
| O que um handler pode fazer | **Só ler e criar comandos** — garantido em runtime pela guarda do `ChangeTracker` |
| Retry | Não no evento; o retry vive nos comandos gerados (`MaxRetries` + `BackoffStrategy`) |
| `InstitutionId` do evento | De `ctx.RequestUser.InstitutionId`; exceção se resolver para `0` |
| Fronteira `Uid` × `Id` | `Uid` é contrato do evento; `Id` continua em FKs, DTOs e comandos |
| Disparo do processamento | Middleware existente estendido: dispara o job de eventos e o de comandos |

## Decisões em aberto

| Decisão | Opções | Sugestão |
|---|---|---|
| `Uid` como identidade pública da API | interno-só × expor no lugar do `int` nos DTOs | Interno-só na v1 — decisão bem maior que eventos de domínio, e é também o gatilho para trocar `string` pelo struct `Ulid` |
| Rastreabilidade evento → comandos | novo `Command.DomainEventId` × nada | Adicionar `Command.DomainEventId` (nullable): custo baixo, rastreabilidade completa |
| Um job só × dois jobs | job único × dois jobs Quartz | Dois jobs (isolamento de falha e de intervalo de polling) |
| Eventos emitidos de dentro de handler de comando | proibir × permitir com institution explícito | Proibir na v1 (fail fast); reavaliar quando aparecer o caso |
| Collation do índice de `uid` | default × `COLLATE "C"` | Default na v1; `COLLATE "C"` se o índice pesar |
| Tela de administração | listar `domain_events` × nada (hoje `commands` também não tem) | Fora de escopo; quando vier, mesma tela para os dois |
| Limpeza / retenção | manter tudo × job de purge dos `Success` antigos | Manter tudo na v1; purge quando o volume justificar |

## Ordem de implementação

1. Pacote `Ulid` no `Back.csproj` — só o gerador, nenhum converter em lugar nenhum.
2. Infra de eventos sem nenhum emissor: `IDomainEvent`, `IDomainEventHandler<T>` + invoker,
   `IHasDomainEvents` + `Raise`, `DomainEventDescriptionAttribute`/mapper.
3. Entidade `DomainEvent` + `DomainEventStatus` + `DomainEventDbConfig` +
   `EstudDbContext.DomainEvents.cs` (`DbSet`, `ConfigureDomainEvents`, `HasPendingDomainEvents`).
4. `DomainEventsSaveChangesInterceptor` + registro no `OnConfiguring`.
5. `DomainEventConfigs` + registro no `Program.cs` (até aqui nada muda de comportamento — não há
   evento nenhum).
6. `DomainEventsProcessor` + guarda de mutação + `DomainEventsExtensions` + job no `QuartzConfigs`
   + middleware estendido.
7. Suporte nos testes: `AwaitDomainEventsProcessing` e encadeamento no `AwaitCommandsProcessing`.
8. Feature-piloto `CreateClassActivity`: `Uid` na `ClassActivity`, navigation `ClassActivity.Class`,
   evento `ClassActivityCreated`, `Raise` no construtor, handler criando o comando, remoção do
   segundo `SaveChangesAsync`.
9. Testes unitários e de integração (round-trip de serialização, atomicidade, guarda, unicidade).
10. SQL de produção: tabela `domain_events` + coluna `uid` com backfill em lotes.
11. Documentar as duas regras no `CLAUDE.md` (seção *Backend conventions*): handler de evento de
    domínio nunca muta estado (só cria comandos); toda entidade cujo identificador aparece em
    payload de evento tem `string Uid`.

## Candidatos seguintes (depois do piloto)

Serviços que hoje salvam e depois criam comando — mesma transformação, um a um:

- `Back/Features/Users/RegisterUser` → `SendFirstAccessMagicLinkEmailCommand`
- `Back/Features/Identity/SendResetPasswordToken` → `SendResetPasswordTokenEmailCommand`
- `Back/Features/Webhooks/CallWebhooks` → `CallWebhookCommand` (candidato mais forte: um evento de
  domínio pode ter *vários* assinantes — notificação **e** webhook — sem o serviço saber de nenhum)
