# Plano de testes de integração — Logins via Google

Cobre os **dois** fluxos de login com Google que existem hoje:

1. **Google One Tap** — `POST /identity/social-login/google-one-tap` (API JSON, valida o `id_token` via `IGoogleService`).
2. **Google OAuth redirect ("login normal")** — `GET /identity/social-login/challenge/google` → redirect pro Google → callback em `/identity/social-login/callback/google` (middleware OAuth do ASP.NET, handler `SocialLoginScheme.HandleTicketReceived`).

O projeto **Mocks** faz o papel de "Google" nos testes (já roda em Kestrel na `localhost:5678` via `MocksFactory`).

---

## Estado atual / gaps de infraestrutura

| Item | Situação | Ação |
|---|---|---|
| One Tap via `FakeGoogleService` | ✅ funciona — `Tokens` (dict estático) mapeia `credential → GoogleIdTokenPayload` | Precisa de helper de teste pra semear tokens e de `Reset()` entre testes |
| Mock Google `authorize`/`token`/`userinfo` | ❌ stubs vazios (`return Ok()` / `Redirect("")`) | **Implementar** pra simular o Google no fluxo redirect |
| Rotas do mock vs settings | ❌ mismatch: mock usa `/social-login/google/*`; `appsettings.Testing.json` aponta pra `/social/google/*` | Alinhar rotas |
| Scheme http/https | ❌ `MocksFactory.Url = http://…:5678`, mas settings usam `https://…:5678` | Alinhar |
| `FakeGoogleService.Reset()` | ❌ nunca chamado no `IntegrationTestBase` | Chamar no setup/teardown (evitar contaminação entre testes) |
| Helpers no `TestsHttpClient` | ❌ não existem pra esses fluxos | Criar `GoogleOneTapLogin(...)`, `SocialLoginChallenge(...)` e seeding do mock/fake |

### Prerequisito A — One Tap (rápido)
- Helper `_back.SeedGoogleToken(credential, email, emailVerified, sub, name)` que escreve em `FakeGoogleService.Tokens`.
- `TestsHttpClient.GoogleOneTapLogin(credential)` retornando `OneOf<GoogleOneTapLoginOut, ErrorOut>` (via `response.Resolve<...>()`).
- Chamar `FakeGoogleService.Reset()` no `OneTimeSetUp`/entre testes.

### Prerequisito B — Redirect flow (mais trabalhoso)
Implementar os 3 controllers do mock (o padrão já está descrito nos comentários dos mocks OIDC — estado por-`login_hint`/por-`code` pra segurança em paralelo):
- **authorize**: recebe `client_id`, `redirect_uri`, `state`, `login_hint`; guarda config do teste keyed por `login_hint`/`code`; redireciona pro `redirect_uri` com `?code=...&state=<echo>`.
- **token**: troca `code` por `access_token` (+ `id_token`) usando o estado capturado.
- **userinfo**: retorna JSON de claims (`sub`, `email`, `email_verified`, `name`, `given_name`, `family_name`) configurável por teste.
- Mecanismo de configuração por-teste (ex.: `_mocks.SetGoogleUser(loginHint, {...})`).
- Alinhar rotas + scheme.

---

## Fluxo 1 — Google One Tap (`POST /identity/social-login/google-one-tap`)

Ordenado do mais importante pro menos. Todos testáveis com o Prerequisito A.

### P0 — Caminhos principais (happy paths)
1. **Novo usuário é auto-provisionado** — token válido de email desconhecido → cria `Institution`, usuário com role Director, `UserSocialLogin`, `EmailConfirmed=true`; resposta 200 com `UserId`/`InstitutionId`/`Permissions`; cookie JWT setado.
2. **Usuário recorrente (link existente)** — segundo login com o mesmo `sub` → loga o **mesmo** usuário, **não** cria nova instituição nem novo `UserSocialLogin`.
3. **Usuário já existente por email (registrado antes por magic link)** — token com email de usuário existente → vincula conta Google, marca `EmailConfirmed=true`, loga o **mesmo** usuário, sem nova instituição.

### P1 — Erros de validação/domínio
4. **Credential vazio** → `GoogleOneTapLoginInvalidToken`.
5. **Token inválido/desconhecido** (não existe no `FakeGoogleService`) → `GoogleOneTapLoginInvalidToken`.
6. **`email_verified = false`** → `SocialLoginEmailNotVerified` (não cria usuário).
7. **Domínio exige SSO corporativo** (`EmailRequiresSsoAsync`) → `SocialLoginSsoRequired`.
8. **Google One Tap desabilitado** (`SocialLogin:Google:Enabled = false`) → `GoogleOneTapLoginDisabled`. *(Requer uma factory/config alternativa com Google desabilitado.)*

### P2 — Edge cases
9. **Email case-insensitive** — payload com email em MAIÚSCULAS e usuário existente em minúsculas → casa o mesmo usuário, sem duplicar.
10. **`name` vazio no payload** → usa o email como `name` do usuário criado.
11. **Mesmo email, `sub` diferente** — usuário criado antes com `sub=A`; novo token com mesmo email mas `sub=B` → cai no passo "usuário por email" e cria **um segundo** `UserSocialLogin` pro mesmo usuário. Documentar/decidir se é comportamento desejado (possível acúmulo de links).
12. **`Permissions` no output** batem com a role do usuário (Director recém-criado).

### P3 — Segurança
13. **Email não verificado não vincula conta existente** — token `email_verified=false` com email de usuário já existente → `SocialLoginEmailNotVerified`, **sem** criar `UserSocialLogin` nem confirmar email (impede sequestro de conta por email não verificado).
14. **Cookie JWT** setado com atributos corretos (HttpOnly/Secure conforme `AuthSettings`).
15. **Confiança de identidade é o `sub`, não o email** — login recorrente resolve pelo par `(provider, providerKey)`; um `sub` conhecido não é sobrescrito por outro token com mesmo email.
16. *(Limitação a documentar)* **Validação de audience/assinatura do `id_token`** é feita pelo `GoogleService` real (`GoogleJsonWebSignature.ValidateAsync`), **não** pelo `FakeGoogleService`. O fake ignora `expectedAudience`. Token para outro `client_id`, expirado ou mal assinado **não** é coberto por teste de integração — cobrir só em nota/teste unitário do `GoogleService` se desejado.

---

## Fluxo 2 — Google OAuth redirect (challenge → callback)

Requer o Prerequisito B (mock implementado). Ordenado por importância.

### Challenge (`GET /identity/social-login/challenge/{provider}`)
1. **P0 — provider `google`** → 302 pro authorization endpoint do mock; `Location` contém `client_id`, `redirect_uri` (callback), `response_type=code`, `scope=openid email profile`, `state`; cookie de correlação setado.
2. **P1 — `login_hint`** — com `?email=...` o `login_hint` é anexado à URL de redirect.
3. **P1 — provider inválido/desconhecido** → redirect pro frontend com `?social_login_error=SocialLoginFailed`.

### Callback (`/identity/social-login/callback/google` → `HandleTicketReceived`)
Caminhos principais (P0):
4. **Novo usuário auto-provisionado** — fluxo completo challenge→authorize→token→userinfo com email novo → cria instituição+usuário+link, cookie JWT, redirect `/home`.
5. **Usuário recorrente (link existente)** → mesmo usuário, redirect `/home`.
6. **Usuário existente por email** → vincula, `EmailConfirmed=true`, redirect `/home`.

Erros de domínio (P1) — todos redirecionam pro frontend com `?social_login_error=...`:
7. **`email_verified != true`** no userinfo → `SocialLoginEmailNotVerified`.
8. **Email ausente** nas claims → `SocialLoginFailed`.
9. **Domínio exige SSO** → `SocialLoginSsoRequired`.

### Segurança do fluxo redirect (P2)
10. **CSRF / correlation cookie ausente ou adulterado** → `OnRemoteFailure` → `SocialLoginFailed` (proteção nativa do handler OAuth).
11. **`state` adulterado / não confere** → remote failure → `SocialLoginFailed`.
12. **Erro na troca do code (token endpoint devolve erro)** → remote failure → `SocialLoginFailed`.
13. **Replay de authorization code** (code reutilizado) → mock rejeita na segunda troca → `SocialLoginFailed`.
14. **Paridade com One Tap**: `email_verified=false` bloqueia login também no redirect (mesma regra dos dois fluxos).
15. **Atributos de cookie** — correlation cookie `SameSite=Lax` + `Secure=Always`; cookie JWT final seguro.

### Edge cases (P3)
16. **Montagem do nome** — `given_name`+`family_name` presentes → `"Given Family"`; só `name` → usa `name`; nenhum → fallback pro email.
17. **Email case-insensitive** no matching de usuário existente (paridade com One Tap).

---

## Estrutura de arquivos dos testes (seguindo convenção do repo)

```
Tests/Features/Identity/GoogleOneTapLogin/GoogleOneTapLoginIntegrationTests.cs
Tests/Features/Identity/SocialLoginChallenge/SocialLoginChallengeIntegrationTests.cs   # challenge + callback redirect
```

- `partial class IntegrationTests`, agrupado em `#region` na ordem: Authentication (n/a — endpoints públicos 🔓, omitir) → Validation errors → Happy path → Security.
- Nomes: `Identity_GoogleOneTapLogin_Should_...`, `Identity_SocialLoginChallenge_Should_...`.
- Asserts de erro via `result.ShouldBeError(SomeError.I)`; happy path lê `result.Success`.
- Helpers de client retornam `OneOf<TOut, ErrorOut>` via `response.Resolve<TOut>()`.

## Cobertura resumida (prioridade global)

| # | Caso | Fluxo | Prio | Testável hoje |
|---|---|---|---|---|
| 1 | Auto-provisão de novo usuário | ambos | P0 | One Tap ✅ / redirect após Prereq B |
| 2 | Login recorrente (link existente) | ambos | P0 | idem |
| 3 | Vínculo a usuário existente por email | ambos | P0 | idem |
| 4 | `email_verified=false` bloqueia | ambos | P1/seg | One Tap ✅ |
| 5 | Domínio exige SSO | ambos | P1 | One Tap ✅ |
| 6 | Token/credential inválido | One Tap | P1 | ✅ |
| 7 | Feature desabilitada | One Tap | P1 | ✅ (config alt.) |
| 8 | CSRF/state/correlation cookie | redirect | seg | Prereq B |
| 9 | Replay de code | redirect | seg | Prereq B |
| 10 | Email não verificado não vincula conta | ambos | seg | One Tap ✅ |
| 11 | Case-insensitive email | ambos | edge | One Tap ✅ |
| 12 | Fallback de nome | ambos | edge | idem |
</content>
