# Infra.md

Documentação da infraestrutura e rede do **Syki**.

## Visão geral

Todo o tráfego público entra pela **Cloudflare**, que faz proxy para o **Railway**.

Dentro do Railway, um serviço **Caddy** atua como reverse proxy / API gateway único,
roteando para os serviços **Web** (Nuxt) e **Back** (ASP.NET Core) pela rede privada
do Railway. O banco é **PostgreSQL**.

```
┌──────────┐   HTTPS    ┌────────────┐   HTTPS   ┌──────────────────────── Railway (rede privada) ────────────────────────┐
│  Cliente │ ─────────► │ Cloudflare │ ────────► │                                                                        │
│ (browser)│            │  (proxy +  │           │   ┌─────────┐                                                          │
└──────────┘            │   WAF/CDN) │           │   │  Caddy  │  :$PORT (80)  ── reverse proxy / gateway público         │
                        └────────────┘           │   └────┬────┘                                                          │
                                                 │        │                                                               │
                                                 │        ├── /api/_*  ─────────► web.railway.internal:80   (Nuxt SSR)    │
                                                 │        ├── /api/*    (strip /api) ─► back.railway.internal:5000 (API)  │
                                                 │        └── /*        ─────────► web.railway.internal:80   (Nuxt SSR)   │
                                                 │                                        │                               │
                                                 │                                        ▼                               │
                                                 │                                 ┌─────────────┐                        │
                                                 │                                 │ PostgreSQL  │ ◄── Back (EF + Dapper) │
                                                 │                                 └─────────────┘                        │
                                                 └────────────────────────────────────────────────────────────────────────┘
```

## Camadas

### 1. Cloudflare (borda)

- Fica na frente de tudo: DNS, terminação TLS pública, CDN, WAF/proxy.
- Encaminha as requisições para o domínio público do serviço **Caddy** no Railway.
- Como há proxy na frente, o Back confia nos cabeçalhos `X-Forwarded-*`
  (ver [Forwarded Headers](#forwarded-headers--https)).

### 2. Caddy — reverse proxy / gateway (Railway)

Único ponto de entrada público dentro do Railway. Imagem `caddy:2-alpine`.

- **Dockerfile**: `Dockerfile.caddy`
- **Config**: `Caddyfile`
- Escuta na porta `$PORT` (Railway injeta), default `80`.

Regras de roteamento (`Caddyfile`):

| Rota | Destino | Observação |
|---|---|---|
| `/api/_*` | `web.railway.internal:80` | rotas internas do Nuxt sob `/api/_*` (ex: `_nuxt`, payloads) ficam no Web |
| `/api/*` | `back.railway.internal:5000` | API do backend; o prefixo `/api` **não** é removido (o backend usa `UsePathBase("/api")`) |
| `/*` (resto) | `web.railway.internal:80` | app Nuxt (SSR) |

> A ordem importa: `/api/_*` é avaliado antes de `/api/*` para não mandar assets do
> Nuxt para o backend.
>
> O Caddy **não faz `strip_prefix /api`**: o prefixo precisa chegar ao backend para que
> o `UsePathBase("/api")` (em `Program.cs` via `UseApiPrefix`) o reconheça e gere URLs
> absolutas corretas — em especial o `redirect_uri` do OAuth do Google. Se o Caddy
> removesse o `/api` e o backend também esperasse `/api`, haveria dupla remoção e o
> `redirect_uri` sairia sem o prefixo (caindo no Web no callback).

O bloco global do `Caddyfile` define `trusted_proxies static 0.0.0.0/0 ::/0` para que o
Caddy **preserve** os `X-Forwarded-*` vindos do edge do Railway (em especial
`X-Forwarded-Proto: https`) em vez de sobrescrevê-los com o `http` do hop interno. Sem
isso o backend acharia que a requisição é `http` e geraria `redirect_uri` com `http://`.
Confiar em toda a faixa é seguro aqui porque o container só é alcançável via edge do
Railway (a faixa CGNAT `100.64.0.0/10` do Railway não é coberta pelo `private_ranges`
padrão do Caddy).

### 3. Web — frontend Nuxt (Railway)

- **Dockerfile**: `Dockerfile.web` (build multi-stage Node 26 + pnpm, output standalone)
- Roda `node .output/server/index.mjs` com `HOST=0.0.0.0`.
- Endereço interno: `web.railway.internal`, alvo do Caddy na porta **80**.
- Build args:
  - `NUXT_PUBLIC_BACKEND_URL` — URL pública do backend usada pelo client.
  - `NUXT_PUBLIC_COMMIT_SHA` — SHA do commit para rastreio de versão.
- Em dev, `NUXT_PUBLIC_BACKEND_URL=http://localhost:5000` (`Web/.env`).

### 4. Back — API ASP.NET Core (Railway)

- **Dockerfile**: `Dockerfile.back` (SDK .NET 10 → runtime aspnet 10).
- Escuta na porta **5000** (`EXPOSE 5000`).
- Endereço interno: `back.railway.internal:5000`, recebe do Caddy **com** o prefixo `/api`.
- `UseApiPrefix` → `app.UsePathBase("/api")`: o middleware só ativa o `PathBase` quando o
  path realmente começa com `/api` (produção, via Caddy); requisições sem o prefixo
  passam intactas (dev/local/testes contra `localhost:5000` continuam funcionando).
- Pipeline relevante (`Back/Program.cs`):
  `UseForwardedHeaders` → `UseHttpsConfigs` (HSTS + HTTPS redirect) → CORS →
  `UseApiPrefix` → routing → auth → rate limiting → authorization.

### 5. PostgreSQL

- Banco relacional acessado pelo Back via **EF Core** + **Dapper**.
- Schema `syki`, convenção `snake_case`.
- Também guarda as **chaves de Data Protection** (ver abaixo).

## Forwarded Headers / HTTPS

Como há **dois proxies na frente** (Cloudflare + Caddy), o Back precisa confiar nos
cabeçalhos encaminhados para reconstruir scheme/host/IP reais
(`Back/Configs/HttpConfigs.cs`):

```csharp
ForwardedHeaders = XForwardedProto | XForwardedHost | XForwardedFor
ForwardLimit = null
RequireHeaderSymmetry = false
KnownProxies.Clear();      // limpa lista de proxies confiáveis...
KnownIPNetworks.Clear();   // ...para aceitar a cadeia Cloudflare→Caddy no Railway
```

> `KnownProxies`/`KnownIPNetworks` são limpos porque os IPs dos proxies (Cloudflare e
> rede interna do Railway) são dinâmicos/desconhecidos. Sem isso o
> `UseForwardedHeaders` ignoraria os headers e o HTTPS redirect/cookies `Secure`
> quebrariam.

Depois disso: `UseHsts()` + `UseHttpsRedirection()`.

## CORS

`Back/Configs/CorsConfigs.cs` — política default:

- `AllowAnyMethod`, `AllowAnyHeader`, `AllowCredentials`.
- `WithOrigins("http://localhost:3000")` (origem de desenvolvimento).

> Em produção, como tudo passa pelo mesmo host via Caddy (`/api/*` e `/*` no mesmo
> domínio), as chamadas são **same-origin** e não dependem de CORS.

## Cookies & autenticação

- **JWT** entregue em cookie HttpOnly (`X-Estud-BearerCookie`), lido em
  `Back/Auth/Schemes/JwtBearerScheme.cs`.
- Opções do cookie (`Back/Extensions/HttpExtensions.cs`): `Path = "/"`,
  `HttpOnly = true`, `Secure = settings.CookieSecure`,
  `SameSite = settings.CookieSameSite`.
- Defaults do `AuthSettings` (`Back/Settings/AuthSettings.cs`):
  `CookieSecure = true`, `CookieSameSite = Lax`.
- **Social login (Google)** — `Back/Auth/Schemes/SocialLoginScheme.cs`:
  - `CorrelationCookie.SameSite = Lax`, `SecurePolicy = Always`.
  - Cookie temporário `X-Estud-SocialTempCookie` (`SameSite = Lax`, `Secure = Always`).
  - Callback: `/identity/social-login/callback/google`.
  - Redireciona para o frontend (`FrontendSettings.Url` / `BuildUrl`).

## Data Protection (multi-instância)

`Back/Configs/DataProtectionConfigs.cs`:

```csharp
AddDataProtection()
    .SetApplicationName("Syki")
    .PersistKeysToDbContext<SykiDbContext>();
```

> Chaves persistidas no banco (não no disco). Necessário porque o Back pode rodar em
> múltiplas instâncias/redeploys no Railway — todas precisam compartilhar a mesma
> chave para validar cookies/tokens.

## CI/CD

- **GitHub Actions** — `.github/workflows/ci.cd.yml` (push em `master`):
  build + testes unitários e de integração (Postgres como service container),
  cobertura publicada em GitHub Pages (`gh-pages` →
  https://zaqueucavalcante.github.io/syki).
- **PRs** — `.github/workflows/pr.tests.yml`.
- Deploy dos serviços (Caddy / Web / Back) feito no **Railway** a partir dos
  respectivos Dockerfiles.

## Resumo de portas / endereços

| Serviço | Imagem / runtime | Porta interna | Endereço interno (Railway) | Exposição |
|---|---|---|---|---|
| Cloudflare | — | — | — | público (DNS/TLS/CDN) |
| Caddy | `caddy:2-alpine` | `$PORT` (80) | — | público (alvo da Cloudflare) |
| Web | Node 26 / Nuxt | 80 | `web.railway.internal` | só via Caddy |
| Back | .NET 10 (aspnet) | 5000 | `back.railway.internal` | só via Caddy (`/api`) |
| PostgreSQL | postgres | 5432 | rede privada | só via Back |
