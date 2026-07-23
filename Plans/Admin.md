# Admin (Backoffice)

Plano para um **backoffice** do produto: endpoints cross-tenant de leitura (instituições, usuários,
eventos de domínio, comandos) e de operação (filtrar o que falhou, reprocessar comandos e eventos).
São endpoints para **operar o produto**, não para os usuários da instituição.

**Decisão de arquitetura:** o admin é um **projeto/host separado** (`Admin.csproj`) que referencia o
`Back.csproj` e reusa `EstudDbContext`, entidades, settings e extensions — nos moldes do projeto
`Mocks/`, que já faz exatamente isso com `RegisterOnlyMocksControllersFeatureProvider`. Os endpoints
de admin **não vivem no assembly da API pública**: não aparecem nas docs, não sobem no deploy da API
e não compartilham o pipeline de autenticação do app — tudo isso por construção, não por
configuração de runtime.

Base atual relevante: `Mocks/` (precedente do host separado que referencia o Back),
`Back/Domain/Commands/Command.cs`, `Back/Domain/DomainEvents/DomainEvent.cs`,
`Back/Commands/CommandsProcessor.cs`, `Back/DomainEvents/DomainEventsProcessor.cs`,
`Back/Configs/EntityFrameworkConfigs.cs`, `Back/Auth/Schemes/JwtBearerScheme.cs`.

## Motivação

Hoje, quando um comando ou evento de domínio falha em produção, a única forma de descobrir é abrir
o banco. Não há como listar o que está com erro, ver o payload, nem reprocessar — o
`CommandsProcessor` só faz retry automático quando o comando foi criado com `MaxRetries > 0`
(`CommandsProcessor.cs:69`), e o `DomainEventsProcessor` não faz retry nenhum: evento que falha
fica `Error` para sempre.

O mesmo vale para a visão de negócio: não existe nenhuma forma, pela API, de saber quantas
instituições existem, quantos usuários cada uma tem ou quando foram criadas.

## Escopo

Três eixos:

1. **Observar** — listar instituições, usuários, eventos de domínio, comandos e lotes, com filtros
   e paginação, atravessando instituições.
2. **Diagnosticar** — filtrar por `Status = Error`, por tipo, por instituição e por janela de
   tempo; ver o payload (`Data`) e o erro (`Error`) completos.
3. **Agir** — reprocessar comandos e eventos com erro, individualmente ou em lote.

Fora de escopo na v1: mutação de dados de negócio (criar/editar instituição, resetar senha de
usuário, apagar registros), frontend de admin, purge/retenção de `commands` e `domain_events`.

## Arquitetura: host separado (a decisão central)

O `Mocks/` já estabelece o padrão no repositório: um `Microsoft.NET.Sdk.Web` com `Program.cs`
próprio, `<ProjectReference Include="..\Back\Back.csproj" />`, host mínimo
(`AddHttpConfigs` + `AddEntityFrameworkConfigs`) e um feature provider que **mantém só os próprios
controllers**, descartando os do Back que entram pela referência:

```csharp
// espelho de RegisterOnlyMocksControllersFeatureProvider
public sealed class RegisterOnlyAdminControllersFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        for (int i = feature.Controllers.Count - 1; i >= 0; i--)
            if (!feature.Controllers[i].AssemblyQualifiedName.StartsWith("Estud.Admin"))
                feature.Controllers.RemoveAt(i);
    }
}
```

O que isso resolve **por construção**, sem toggle de runtime:

- **Não sobe no deploy da API.** Os controllers de admin estão num binário diferente. A API pública
  (`Back`) não os contém — não há flag para configurar errado, não há rota bloqueada por middleware
  que possa vazar se a ordem quebrar. A rota simplesmente não existe no processo público.
- **Fora das docs, de graça.** O ApiExplorer do Back nunca vê os endpoints de admin porque não
  estão no assembly dele. Some a necessidade de `[ApiExplorerSettings(IgnoreApi = true)]`, de
  classe base disciplinadora e do teste de regressão "nenhuma `admin/` no swagger".
- **Superfície de auth mínima.** O host de admin registra **só** o `AdminBearerScheme`. Os cinco
  caminhos de login do app (social, SSO OIDC, magic link, One Tap, reset de senha) que terminam
  emitindo um token do `JwtBearerScheme` **não existem no processo de admin**.

### O que o host de admin reusa (via referência)

- **`EstudDbContext` inteiro**, com os interceptors de audit e de domain events (que vivem no
  `OnConfiguring`, `EstudDbContext.cs:28`) — vêm junto, sem reconfigurar.
- Entidades, `EstudError` + `OneOf`/`Match`, extensions (`HasValue`/`IsEmpty`), `Serialize`, etc.
- A factory de reprocessamento de comandos (extraída, ver *Reprocessamento*).

### O que o host de admin refaz (pequeno, como no Mocks)

- **`Program.cs`** próprio, enxuto.
- **EF completa, não a magra do Mocks.** O `Mocks/Configs/EntityFrameworkConfigs.cs` só chama
  `AddDbContext<EstudDbContext>()` e se safa porque os mocks não tocam o banco — o construtor do
  `EstudDbContext` exige `NpgsqlDataSource`. O admin consulta o banco pesado, então precisa
  espelhar a EF **do Back** (`EntityFrameworkConfigs.cs`): registrar o `NpgsqlDataSource` singleton
  **e** o `AddDbContext`, mais `AddDapperConfigs` (para o `admin/stats`).
- **Auth própria:** `AdminBearerScheme` + policy `Admin` + rate limiting próprio.
- **Pipeline mínimo:** sem `EnrichBackDbContextMiddleware` (logo `RequestUser` fica `0/0`, que é o
  que se quer para leitura cross-tenant) e sem `BackgroundProcessorsTriggerMiddleware`.
- **Infra de deploy:** `Dockerfile.admin`, serviço no Railway, entrada no CI.

### A "variável de ambiente" do pedido original, reenquadrada

O pedido inicial era "via env var, incluir os endpoints no deploy da API; por default não sobem".
Com o host separado, **o deployable é o próprio toggle**: você decide se roda o serviço de admin.
Isso é uma forma mais forte do mesmo objetivo — "admin não exposto publicamente por padrão" deixa
de ser config que não pode vazar e vira fato estrutural. Não há `Admin:Enabled`; a ausência do
serviço é o desligado. O que sobra de env var é o **segredo** do host de admin (chave de assinatura
e credenciais), não um flag de inclusão.

## Superfície de endpoints

Servidos na URL própria do serviço de admin. O prefixo `admin/` nas rotas é convenção (redundante
num host dedicado, mas ajuda caso um dia seja reverse-proxied sob domínio compartilhado).

| Método | Rota | O que faz |
|---|---|---|
| `POST` | `admin/login` | Emite o token de admin |
| `GET` | `admin/stats` | Contadores globais: instituições, usuários, comandos e eventos pendentes/com erro |
| `GET` | `admin/institutions` | Lista paginada, filtro por nome e data de criação, com contagem de usuários |
| `GET` | `admin/institutions/{id}` | Detalhe: config, contagens por tipo de usuário, cursos, turmas |
| `GET` | `admin/users` | Lista paginada, filtro por instituição, tipo, email e status de confirmação |
| `GET` | `admin/domain-events` | Lista paginada, filtro por `status`, `type`, `institutionId`, `entityUid`, janela de tempo |
| `GET` | `admin/domain-events/{id}` | Detalhe com `Data`, `Error`, `ActivityId`, duração |
| `POST` | `admin/domain-events/{id}/reprocess` | Reenfileira o evento |
| `POST` | `admin/domain-events/reprocess` | Reenfileira um conjunto de eventos (lista de ids, com teto) |
| `GET` | `admin/commands` | Lista paginada, filtro por `status`, `type`, `institutionId`, `batchId`, janela de tempo |
| `GET` | `admin/commands/{id}` | Detalhe com `Data`, `Error`, `Logs`, linhagem (`ParentId`, `OriginalId`, `BatchId`) |
| `POST` | `admin/commands/{id}/reprocess` | Cria a cópia de reprocessamento |
| `POST` | `admin/commands/reprocess` | Reprocessa um conjunto de comandos (lista de ids, com teto) |
| `GET` | `admin/command-batches` | Lista paginada de lotes, filtro por status e instituição |

Cada endpoint segue a vertical slice do `CLAUDE.md` (`Controller`/`Service`/`In`/`Out`), com as
diferenças da seção *Convenções que mudam*.

### Filtros de erro — o caso de uso principal

`GET admin/commands?status=Error&from=2026-07-01` e `GET admin/domain-events?status=Error` são o
motivo do plano existir. Ambos precisam de índice — e esses índices ficam **no Back**, junto dos
`DbConfig`s das tabelas (`Back/Database/Commands/CommandDbConfig.cs` e
`Back/Database/DomainEvents/DomainEventDbConfig.cs`), porque a migração é compartilhada:

```csharp
entity.HasIndex(e => new { e.Status, e.CreatedAt });   // commands
entity.HasIndex(e => new { e.Status, e.OccurredAt });  // domain_events
```

Ordenação padrão: mais recente primeiro. Paginação idêntica à de `GetClasses`
(`GetClassesService.cs:10`): `Page`/`PageSize` com `Math.Clamp(query.PageSize, 1, MaxPageSize)`.

## Autenticação dedicada

O esquema dedicado deixou de ser uma decisão a defender (era, no desenho in-process) e passou a ser
**consequência do host separado**: o processo de admin só registra o que precisa. Ainda assim, as
três razões de fundo continuam válidas e explicam por que um token do app **não** deve valer aqui:

1. **O admin não pertence a uma instituição.** Todo `EstudUser` tem `InstitutionId`, e todo service
   escopa por `ctx.RequestUser.InstitutionId`. Um admin cross-tenant não tem valor válido para esse
   campo — no host separado ele naturalmente fica `0`, porque não há middleware de enriquecimento.
2. **Chave própria.** O `Auth:SecurityKey` assina o token de todo aluno da plataforma. O admin usa
   `Admin:SecurityKey`, sem relação — se a do app vazar, forjar admin continua impossível.
3. **Só um caminho de login.** O único jeito de obter um token de admin é `admin/login`, no host de
   admin. Nenhum dos fluxos de login do app leva a ele.

### `AdminBearerScheme`

```csharp
public static class AdminBearerScheme
{
    public const string Name = "AdminBearer";
    public const string Cookie = "X-Estud-AdminBearerCookie";

    public static AuthenticationBuilder AddAdminBearerScheme(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        var settings = configuration.Admin;

        return builder.AddJwtBearer(Name, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,   ValidIssuer = "estud-admin",
                ValidateAudience = true, ValidAudience = "estud-admin",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SecurityKey)),
                ValidAlgorithms = ["HS256"],
                ValidateLifetime = true, ClockSkew = TimeSpan.Zero,
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies[Cookie];
                    if (token.HasValue()) context.Token = token;
                    return Task.CompletedTask;
                }
            };
        });
    }
}
```

- **Issuer/audience próprios** (`estud-admin`): um token do app nunca valida aqui, e vice-versa.
- **Expiração curta** — `Admin:ExpirationTimeInMinutes`, default `60`.
- **Claims próprias** — `admin_id`, `admin_email`, `jti`. Nada de `sub`, `inst`, `prms`, `type`.

A policy vive no host de admin (não passa pelas sobrecargas de `Policies.cs`, todas amarradas ao
`JwtBearerScheme.Name`):

```csharp
builder.AddPolicy("Admin", policy => policy
    .RequireAuthenticatedUser()
    .AddAuthenticationSchemes(AdminBearerScheme.Name));
```

Todo controller de admin herda de uma base pequena — não para esconder das docs (isso já é grátis),
mas para não repetir `[Authorize]` e garantir que nenhum controller de admin suba sem policy:

```csharp
[ApiController]
[Authorize("Admin")]
public abstract class AdminControllerBase : ControllerBase;
```

### De onde vem a identidade do admin (v1)

Sem tabela nova: `Admin:Users` é uma lista de `{ Email, PasswordHash }` vinda de configuração (env
var no Railway), com hash PBKDF2 verificado pelo `PasswordHasher<T>` que o Identity já traz.
`POST admin/login` valida a credencial e emite o token.

Motivo de não criar `admin_users` agora: uma tabela própria arrasta hashing, lockout, troca de
senha e reset — todo o trabalho que o ASP.NET Identity faz para `EstudUser`. O gatilho para migrar
é claro: **mais de ~5 admins, ou a primeira necessidade de revogar um acesso sem redeploy**. Aí a
tabela entra e o `admin/login` troca de fonte, sem tocar em esquema, policy ou endpoint.

Cuidados no `admin/login`, com precedente no repositório:
- rate limiting sensível (ver abaixo);
- resposta genérica para credencial inválida (sem distinguir email inexistente de senha errada);
- log em `Warning` de toda tentativa falha, com IP.

## Fora das docs

Grátis, por construção: os controllers de admin não estão no assembly do Back, então nem o
Swashbuckle nem o `MapOpenApi` do Back os enxergam. Nenhum atributo, nenhum filtro, nenhum teste de
regressão necessário no Back.

O host de admin, por sua vez, **pode** expor um Scalar próprio para conveniência do operador — sem
risco de vazamento, já que o serviço não é público. Fica como decisão em aberto; na v1, sem docs
(curl/Insomnia), para manter o host mínimo.

## Reprocessamento

Comandos e eventos têm semânticas **diferentes**, e o plano segue o que cada modelo já suporta.

### Comandos — cópia, nunca mutação

O `Command` já é imutável por design: o retry automático cria um comando novo apontando para o
original (`CommandsProcessor.cs:95`, `Command.OriginalId`). O reprocessamento manual faz o mesmo,
para que o histórico da falha continue inspecionável.

`CommandsProcessor.CreateRetryCommand` é `private static` hoje. Extrair para
`Back/Extensions/CommandsExtensions.cs` como factory parametrizada, usada pelos dois caminhos (o
retry automático no Back e o reprocessamento no host de admin, que a acessa pela referência):

```csharp
public static Command CreateReprocessCommand(this Command failed, string activityId, int? delaySeconds)
```

Diferenças do reprocessamento manual em relação ao retry automático:

- `delaySeconds: null` — quem apertou o botão quer agora, não daqui a `BaseDelaySeconds * 2^n`;
- `maxRetries` preservado do original (não decrementado): nova tentativa autorizada por uma pessoa;
- `RetryAttempt = failed.RetryAttempt + 1`, mantendo a linhagem legível.

Pré-condição: só `Status = Error`. Reprocessar um `Pending` ou `Success` retorna
`CommandIsNotInErrorStatus.I`.

### Eventos de domínio — reset no lugar

`DomainEvent` não tem `OriginalId`, `MaxRetries` nem `Logs`, e o `DomainEventsProcessor` executa
todos os handlers de um evento na **mesma transação** — se um falha, nenhum comando é criado. É
essa propriedade tudo-ou-nada que torna o reset seguro: não há efeito parcial para duplicar.

```csharp
evt.Status = DomainEventStatus.Pending;
evt.ProcessorId = null;
evt.ProcessedAt = null;
evt.Error = null;
```

O `Error` **precisa** ser limpo: `DomainEvent.Processed()` decide o status final por
`Error.HasValue()`, então um erro remanescente marcaria como falho um reprocessamento
bem-sucedido. Limpar o `Error` perde a mensagem original — daí a `admin_actions` (abaixo) guardar o
erro anterior.

### Quem executa o reprocessamento — atenção ao host separado

O host de admin **grava a linha** (o comando-cópia ou o reset do evento), mas quem tem os jobs
Quartz é o processo da **API** (`Back`). No desenho in-process, o `BackgroundProcessorsTrigger`
dispararia o job na hora; aqui não há esse middleware nem os jobs no processo de admin. Duas
consequências:

- O reprocessamento é **assíncrono entre processos**: a API pega a linha no próximo ciclo de
  polling (`CommandsPollingIntervalInSeconds`/`DomainEventsPollingIntervalInSeconds`, 60s em prod).
  Latência aceitável para uma ação de operação; documentar na resposta do endpoint que "foi
  reenfileirado", não "foi processado".
- Não é preciso `HasPendingCommands`/`HasPendingDomainEvents` no host de admin — esses flags só
  servem ao trigger in-process, que não existe aqui. A API descobre o trabalho pelo polling.

### Registro das ações — `admin_actions`

Toda ação de escrita do admin grava uma linha. A entidade e o mapeamento vivem **no Back**, porque
o `EstudDbContext` (que o admin reusa) define seu modelo no Back — a tabela é só mais uma no
contexto compartilhado, e a migração sai junto das demais:

```csharp
public class AdminAction
{
    public int Id { get; set; }
    public string AdminEmail { get; set; }
    public string Action { get; set; }          // ReprocessCommand, ReprocessDomainEvent
    public string TargetType { get; set; }
    public int TargetId { get; set; }
    public int? InstitutionId { get; set; }     // da entidade afetada
    public string? Payload { get; set; }        // inclui o Error anterior
    public DateTime CreatedAt { get; set; }
}
```

Por que não reusar o Audit.NET: `AuditConfigs.cs:29` preenche o trail a partir de
`dbContext.RequestUser` — que, num request de admin, é `0/0` de propósito. Fazer o audit funcionar
exigiria forjar um `RequestUser` cross-tenant. Uma tabela própria de 8 colunas é mais barata e mais
honesta; o `AdminEmail` vem da claim do token, setado pelo próprio service.

### Lote

`POST admin/commands/reprocess` e `admin/domain-events/reprocess` recebem lista de ids, com teto de
**100** por chamada (validação → `TooManyItemsToReprocess.I`). Sem filtro-como-alvo ("reprocessa
tudo que está com erro"): o operador lista, escolhe e envia os ids.

## Rate limiting

Simples no host separado: o admin tem pipeline próprio e define seu **próprio** limiter,
particionado por `admin_id` (do token), mais a partição sensível por IP no `admin/login`. Não há o
problema do balde `user_0` compartilhado do limiter global do Back (`RateLimitingConfigs.cs:19`),
porque o Back não está no caminho.

## Convenções que mudam para o admin

O `CLAUDE.md` descreve o padrão dos endpoints de produto. No admin, como não há docs geradas:

| Convenção | No admin |
|---|---|
| `SwaggerResponseExample` + `ExamplesProvider` + `IApiDto<T>.GetExamples()` | Não se aplica (sem docs na v1). DTOs de admin são POCOs simples |
| Comentários XML `<summary>`/`<remarks>` | Mantidos — servem a quem lê o código |

O que **não** muda: vertical slice (`Admin/Features/<Feature>/`), `IEstudService`, `Validator`
privado com `V.Run`, `OneOf<TOut, EstudError>`, `result.Match<IActionResult>(Ok, BadRequest)`, LINQ
em method syntax, `HasValue()`/`IsEmpty()`.

Erros novos (no host de admin, derivando do `EstudError` do Back): `InvalidAdminCredentials`,
`CommandNotFound`, `DomainEventNotFound`, `CommandIsNotInErrorStatus`,
`DomainEventIsNotInErrorStatus`, `TooManyItemsToReprocess`.

## O que muda em cada projeto

### `Back` (poucas alterações)

- `Back/Extensions/CommandsExtensions.cs` — `CreateReprocessCommand` extraído de
  `CommandsProcessor` (com o retry automático passando a usá-la).
- `Back/Commands/CommandsProcessor.cs` — usa a factory extraída.
- `Back/Database/Commands/CommandDbConfig.cs` e `Back/Database/DomainEvents/DomainEventDbConfig.cs`
  — índices de filtro `(status, created_at)` / `(status, occurred_at)`.
- `Back/Domain/Admin/AdminAction.cs`, `Back/Database/Admin/AdminActionDbConfig.cs`,
  `Back/Database/EstudDbContext.Admin.cs` (`DbSet` + `ConfigureAdmin`) — a tabela vive no contexto
  compartilhado.

O Back **não** ganha: controllers de admin, esquema, policy, settings de admin, nem alteração de
`Program.cs`/`AuthenticationConfigs`/`AuthorizationConfigs`/`RateLimitingConfigs`.

### `Admin` (projeto novo, molde do `Mocks`)

| Arquivo | Papel |
|---|---|
| `Admin/Admin.csproj` | `Sdk.Web`, `<ProjectReference>` para o Back |
| `Admin/Program.cs` | Host enxuto (http, EF completa, auth, rate limiting, controllers) |
| `Admin/Configs/HttpConfigs.cs` | `AddControllers` + `RegisterOnlyAdminControllersFeatureProvider` |
| `Admin/Configs/EntityFrameworkConfigs.cs` | `NpgsqlDataSource` + `AddDbContext` + Dapper (EF **completa**, não a magra do Mocks) |
| `Admin/Configs/AuthConfigs.cs` | `AdminBearerScheme` + policy `Admin` + rate limiting |
| `Admin/Providers/RegisterOnlyAdminControllersFeatureProvider.cs` | Mantém só controllers `Estud.Admin` |
| `Admin/Auth/AdminBearerScheme.cs` | Esquema dedicado |
| `Admin/Auth/AdminControllerBase.cs` | `[ApiController]` + `[Authorize("Admin")]` |
| `Admin/Settings/AdminSettings.cs` | `SecurityKey`, `ExpirationTimeInMinutes`, `Users[]` + validações de boot |
| `Admin/Errors/EstudErrors.Admin.cs` | Erros do admin |
| `Admin/Features/AdminLogin/` | Login e emissão do token |
| `Admin/Features/GetAdminStats/` | Contadores globais (Dapper, uma query) |
| `Admin/Features/GetAdminInstitutions/` | Lista + detalhe |
| `Admin/Features/GetAdminUsers/` | Lista cross-tenant |
| `Admin/Features/GetAdminDomainEvents/` | Lista + detalhe |
| `Admin/Features/GetAdminCommands/` | Lista + detalhe |
| `Admin/Features/GetAdminCommandBatches/` | Lista |
| `Admin/Features/ReprocessCommand/` | Individual + lote |
| `Admin/Features/ReprocessDomainEvent/` | Individual + lote |
| `Admin/appsettings.*.json` | Connection string + seção `Admin` |
| `Dockerfile.admin` | Publica o `Admin.csproj` |

### `AdminSettings` — validações de boot

- **Fail fast quando o serviço sobe:** `RequireNonEmpty(SecurityKey)` e pelo menos um usuário; senão
  o host não inicia. Não há flag `Enabled` — rodar o serviço já é a decisão de ligar.
- **Segredo fora do versionamento:** nenhum `appsettings.*.json` versionado traz `SecurityKey`
  real, exceto `Admin/appsettings.Testing.json` (chave de teste).
- **Log no boot** — uma linha em `Warning` quando o host de admin sobe, para aparecer no Serilog.

## Testes

Os testes do repositório usam `WebApplicationFactory<Back::Program>` (`BackFactory`, com
`extern alias Back`). O admin ganha um `AdminFactory` (`WebApplicationFactory<Admin::Program>`,
`extern alias Admin`) apontando para o **mesmo** `estud-tests-db`. O padrão de seed continua sendo
pelos endpoints do Back (`BackFactory.LoggedAsAcademic()` etc.); os asserts de admin batem no
`AdminFactory`. Como os dois compartilham o banco, o fluxo é: semear via Back → operar/consultar via
Admin.

Regiões do `CLAUDE.md`:

- **Authentication** — sem token → 401; **com token válido do app** (`JwtBearerScheme`, emitido pelo
  Back) → 401 no host de admin. Esse segundo caso prova a separação de auth e é o teste mais
  importante.
- **Authorization** — token de admin expirado ou assinado com `Auth:SecurityKey` → 401.
- **Validation errors** — reprocessar comando inexistente → `CommandNotFound.I`; reprocessar comando
  `Success` → `CommandIsNotInErrorStatus.I`; lote acima de 100 → `TooManyItemsToReprocess.I`.
- **Happy path** — criar dados em duas instituições diferentes (dois logins no Back) e afirmar que
  `admin/institutions` e `admin/users` retornam as duas (prova do cross-tenant); forçar um comando
  com erro (`TestRetryCommand` já existe em `Back/Commands/`), filtrar por `status=Error`,
  reprocessar, `await _back.AwaitCommandsProcessing()` (a API/factory do Back é quem processa),
  afirmar o comando novo com `OriginalId` preenchido e a linha em `admin_actions`.

Fora das features:

- **A API pública não serve admin** — bater `admin/stats` e `admin/login` no `BackFactory` → 404.
  Prova estrutural de que os endpoints não estão no processo público.
- **Guarda de colocação errada** — teste unitário afirmando que o assembly do Back não contém
  controller com rota `admin/*` (pega alguém que crie um controller de admin no projeto errado).
- **Boot do host de admin** — `AdminFactory` sem `SecurityKey` ou sem `Users` derruba o boot.

`Admin/appsettings.Testing.json` traz a connection string do banco de testes, uma `SecurityKey`
própria e um usuário de teste.

## Decisões tomadas

| Decisão | Resolução |
|---|---|
| Onde o admin vive | **Projeto/host separado** (`Admin.csproj`) referenciando o Back, molde do `Mocks/` |
| Como não sobe no deploy da API | Está noutro binário; a API pública não contém os controllers — estrutural, sem flag |
| Fora das docs | Grátis: não está no assembly do Back. Scalar próprio do admin fica opcional |
| Esquema de autenticação | Dedicado (`AdminBearer`): chave, issuer/audience, expiração e claims próprios; único no host de admin |
| Identidade do admin | v1 por configuração (`Admin:Users`, hash PBKDF2); tabela `admin_users` quando passar de ~5 admins ou surgir revogação sem redeploy |
| Escopo multi-tenant | `RequestUser` fica `0/0` no host de admin (sem middleware de enriquecimento) — o que se quer para leitura cross-tenant |
| Onde vivem as tabelas novas | `admin_actions` no `EstudDbContext` (Back), por ser contexto compartilhado |
| Reprocessar comando | Cópia com `OriginalId`, sem delay, `maxRetries` preservado; só a partir de `Error` |
| Reprocessar evento | Reset no lugar (`Pending`, `ProcessorId`/`ProcessedAt`/`Error` nulos) |
| Quem processa | Os jobs Quartz da **API**; reprocessamento é assíncrono entre processos (latência do polling) |
| Rastro das ações | Tabela `admin_actions` própria, não o Audit.NET (que depende de `RequestUser`) |
| Lote | Lista explícita de ids, teto de 100; sem "reprocessar tudo por filtro" |
| Rate limiting | Limiter próprio do host de admin, por `admin_id`; sem o balde `user_0` do Back |
| Docs/exemplos nas features | Sem `IApiDto<T>`/`ExamplesProvider`; comentários XML mantidos |

## Decisões em aberto

| Decisão | Opções | Sugestão |
|---|---|---|
| Scalar no host de admin | sem docs × Scalar próprio para o operador | Sem docs na v1 (host mínimo); Scalar quando a superfície crescer — sem risco, o serviço não é público |
| 2FA no `admin/login` | senha só × TOTP obrigatório | Senha na v1; TOTP quando houver tabela de admins (o `SetupTwoFactor` do Back dá para reaproveitar) |
| Permissões dentro do admin | admin único × leitura × operação | Único na v1; dois níveis (`read`/`operate`) quando houver mais de um operador |
| Latência do reprocessamento | esperar o polling da API × trigger cross-process | Esperar o polling na v1 (60s); se incomodar, um endpoint interno na API que o admin chama para disparar o job |
| Allowlist de IP / rede privada | não × sim | Não na v1; o serviço separado já reduz a superfície, e IP fixo atrapalha operação |
| Frontend de admin | nenhum × app separado × rota no Nuxt | Nenhum na v1 (curl/Insomnia); se vier, app separado — o Nuxt atual autentica pelo cookie do app |
| Purge de `commands`/`domain_events` | manter tudo × job de retenção | Fora de escopo; quando vier, o admin ganha a visão do que foi purgado |
| `DomainEvent.Logs` | não existe × espelhar `Command.Logs` | Adicionar (no Back) se o diagnóstico de eventos se mostrar cego sem isso |

## Ordem de implementação

1. **No Back** (sem nada de admin ainda): `CreateReprocessCommand` extraído de `CommandsProcessor`
   (testes de comando existentes seguem verdes); índices de filtro nos dois `DbConfig`s;
   `AdminAction` + `DbConfig` + `EstudDbContext.Admin.cs`.
2. **Esqueleto do projeto `Admin`**: `.csproj` com referência ao Back, `Program.cs`, `HttpConfigs`
   com o `RegisterOnlyAdminControllersFeatureProvider`, EF completa. Sobe vazio, sem endpoints.
3. `AdminSettings` + validações de boot; `AdminBearerScheme` + policy + `AdminControllerBase`.
4. `AdminLogin` (emissão do token) + `AdminFactory` nos testes + testes de autenticação —
   incluindo o token do app sendo recusado e o `BackFactory` respondendo 404 em `admin/*`. É o
   ponto em que a separação fica provada, antes de existir qualquer dado exposto.
5. Leituras: `GetAdminStats`, `GetAdminInstitutions`, `GetAdminUsers`.
6. Leituras de background: `GetAdminCommands`, `GetAdminDomainEvents`, `GetAdminCommandBatches`.
7. `ReprocessCommand` (individual + lote) gravando `admin_actions`.
8. `ReprocessDomainEvent` (individual + lote).
9. Rate limiting próprio do host de admin.
10. Infra: `Dockerfile.admin`, serviço no Railway, entrada no CI (espelhando o que existe para o
    Back e, se houver, para o Mocks).
11. SQL de produção: tabela `admin_actions` + os dois índices.
12. Documentar no `CLAUDE.md`: o admin é o host `Admin/` (molde do `Mocks/`); features herdam de
    `AdminControllerBase` e não usam `IApiDto<T>`/`ExamplesProvider`.
