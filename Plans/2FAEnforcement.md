# Plano — 2FA Enforcement (obrigatoriedade de 2FA por instituição)

Hoje o 2FA é **opt-in**: qualquer usuário pode ativar em `identity/2fa-setup`, mas nada obriga.
O 2FA Enforcement permite que a instituição **force** determinados usuários a logar com 2FA.
Se um usuário obrigado ainda não configurou o 2FA, ao logar com email+senha ele é levado a uma tela de setup — usando uma **credencial de setup dedicada** (um _auth scheme_ próprio) que só autentica nos endpoints de setup do 2FA.
Terminado o setup, ele recebe o **JWT real** (com todas as permissões) e entra logado de verdade.

**Escopo desta feature:** apenas login **email+senha**. Login SSO e Google Social Login ficam de fora por enquanto.

---

## Estado atual do sistema (base para o plano)

### Backend
- **Login email+senha**: `Back/Features/Identity/EmailPasswordLogin/EmailPasswordLoginService.cs`. Valida credenciais; se `user.TwoFactorEnabled`, seta o cookie `TwoFactorUserIdScheme` e retorna o erro `LoginRequiresTwoFactor`; senão chama `SignInService.SignIn(email)` → JWT completo.
- **Geração de JWT**: `Back/Features/Identity/SignIn/SignInService.cs` (namespace `Estud.Back.Features.Cross.SignIn`). Monta claims (`UserId`, `UserPermissions`, `Jti`, `UserType`, `UserInstitutionId`), assina e grava o cookie `X-Estud-BearerCookie` via `AppendJWTCookie`. Permissões vêm de `ctx.GetUserRole(userId, institutionId)`.
- **Claims**: `Back/Auth/Claims/EstudClaims.cs` (`sub`, `type`, `role`, `prms`, `inst`, `jti`).
- **Setup 2FA**: `Back/Features/Identity/SetupTwoFactor/SetupTwoFactorService.cs` — valida o TOTP e chama `SetTwoFactorEnabledAsync(user, true)`. Retorna `EstudSuccess`. Policy `SetupTwoFactor` = **apenas autenticado** (`AddEstudPolicy(SetupTwoFactor)` sem permissão).
- **Chave/QR 2FA**: `Back/Features/Identity/GetTwoFactorKey/` (`identity/2fa-key`). Policy = apenas autenticado.
- **2FA login**: `Back/Features/Identity/TwoFactorLogin/` (`identity/2fa-login`) — usado no fluxo de quem já tem 2FA ativo. Após email+senha, o `EmailPasswordLoginService` faz `SignInTwoFactorUserIdSchemeAsync(user.Id)` (cookie temp do Identity `TwoFactorUserIdScheme`); o `TwoFactorLoginService` lê via `AuthenticateAsync(TwoFactorUserIdScheme)`, valida o TOTP, dá `SignOutAsync` no scheme e emite o JWT. **É exatamente o padrão de credencial intermediária que vamos replicar.**
- **Schemes intermediários existentes**: `Back/Auth/Schemes/SsoTempScheme.cs` e `SocialTempScheme.cs` — cookies de vida curta (5 min, `AddCookie`) usados como estado intermediário em fluxos multi-step (`SignInScheme` dos handlers OIDC/social). Registrados em `Back/Configs/AuthenticationConfigs.cs`. Helper de sign-in em `Back/Extensions/HttpExtensions.cs` (`SignInTwoFactorUserIdSchemeAsync`).
- **Policies**: `Back/Auth/Policies/Policies.cs` — três overloads de `AddEstudPolicy` (só autenticado / autenticado + permissões / autenticado + userType + permissões). **Todas fixam `AddAuthenticationSchemes(JwtBearerScheme.Name)`** — os schemes temp não passam por `[Authorize]`, são lidos manualmente nos services. Identity policies em `Policies.Identity.cs`.
- **Autorização por permissão**: `RequireAssertion(x => x.User.Permissions.Any(...))`. Permissões lidas do claim `prms` (`Back/Extensions/UserExtensions.cs`).
- **Contexto por request**: `EnrichBackDbContextMiddleware` popula `ctx.RequestUser` (Id, InstitutionId, RoleId, Permissions) a partir das claims.
- **Role**: `Back/Domain/Identity/EstudRole.cs` (`OwnerId` = instituição, `BaseType`, `Permissions`). DbConfig em `Back/Database/Identity/EstudRoleDbConfig.cs`.
- **Config por instituição (padrão de referência)**: features `Institutions/GetInstitutionConfig` + `SetupInstitutionConfig` (GET/POST `institutions/config`) e o par SSO `Identity/GetSsoConfiguration` + `UpdateSsoConfiguration` — modelo a seguir para a tela do Manager.

### Frontend (já há scaffolding parcial!)
- `Web/app/pages/login/index.vue` — **já trata** `errorData?.code === 'LoginTwoFactorEnforced'` e redireciona para `/login/setup-2fa` (linhas ~72-75). ⚠️ **Bug existente**: usa `router.push(...)` mas `router` nunca é declarado no `<script setup>` (falta `const router = useRouter()`). Corrigir junto.
- `Web/app/pages/login/setup-2fa.vue` — **já existe**, com QR + PinInput + textos "Sua organização exige autenticação de dois fatores". ⚠️ Hoje, ao concluir o setup, ela redireciona para `/login` ("Faça login novamente"). O requisito é **entrar logado direto** — precisa mudar.
- `Web/app/composables/useAuth.ts` — já tem `getTwoFactorKey`, `setupTwoFactor`, `twoFactorLogin`, `fetchUser`.
- **Seção Segurança do Manager**: `Web/app/pages/security.vue` (nav com abas "Perfis" e "SSO"), `security/index.vue` (perfis), `security/sso.vue` (SSO). Padrão de tela a seguir.
- **Policies frontend**: `Web/app/policies/store.ts` + `types.ts`, guard de rota em `middleware/policy.global.ts`.

---

## Decisão de design 1 — Onde armazenar "quais usuários são obrigados"

**Recomendado: flag por Role (`TwoFactorRequired` em `EstudRole`).**

Justificativa: toda associação usuário↔instituição já passa por uma Role, o login já resolve a role do usuário (`GetUserRole`), e o Manager já gerencia roles na seção Segurança. Isso dá granularidade real ("Coordenadores são obrigados, Alunos não") sem inventar um novo eixo de configuração. "Determinados usuários" = "usuários de determinadas roles".

Alternativas consideradas (não recomendadas agora):
- **Por UserType** (ex.: todos os Managers): menos granular; um UserType pode ter várias roles.
- **Por usuário individual**: controle fino demais, custoso de gerenciar na UI e no cadastro.

> Ver "Perguntas em aberto" no fim — esta é a principal decisão de produto a confirmar.

## Decisão de design 2 — Auth scheme dedicado para o setup (cookie temp)

Em vez de um JWT Bearer "de permissões mínimas" restringido por um middleware de whitelist, usar um **auth scheme dedicado** — a **quarta instância** do padrão que o sistema já usa 3x (`SsoTemp`, `SocialTemp`, `TwoFactorUserIdScheme`): uma credencial intermediária, de vida curta e propósito único, que só destrava o próximo passo.

**Novo scheme `TwoFactorSetupScheme`** (cookie temp, modelado em `SsoTempScheme`/`SocialTempScheme`). Após email+senha válidos de um usuário obrigado sem 2FA, o login faz `SignInAsync(TwoFactorSetupScheme)` com um principal que carrega só `sub` (UserId) e `inst` (InstitutionId). Esse cookie **não é um Bearer JWT** e **não autentica em nenhuma policy `JwtBearerScheme`**.

**Restrição por construção, não por enforcement.** O acesso é definido por *quais endpoints aceitam o scheme na policy*, não por uma whitelist de paths mantida à mão:
- As policies de `GetTwoFactorKey` e `SetupTwoFactor` passam a aceitar **os dois schemes**: `AddAuthenticationSchemes(JwtBearerScheme.Name, TwoFactorSetupScheme.Name)`. Assim atendem tanto o fluxo enforced (cookie de setup) quanto o voluntário (Bearer full de quem já está logado).
- Todo o resto das policies continua só com `JwtBearerScheme.Name` → o cookie de setup **não autentica em lugar nenhum além desses dois endpoints**. O default seguro (zero acesso) é estrutural, não depende de middleware.

Consequências (vs. a abordagem anterior de claim `scp` + middleware):
- **Sem** claim de escopo no JWT, **sem** `TwoFactorSetupScopeMiddleware` de whitelist (o antigo B6 deixa de existir).
- Contradiz a palavra literal "JWT" do requisito original, mas segue o precedente do próprio 2FA login (que usa cookie, não JWT) — é a forma mais idiomática aqui.
- `EnrichBackDbContextMiddleware` popula `ctx.RequestUser.Id`/`InstitutionId` normalmente, desde que o principal do scheme carregue o claim `sub` (o `IsAuthenticated` do sistema exige `sub`).
- `logout` não precisa mais ser "liberado": o cookie de setup expira sozinho (janela curta) se o usuário abandonar.

### Alternativas descartadas

**A) Claim de escopo (`scp=2fa-setup`) num Bearer JWT + middleware de whitelist de paths.**
Era a proposta inicial. Descartada porque: o token de setup seria um Bearer válido (passa em `IsAuthenticated`), e a única coisa impedindo seu uso amplo seria um `TwoFactorSetupScopeMiddleware` com whitelist de rotas por string — deny **por enforcement**, frágil (matching por `EndsWith`, quebra em rename/casing, precisa ser lembrado a cada endpoint novo). O scheme dedicado dá deny **por construção** (o cookie só é aceito onde a policy o lista) e elimina o middleware.

**B) Reusar o `IdentityConstants.TwoFactorUserIdScheme` (o cookie do 2FA login) também para o enforcement, fazendo as policies de `2fa-key`/`2fa-setup` aceitarem os dois schemes.**
Descartada — é um **downgrade de segurança**. Motivos, todos verificados no código:
- **Escalonamento de 1º → 2º fator (bloqueante).** Esse cookie representa "provou só a senha, aguardando o TOTP" e é emitido no 2fa-login normal, para usuários que **já têm 2FA**. Se `2fa-key`/`2fa-setup` aceitarem esse scheme, um usuário nesse estado (ou um atacante com só a senha) chamaria `GetTwoFactorKey` e, para um usuário com 2FA ativo, `GetUserTwoFactorKeyAsync` (`EstudDbContext.Identity.cs:148`) retorna a **chave TOTP existente** + QR — vazando o segredo do segundo fator para quem tem apenas o primeiro, derrotando o 2FA. Um scheme dedicado só é emitido para usuários **sem** 2FA, então não há segredo a vazar.
- **Não é drop-in.** `SignInTwoFactorUserIdSchemeAsync` (`HttpExtensions.cs:79`) grava o userId como `ClaimTypes.Name`, não como `sub`; sem `sub`, o `EnrichBackDbContextMiddleware` não popula `ctx.RequestUser.Id` e GetKey/Setup quebram. Corrigir exigiria mexer em código **compartilhado** com o fluxo 2fa-login.
- **Não distingue os fluxos.** Mesmo scheme = os endpoints não separam "2fa-login (tem 2FA)" de "setup enforced (não tem 2FA)"; fechar o buraco exigiria checagens defensivas de `user.TwoFactorEnabled` espalhadas — de novo, deny por enforcement.
- **Blast radius.** Sobrecarrega a semântica de um scheme built-in do ASP.NET Identity com um segundo propósito.
- Upside real (economizar ~12 linhas de registro do scheme) não compensa nenhum dos itens acima.

---

## Backend

### B1. Domínio: flag na Role
- `Back/Domain/Identity/EstudRole.cs`: adicionar `public bool TwoFactorRequired { get; set; }`. Default `false`. Incluir no ctor/`Update` conforme necessário (ou setar via método dedicado — ver B5).
- `Back/Database/Identity/EstudRoleDbConfig.cs`: mapear a coluna (`two_factor_required`, `bool`, default `false`).
- **Migração** EF (`dotnet ef migrations add AddTwoFactorRequiredToRoles`). Coluna `not null default false` já cobre roles existentes.

### B2. Scheme dedicado + policies que o aceitam
- Novo `Back/Auth/Schemes/TwoFactorSetupScheme.cs`, modelado em `SsoTempScheme`/`SocialTempScheme`:
  ```csharp
  public static class TwoFactorSetupScheme
  {
      public const string Name = "TwoFactorSetup";
      public const string Cookie = "X-Estud-TwoFactorSetupCookie";

      public static AuthenticationBuilder AddTwoFactorSetupScheme(this AuthenticationBuilder builder)
      {
          return builder.AddCookie(Name, options =>
          {
              options.Cookie.Name = Cookie;
              options.Cookie.SameSite = SameSiteMode.Lax;
              options.ExpireTimeSpan = TimeSpan.FromMinutes(10);   // janela curta só p/ o setup
              options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
          });
      }
  }
  ```
- Registrar em `Back/Configs/AuthenticationConfigs.cs` (`.AddTwoFactorSetupScheme()`).
- Helper de sign-in em `Back/Extensions/HttpExtensions.cs` (irmão de `SignInTwoFactorUserIdSchemeAsync`), com o principal carregando `sub` + `inst`:
  ```csharp
  public async Task SignInTwoFactorSetupSchemeAsync(int userId, int institutionId)
  {
      var claims = new List<Claim>
      {
          new(EstudClaims.UserId, userId.ToString()),
          new(EstudClaims.UserInstitutionId, institutionId.ToString()),
      };
      var identity = new ClaimsIdentity(claims, TwoFactorSetupScheme.Name);
      await context!.SignInAsync(TwoFactorSetupScheme.Name, new ClaimsPrincipal(identity));
  }
  ```
  > O claim `sub` é obrigatório: o `IsAuthenticated` do sistema e o `EnrichBackDbContextMiddleware` dependem dele para popular `ctx.RequestUser.Id`.
- **Policies `GetTwoFactorKey` e `SetupTwoFactor` passam a aceitar os dois schemes.** Como o overload `AddEstudPolicy(name)` fixa só `JwtBearerScheme.Name`, adicionar um overload que recebe schemes:
  ```csharp
  public static AuthorizationBuilder AddEstudPolicy(this AuthorizationBuilder builder, string name, string[] schemes)
      => builder.AddPolicy(name, p => p.RequireAuthenticatedUser().AddAuthenticationSchemes(schemes));
  ```
  e em `Policies.Identity.cs`:
  ```csharp
  .AddEstudPolicy(SetupTwoFactor,  [JwtBearerScheme.Name, TwoFactorSetupScheme.Name])
  .AddEstudPolicy(GetTwoFactorKey, [JwtBearerScheme.Name, TwoFactorSetupScheme.Name]);
  ```
  (Cuidado com ambiguidade de overload: `string[]` explícito evita colidir com o `params EstudPermission[]`.) Todo o resto continua só com `JwtBearerScheme` → o cookie de setup não autentica em nenhum outro endpoint.

### B3. ~~SignInService: token de setup~~ (não é mais necessário)
O login não emite JWT no fluxo enforced — apenas faz `SignInAsync` no scheme de setup (ver B4). `SignInService.SignIn(email)` (JWT full) continua inalterado e é reusado só no fim do setup (B5).

### B4. EmailPasswordLoginService: decidir enforcement
Após validar a senha e **antes** do bloco `if (user.TwoFactorEnabled)`:
```csharp
if (!user.TwoFactorEnabled)
{
    var role = await ctx.GetUserRole(user.Id, user.InstitutionId);
    if (role.TwoFactorRequired)
    {
        await httpCtx.HttpContext.SignInTwoFactorSetupSchemeAsync(user.Id, user.InstitutionId);
        return new LoginTwoFactorEnforced();
    }
}
```
- Espelha exatamente o `SignInTwoFactorUserIdSchemeAsync` já usado logo abaixo para o `LoginRequiresTwoFactor` — usa o `IHttpContextAccessor` já injetado, **sem** precisar do `SignInService`.
- Ordem final do fluxo: credenciais → (2FA já ativo? → `LoginRequiresTwoFactor` como hoje) → (enforcement sem 2FA? → `SignInAsync(TwoFactorSetupScheme)` + `LoginTwoFactorEnforced`) → senão login normal.
- Novo erro `LoginTwoFactorEnforced` em `Back/Features/Identity/EmailPasswordLogin/EmailPasswordLoginErrors.cs` (código = `nameof`, mensagem PT). Adicionar ao `ErrorExamplesProvider` do `EmailPasswordLoginController`. **O código `LoginTwoFactorEnforced` já é esperado pelo frontend.**

### B5. SetupTwoFactor: emitir JWT real ao final do setup enforced
Requisito: "após o setup ele já vai entrar realmente logado, com um JWT contendo todas as suas permissões reais."
- Em `SetupTwoFactorService.Setup`, após `SetTwoFactorEnabledAsync(webUser, true)`:
  - Se a request autenticou pelo **scheme de setup**, chamar `signInService.SignIn(webUser.Email)` para gravar o **JWT full** e `SignOutAsync(TwoFactorSetupScheme.Name)` para limpar o cookie temp (espelha o `SignOutAsync(TwoFactorUserIdScheme)` do `TwoFactorLoginService`).
  - Se for setup voluntário (Bearer full de quem já está logado), **não** reemitir.
  - **Como detectar**: pelo scheme que autenticou, via `httpCtx.HttpContext.User.Identities.Any(i => i.AuthenticationType == TwoFactorSetupScheme.Name)`. (Com policy multi-scheme, o `HttpContext.User` combina as identities dos schemes presentes; checar por `AuthenticationType` é robusto.)
- Injetar `SignInService` e `IHttpContextAccessor` no `SetupTwoFactorService`.
- Manter retorno `EstudSuccess` (o efeito colateral é o cookie Bearer full gravado + cookie de setup limpo).

> Segurança: emitir o JWT full logo após o setup é aceitável — o usuário **acabou de provar** posse do segundo fator ao inserir um TOTP válido no próprio setup. Não é preciso um `2fa-login` adicional.

### B6. ~~Middleware de guarda~~ (eliminado)
Não existe mais. A restrição do credencial de setup é **estrutural** (B2): o cookie de setup só é aceito nas policies de `GetTwoFactorKey`/`SetupTwoFactor`, que o listam explicitamente; qualquer outro endpoint exige `JwtBearerScheme` e rejeita o cookie de setup automaticamente (401).

### B7. Feature de configuração do Manager (par Get/Set enforcement)
Modelo: par SSO (`GetSsoConfiguration` / `UpdateSsoConfiguration`). Nova permissão para não sobrecarregar `ManageRoles` (opcional — ver perguntas em aberto).

- **`Back/Features/Identity/GetTwoFactorEnforcement/`** (GET `identity/2fa-enforcement`):
  - Retorna a lista de roles da instituição com `{ roleId, name, baseType, twoFactorRequired }`.
  - Policy `Manager` + permissão de gestão (ex.: reusar `ManageRoles`, ou nova `ManageSecurity`).
- **`Back/Features/Identity/SetTwoFactorEnforcement/`** (POST/PUT `identity/2fa-enforcement`):
  - Input: `{ roleId, required }` (ou lista de pares para salvar em lote).
  - Valida que a role pertence à instituição (`OwnerId == ctx.RequestUser.InstitutionId`), atualiza `TwoFactorRequired`, `SaveChangesAsync`.
  - Seguir o padrão POST de 4 arquivos do `CLAUDE.md` (Controller/Service/In/Out + Mapper), validators nested, `OneOf<TOut, EstudError>`.
- Registrar policies em `Policies.Identity.cs` (`GetTwoFactorEnforcement`, `SetTwoFactorEnforcement`).

### B8. Testes de integração
Arquivos partial de `IntegrationTests` espelhando as pastas, com regiões `Authentication`/`Authorization`/`Validation errors`/`Happy path`:
- **EmailPasswordLogin**: usuário com role `TwoFactorRequired=true` e sem 2FA → `ShouldBeError(LoginTwoFactorEnforced.I)` e cookie do scheme de setup presente. Usuário com role obrigada mas **com** 2FA → cai no fluxo `LoginRequiresTwoFactor` normal. Usuário sem enforcement → login normal.
- **Isolamento do scheme de setup**: com o cookie de setup, chamar um endpoint protegido por Bearer (ex.: `GetUserAccount`) → **401** (o scheme não é aceito ali); chamar `2fa-key`/`2fa-setup` → OK.
- **SetupTwoFactor no fluxo enforced**: após setup autenticado pelo scheme de setup, o cookie passa a ser JWT full e o cookie de setup é limpo (checar que um endpoint protegido por permissão passa a responder 200).
- **Get/SetTwoFactorEnforcement**: 401 sem auth, 403 sem permissão/UserType, happy path atualiza a flag.
- `TestsHttpClient`: métodos retornando `OneOf<TOut, ErrorOut>` via `response.Resolve<TOut>()` para os asserts `ShouldBeError`/`.Success`.

---

## Frontend

### F1. Login (`Web/app/pages/login/index.vue`)
- **Corrigir o bug**: declarar `const router = useRouter()` (ou trocar `router.push` por `navigateTo`). Hoje `router` é usado mas não existe → o redirect quebraria.
- Manter o tratamento de `LoginTwoFactorEnforced` → `/login/setup-2fa` (já presente).

### F2. Setup enforced (`Web/app/pages/login/setup-2fa.vue`)
- Mudar o comportamento **de sucesso**: em vez de redirecionar para `/login` ("faça login novamente"), como o backend já reemitiu o **JWT full** no cookie, fazer `await fetchUser()` + `await navigateTo('/home')` — o usuário entra logado direto (requisito).
- Ajustar toast de sucesso ("2FA ativado, entrando…").
- A página já consome `getTwoFactorKey`/`setupTwoFactor` com `credentials: 'include'`, então envia o cookie do scheme de setup automaticamente. Confirmar que as policies dessas duas rotas aceitam o `TwoFactorSetupScheme` (B2).

### F3. Tela de configuração do Manager (nova aba "2FA" em Segurança)
- `Web/app/pages/security.vue`: adicionar item de nav "2FA" (`i-lucide-shield-check`, `to: '/security/2fa'`).
- Nova página `Web/app/pages/security/2fa.vue`: lista as roles da instituição (GET `identity/2fa-enforcement`) numa tabela/lista com um `USwitch` por role ("Exigir 2FA"). Ao alternar, chama `SetTwoFactorEnforcement` e dá feedback (toast). Seguir o visual de `security/sso.vue`.
- Handlers `@click`/`@change` sempre arrow function em bloco (convenção `CLAUDE.md`).

### F4. Policies frontend
- `Web/app/policies/types.ts` + `store.ts`: adicionar `AccessTwoFactorEnforcementPage` (UserType Manager + permissão correspondente), espelhando `AccessSsoPage`.
- `Web/app/middleware/policy.global.ts`: mapear `'/security/2fa': 'AccessTwoFactorEnforcementPage'`.
- Se criar nova permissão no backend (B7), refletir em `Web/app/policies` (Permissions).

### F5. Types
- Gerar/atualizar os tipos TS (`~/types`) para os DTOs novos (`GetTwoFactorEnforcementOut`, `SetTwoFactorEnforcementIn/Out`), conforme o mecanismo usado hoje (ex.: geração a partir do Swagger/Scalar).

---

## Fluxos (resumo)

**Config (Manager):** Segurança → aba 2FA → liga "Exigir 2FA" na role X → `SetTwoFactorEnforcement`.

**Login de usuário obrigado, sem 2FA:**
1. Email+senha → backend valida.
2. Role tem `TwoFactorRequired` e `!TwoFactorEnabled` → backend faz `SignInAsync(TwoFactorSetupScheme)` (cookie temp, só `sub`+`inst`) e retorna `LoginTwoFactorEnforced`.
3. Front redireciona `/login/setup-2fa`. O cookie de setup só autentica em `2fa-key`/`2fa-setup` (as únicas policies que aceitam esse scheme).
4. Usuário escaneia QR e envia TOTP → `2fa-setup` habilita 2FA, **grava o cookie Bearer com JWT full** e limpa o cookie de setup.
5. Front faz `fetchUser()` + vai para `/home`. **Logado de verdade.**

**Login de usuário obrigado, já com 2FA:** fluxo atual inalterado (`LoginRequiresTwoFactor` → `/login/2fa` → `2fa-login` → JWT full).

**Login sem enforcement:** inalterado.

---

## Perguntas em aberto (confirmar antes/junto da implementação)
1. **Granularidade do enforcement**: por Role (recomendado) vs. por UserType vs. por usuário? Muda schema, endpoints e UI.
2. **Permissão de configuração**: reusar `ManageRoles` para a tela de 2FA, ou criar `ManageSecurity`/`ManageTwoFactor` dedicada?
3. **Expiração do cookie de setup**: `ExpireTimeSpan` proposto de 10 min (os temp SSO/social usam 5 min). Ajustar?
4. **Usuário já logado (token full) cuja role vira obrigada depois**: enforcement só vale no próximo login (proposta atual), ou revoga sessão ativa? Proposta: só no próximo login — simples e suficiente.
