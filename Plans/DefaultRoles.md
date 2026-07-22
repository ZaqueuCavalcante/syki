# Plano — Roles default por instituição

Hoje as 4 roles default (Diretor, Professor, Aluno, Responsável) são **linhas globais compartilhadas** por todas as instituições (`OwnerId = null`), seedadas uma única vez. Este plano passa a **criar 4 roles próprias por instituição na criação dela**, clonando os valores de `EstudDefaultRoles`. Com isso `EstudRole.OwnerId` deixa de ser `int?` e vira um `int InstitutionId` não-nulo.

**Ganhos:**
- Customização real das roles default por instituição: renomear, mudar permissões/descrição e — principalmente — o flag `TwoFactorRequired` passam a valer para Diretor/Professor/Aluno/Responsável (hoje **não valem**, ver abaixo).
- Modelo uniforme: some o caso especial `OwnerId == null`; FK e índice sem edge case de null.
- Isolamento multi-tenant correto: nenhuma linha mutável compartilhada entre tenants.
- O diretor abre a tela de perfis e já vê 4 perfis reais e editáveis.

---

## Estado atual do sistema (base para o plano)

### Domínio e persistência
- **`Back/Domain/Identity/EstudRole.cs`**: `OwnerId` é `int?`. Tem ctor `(int ownerId, name, description, baseType, permissions)`, `Update(...)` (que hoje **também altera `BaseType`**), `SetTwoFactorRequired(bool)`, `TwoFactorRequired`.
- **`Back/Auth/Roles/EstudDefaultRoles.cs`**: 4 templates estáticos, cada um com `OwnerId = null`, `Name`/`NormalizedName`/`Description`/`BaseType`/`Permissions`. Professor/Aluno/Responsável têm `Permissions = []`.
- **`Back/Database/Identity/EstudRoleDbConfig.cs`**: `HasOne<Institution>().WithMany().HasForeignKey(e => e.OwnerId)` (FK **opcional**, sem navigation collection no `Institution`). Índice único composto `(OwnerId, NormalizedName)` com `AreNullsDistinct(false)` (NULLS NOT DISTINCT, PG 15+).
- **`Back/Domain/Institutions/Institution.cs`**: **não** tem `List<EstudRole> Roles`. Criada via `NewForUserRegister()`.
- **`Back/Domain/Identity/EstudUserRole.cs`**: ctors tomam `int roleId`; tem navigation `Role`. Não há ctor que receba a entidade `EstudRole`.

### Getters cacheados (globais)
- **`Back/Database/EstudDbContext.Identity.cs`**: `GetDirectorRole/GetTeacherRole/GetStudentRole/GetParentRole()` — cada um faz `Where(x => x.OwnerId == null && x.NormalizedName == EstudDefaultRoles.X.NormalizedName)` e cacheia com **uma chave global** (`CacheKeys.Get*Role`, expiração 100 dias).
- **`Back/Cache/CacheKeys.cs`**: `GetParentRole/GetStudentRole/GetTeacherRole/GetDirectorRole`.

### Consumidores dos getters
- **Diretor de instituição nova** (3 fluxos, mesmo padrão): `Back/Features/Users/RegisterUser/RegisterUserService.cs:29`, `Back/Features/Identity/GoogleOneTapLogin/GoogleOneTapLoginService.cs:64`, `Back/Auth/Schemes/SocialLoginScheme.cs:140`. Todos: `Institution.NewForUserRegister()` → `GetDirectorRole()` → `new EstudUserRole(institution, user, directorRole.Id)` → `ctx.AddRange(institution, ...)`.
- **Atribuição por tipo**: `CreateTeacherService.cs:18` (`GetTeacherRole`), `CreateStudentService.cs:34` (`GetStudentRole`), `CreateParentService.cs:40` (`GetParentRole`). Cada um cria o usuário + `EstudUserRole(institutionId, user, role.Id)`.

### Roles CRUD
- **`CreateRoleService.cs`**: valida `BaseType.IsInEnum()`; cria `new EstudRole(institutionId, ...)`; conflito de nome por `OwnerId == institutionId && NormalizedName`.
- **`UpdateRoleService.cs`**: valida `BaseType.IsInEnum()` e **passa `data.BaseType` para `role.Update(...)`** → hoje `BaseType` é editável (a corrigir).
- **`GetRolesService.cs` / `GetRoleService.cs`**: filtram por `OwnerId == ctx.RequestUser.InstitutionId`.
- **`SetTwoFactorEnforcementService.cs:9`**: `Where(r => r.OwnerId == institutionId && r.Id == data.RoleId)`.
- **Não existe `DeleteRole`.**

> ⚠️ **Limitação atual que este plano corrige:** `SetTwoFactorEnforcement` só acha roles com `OwnerId == institutionId`, e as default têm `OwnerId == null`. Ou seja, **hoje é impossível forçar 2FA nas roles Diretor/Professor/Aluno/Responsável**. Cf. `Plans/2FAEnforcement.md`.

### Bootstrap / testes
- **Sem migrations EF commitadas** (não há pasta `Migrations`, `ModelSnapshot`, nem `MigrateAsync`). Prod aplica SQL gerado à parte (`Back/Database/Migrations.md`).
- **`Tests/Base/IntegrationTestBase.cs`**: `EnsureDeletedAsync` + `EnsureCreatedAsync`, depois `new DataSeeder(ctx).Run()`.
- **`Tests/Seed/DataSeeder.cs`**: `SeedDefaultRoles()` insere as 4 roles globais (`OwnerId = null`). É delas que `LoggedAsDirector`→`RegisterUser`→`GetDirectorRole()` depende.

---

## Decisões de design

### D1 — Onde criar as 4 roles: no domínio `Institution` (navigation + cascade)
Adicionar `List<EstudRole> Roles` em `Institution` e popular no construtor a partir dos templates de `EstudDefaultRoles`. Como os 3 fluxos de criação de instituição já fazem `ctx.AddRange(institution, ...)`, o cascade do EF persiste as 4 roles junto — **um único ponto** cobre `RegisterUser`, `GoogleOneTapLogin` e `SocialLoginScheme`. O `OwnerId`/`InstitutionId` de cada role é preenchido pelo EF a partir do relacionamento (não precisa do Id antes do `SaveChanges`).

### D2 — Resolução da role a atribuir (aluno/professor/responsável): por `BaseType` + contagem, sem flag
Não é preciso um marcador "isDefault". A role a atribuir é resolvida por `BaseType`:
- **`RoleId` informado** → valida que pertence à instituição **e** que `role.BaseType` bate com o tipo da entidade sendo criada; senão erro.
- **`RoleId` vazio** → conta roles daquele `BaseType` na instituição: exatamente **1** → usa essa; **>1** → erro (`AmbiguousRole`, o front deveria ter perguntado — backend **não chuta**); **0** → erro (não deve ocorrer pela invariante D3).

### D3 — Invariante "sempre ≥1 role de cada `BaseType`"
Sustentada por: (a) as 4 default são criadas na criação da instituição (D1); (b) **`BaseType` é imutável no update** (removido de `UpdateRole`); (c) **não há `DeleteRole`**. Se um `DeleteRole` for criado no futuro, ele **precisa** barrar excluir o último perfil de um `BaseType` (ou tornar as default indeletáveis) — registrar como invariante explícita.

### D4 — Remover os getters globais e as cache keys
Com as roles criadas em memória como parte do `Institution` (D1) e a atribuição por tipo resolvida via D2, `GetDirectorRole/GetTeacherRole/GetStudentRole/GetParentRole()` e as 4 `CacheKeys.Get*Role` deixam de ser necessários. O diretor do 1º acesso é pego de `institution.Roles` (o único `Manager` no momento da criação).

---

## Backend

### B1. `EstudRole`: `OwnerId int?` → `InstitutionId int`
- `Back/Domain/Identity/EstudRole.cs`: trocar `public int? OwnerId` por `public int InstitutionId` (não-nulo). Ajustar ctor e o construtor sem parâmetros usado pela criação via navigation.
- Ajustar **todos** os `Where`/atribuições que hoje usam `OwnerId`: `CreateRoleService`, `UpdateRoleService`, `GetRoleService`, `GetRolesService`, `SetTwoFactorEnforcementService`.
- **`EstudRoleDbConfig.cs`**: FK passa a `IsRequired()`; `HasForeignKey(e => e.InstitutionId)` com `WithMany(i => i.Roles)` (ver B2). Índice composto `(InstitutionId, NormalizedName)` volta a poder usar `AreNullsDistinct` padrão (não há mais null); manter `IsUnique()`. Decidir nome da coluna (ver Perguntas em aberto).

### B2. `Institution`: navigation `Roles` + criação na construção
- `Back/Domain/Institutions/Institution.cs`: adicionar `public List<EstudRole> Roles { get; set; }`. No ctor `Institution(string name)`, popular `Roles` clonando os templates de `EstudDefaultRoles` (ver B3) — **sem** setar `InstitutionId` (o EF resolve pelo relacionamento no `SaveChanges`).
- Expor um acessor conveniente para o diretor, ex. `public EstudRole DirectorRole => Roles.First(r => r.BaseType == UserType.Manager);` (usado em B4).
- `EstudRoleDbConfig`: `HasOne<Institution>().WithMany(i => i.Roles).HasForeignKey(e => e.InstitutionId)`.

### B3. `EstudDefaultRoles`: templates puros + clone
- Remover `OwnerId = null` dos templates (viram só dados: `Name`/`NormalizedName`/`Description`/`BaseType`/`Permissions`).
- Adicionar uma coleção/`All` e um método de clone que gera `EstudRole` novas (novo `ConcurrencyStamp` por role, `Permissions` copiada para nova lista) — consumido pelo ctor de `Institution` (B2) e pela migração (B9).

### B4. Fluxos de criação de instituição: pegar o diretor de `institution.Roles`
Nos 3 fluxos (`RegisterUserService`, `GoogleOneTapLoginService`, `SocialLoginScheme`):
- Remover `var directorRole = await ctx.GetDirectorRole();`.
- Atribuir o diretor pela role já criada em memória. Como o Id ainda é 0 antes do `SaveChanges`, **atribuir por navigation**, não por `roleId`. Duas opções:
  - Adicionar ctor `EstudUserRole(Institution institution, EstudUser user, EstudRole role)` que seta `Role = role` (EF resolve `RoleId` no save); ou
  - Setar `new EstudUserRole(institution, user, 0) { Role = institution.DirectorRole }`.
- Preferir o ctor novo por clareza: `new EstudUserRole(institution, user, institution.DirectorRole)`.

### B5. Remover getters globais e cache keys
- `EstudDbContext.Identity.cs`: remover `GetDirectorRole/GetTeacherRole/GetStudentRole/GetParentRole`.
- `CacheKeys.cs`: remover `GetDirectorRole/GetTeacherRole/GetStudentRole/GetParentRole`.

### B6. Resolver de role por tipo (para aluno/professor/responsável)
Método reutilizável (num serviço/extension do `ctx`), assinatura sugerida:
`Task<OneOf<EstudRole, EstudError>> ResolveRoleForUserType(UserType baseType, int? roleId)` — usando `ctx.RequestUser.InstitutionId`:
- `roleId != null` → `role = FirstOrDefault(r => r.InstitutionId == inst && r.Id == roleId)`; se `null` → `RoleNotFound`; se `role.BaseType != baseType` → `InvalidRoleForUserType`.
- `roleId == null` → `roles = Where(r => r.InstitutionId == inst && r.BaseType == baseType)`; `Count == 1` → usa; `> 1` → `AmbiguousRole`; `== 0` → `RoleNotFound` (invariante D3 evita).

### B7. `CreateTeacher/CreateStudent/CreateParent`
- **DTOs In** ganham `public int? RoleId { get; set; }` (opcional).
- Substituir `await ctx.Get*Role()` por `ResolveRoleForUserType(UserType.X, data.RoleId)`; propagar erro; usar a role resolvida em `new EstudUserRole(institutionId, user, role.Id)`.
- Novos erros: `InvalidRoleForUserType`, `AmbiguousRole` (em `Back/Errors/EstudInvalidErrors.cs`), reusar `RoleNotFound`. Adicionar aos `ErrorExamplesProvider<...>` dos 3 controllers.

### B8. `UpdateRole`: `BaseType` imutável
- Remover `RuleFor(x => x.BaseType).IsInEnum()...` do `UpdateRoleService`.
- Remover `BaseType` de `UpdateRoleIn` e do `role.Update(...)` (a assinatura de `EstudRole.Update` perde o parâmetro `baseType`).
- `CreateRole` **mantém** `BaseType` (é onde o tipo é definido). `InvalidRoleBaseType` passa a valer só no create.

### B9. Migração de dados (prod)
Sem migrations EF commitadas → script SQL (padrão `Migrations.md`), idempotente, em transação:
1. Para **cada** instituição, inserir 4 roles clonando os templates (`InstitutionId = <id>`, `TwoFactorRequired = false`).
2. **Re-apontar** `user_roles` que hoje referenciam as roles globais para os clones da instituição correspondente (casar por `BaseType`/`NormalizedName` + `InstitutionId` do `user_role`).
3. Apagar as 4 roles globais (`OwnerId IS NULL`).
4. Ajustes de schema: coluna `institution_id NOT NULL` + índice único novo (feito pelo DDL gerado do DbContext).
- Validar contagem: cada instituição com exatamente 4 roles default e nenhum `user_role` órfão antes de dropar as globais.

### B10. Testes — ajustar seeding
- `Tests/Seed/DataSeeder.cs`: **remover** `SeedDefaultRoles()` (as instituições criadas via `RegisterUser` já trazem suas 4 roles). Verificar todo ponto de teste que assumia as roles globais.
- Auditar `Tests/Base/BackFactory.*` (`LoggedAsDirector/Teacher/Student/...`) — devem seguir funcionando via os fluxos de criação, agora com roles próprias da instituição.

### B11. Testes de integração
- **Criação de instituição** (`RegisterUser` e social): após o registro, a instituição tem exatamente 4 roles (`InstitutionId` setado, uma de cada `BaseType`) e o usuário recebe a role `Manager`.
- **CreateTeacher/Student/Parent — happy path**: sem `RoleId` e só a default existindo → atribui a única do tipo.
- **Com `RoleId` válido do tipo certo** → atribui essa; **`RoleId` de outro tipo** → `InvalidRoleForUserType`; **`RoleId` inexistente/de outra instituição** → `RoleNotFound`.
- **Ambiguidade**: criar uma 2ª role do mesmo `BaseType` e chamar sem `RoleId` → `AmbiguousRole`.
- **UpdateRole**: não altera `BaseType` (campo removido); demais campos ok.
- **SetTwoFactorEnforcement**: agora consegue setar `TwoFactorRequired` numa role default da instituição (antes impossível).
- Métodos do `TestsHttpClient` retornando `OneOf<TOut, ErrorOut>` via `response.Resolve<TOut>()` para os asserts `ShouldBeError`/`.Success`.

---

## Frontend

### F1. Modais de criar aluno/professor/responsável — seleção condicional de perfil
- Buscar as roles da instituição filtradas pelo `BaseType` da entidade (reusar o GET de roles).
- **> 1 role** do tipo → exibir um `USelect`/`URadioGroup` obrigatório e enviar `RoleId` no POST.
- **exatamente 1** (só a default) → **não exibir nada**; POST vai sem `RoleId` e o backend resolve a única.
- Validação Zod: quando o select aparece, `RoleId` é obrigatório com `required_error` em PT (cf. `CLAUDE.md`).

### F2. Tela de perfis
- Já lista as roles da instituição — agora aparecem as 4 default + custom, todas editáveis.
- Na **edição**, `BaseType` fica **read-only/oculto** (imutável — B8). Na **criação** de role custom, continua selecionável.

### F3. Types
- Atualizar os tipos TS dos DTOs alterados (`CreateTeacher/Student/ParentIn` com `RoleId?`, `UpdateRoleIn` sem `BaseType`).

---

## Fluxos (resumo)

**Criar instituição (qualquer um dos 3 fluxos):** `new Institution(...)` já monta as 4 roles (clone dos templates) → `AddRange(institution, ...)` persiste tudo em cascade → usuário recebe a role `Manager` da própria instituição.

**Criar professor/aluno/responsável:** front busca roles do tipo → 1 opção: não mostra picker, POST sem `RoleId`; ≥2: mostra picker obrigatório, POST com `RoleId` → backend `ResolveRoleForUserType` valida/atribui.

**Forçar 2FA num tipo:** diretor liga o flag na role default correspondente (agora possível, pois a role tem `InstitutionId` = a instituição dele).

---

## Perguntas em aberto (confirmar antes/junto da implementação)
1. **Nome da coluna**: renomear `owner_id` → `institution_id` (mais claro, custa DDL/rename na migração) ou manter `owner_id` só trocando a nullability? Recomendo renomear.
2. **`DeleteRole` futuro**: confirmar a regra da invariante D3 (barrar excluir o último do `BaseType` vs. default indeletável) para já deixar documentado.
3. **Propagação de mudanças nos templates**: após a mudança, alterar `EstudDefaultRoles` (ex.: nova permissão no Diretor) **não** afeta instituições já criadas (cópias independentes) — comportamento desejado, mas confirmar que não há expectativa de "sincronizar".
