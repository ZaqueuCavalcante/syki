# Admin Web (Frontend do backoffice)

Plano para a **UI do backoffice**, companheira do `Plans/Admin.md` (o host de API de admin). A
decisão é **uma base de código só** (`Web/`), com as páginas de admin **removidas do build** via
variável de ambiente. A imagem pública que responde em `estud.com.br` builda sem admin; quando
preciso, sobe-se uma imagem buildada com `ADMIN=true` (servindo `admin.estud.com.br`) junto do
container do backend de admin — usa, derruba os dois.

Base atual relevante: `Web/nuxt.config.ts`, `Web/app/pages/`, `Web/app/middleware/policy.global.ts`,
`Web/app/composables/useUserAccount.ts` + `useAuth.ts` + `usePolicy.ts`, `Dockerfile.web`.

## Por que não é o padrão do `Mocks`/host separado

No backend, tirar o admin do binário público é **fronteira de segurança** — remove superfície de
ataque, isola a chave de assinatura, resolve o modelo multi-tenant. Por isso virou host separado
(`Plans/Admin.md`).

**No frontend isso não vale.** O código do Nuxt roda no browser — é público por natureza. Remover
uma página do bundle **não é fronteira de segurança**: a fronteira é o backend de admin (host
dedicado, token `AdminBearer`). Com ou sem a UI no bundle, ninguém faz nada sem token válido contra
o host de admin.

Então a remoção por env var aqui serve a **acoplamento, tamanho de bundle e higiene de deploy** —
não a segurança. E, como o mecanismo do Nuxt é de build (Vite/Rollup), não existe o equivalente
subtrativo em runtime do `ControllerFeatureProvider`: a decisão é tomada **na hora do build**, que é
exatamente o que o fluxo de dois containers pede.

## Escopo

- Páginas de operador que consomem o host de API de admin: stats, instituições, usuários, eventos de
  domínio, comandos, lotes, e as ações de reprocessamento.
- Login próprio de admin (contra `admin/login` do host de admin).
- Remoção total dessas páginas — e do código exclusivo delas — do build público.

Fora de escopo na v1: app Nuxt separado / Nuxt Layers (avaliado e descartado abaixo), SSR da área de
admin (pode ser SPA-only), i18n, testes E2E do fluxo de admin.

## Decisão central — strip em build-time via `ignore`

O jeito mais completo é o `ignore` do Nuxt: os arquivos **nem são escaneados** no build, então nem a
rota nem o código exclusivo entram no `.output`. Mais forte que remover a rota depois (que dependeria
de tree-shaking para dropar componentes).

A decisão é lida de `process.env` **direto no config** (build-time) — **não** de `runtimeConfig`,
que é runtime e não removeria nada de um bundle já pronto:

```ts
// nuxt.config.ts
const admin = process.env.ADMIN === 'true'

export default defineNuxtConfig({
  ignore: admin ? [] : [
    'pages/admin/**',
    'components/admin/**',
    'middleware/admin.ts',
    // 'server/api/admin/**',   // só se houver BFF de admin
  ],
  // ...resto inalterado
})
```

- **Fail closed.** Qualquer valor que não seja exatamente `'true'` (inclusive ausência) → admin
  removido. O default é o build público.
- **Base do glob.** No Nuxt 4 os padrões de `ignore` resolvem a partir do `srcDir` (que é `app/`),
  então `pages/admin/**` deve casar com `app/pages/admin/**`. Confirmar com o teste de fumaça abaixo;
  se não casar, usar o caminho a partir da raiz (`app/pages/admin/**`).

Alternativa considerada: o hook `pages:extend` filtrando `page.path.startsWith('/admin')`. Remove a
rota, mas deixa o código exclusivo para o tree-shaking limpar — menos determinístico que o `ignore`.
Fica como plano B se algum arquivo precisar continuar escaneado por outra razão.

### Teste de fumaça (a prova de que funciona)

Buildar sem o var e afirmar que `admin` sumiu do output:

```bash
pnpm build                     # ADMIN ausente
grep -r "admin" .output/ | ...  # nenhuma rota /admin, nenhum chunk de página de admin
```

Vira um passo do CI do build público: se aparecer `admin` no `.output`, o build falha.

## Estrutura namespaced

Tudo de admin sob prefixos previsíveis, para que uma lista de `ignore` limpe o conjunto e o
tree-shaking dropa o que é usado **só** pelo admin:

```
Web/app/
  pages/admin/          index.vue, institutions.vue, users.vue, domain-events.vue,
                        commands.vue, command-batches.vue, login.vue
  components/admin/     tabelas, filtros de status/erro, modais de reprocessamento
  middleware/admin.ts   guarda de sessão de admin (route middleware nomeado)
  composables/          useAdminAuth.ts, useAdminApi.ts   (ver nota abaixo sobre composables)
```

Nota sobre composables: `app/composables/` é auto-import e **não** casaria com o `ignore` de
`components/admin/**`. Duas saídas:

- **Preferida:** mover os composables de admin para `app/components/admin/` não dá — auto-import de
  composables é por pasta `composables/`. Então: ou colocá-los sob `app/composables/admin/` e incluir
  `composables/admin/**` no `ignore`, ou mantê-los em `app/admin/` como um subdiretório próprio e
  referenciá-los por import explícito. A primeira é a mais alinhada com o resto.
- O `ignore` final cobre: `pages/admin/**`, `components/admin/**`, `composables/admin/**`,
  `middleware/admin.ts` (e `server/api/admin/**` se existir).

**Guardrail de graça:** se uma página não-admin importar um componente/composable de admin, o
**build público quebra** — o arquivo não existe naquele build. É desejável: avisa que vazou
acoplamento, em vez de arrastar código de admin para o bundle público em silêncio.

## Autenticação do admin no frontend

Separada da auth do app, espelhando a separação do backend (`Plans/Admin.md`):

- O app usa o cookie httpOnly `X-Estud-BearerCookie` contra o backend público
  (`redirect-if-logged.ts`, `useUserAccount.ts`). O admin usa o cookie próprio do host de admin
  (`X-Estud-AdminBearerCookie`) contra `admin/login`.
- `useAdminAuth.ts` faz login (POST `admin/login`, `credentials: 'include'`) e uma checagem leve de
  sessão (endpoint equivalente ao `/users/logged`, a definir no host de admin), no mesmo padrão dos
  composables existentes.
- **`policy.global.ts` não muda.** O `routePolicies` dele é todo sobre `UserType`/`permissions` do
  app, que o admin não tem. As rotas `/admin/*` não entram naquele mapa; a guarda de admin é o
  `middleware/admin.ts` dedicado, aplicado só nas páginas de admin (via
  `definePageMeta({ middleware: 'admin' })` ou por convenção de pasta).

`middleware/admin.ts` (só client, como os outros): sem sessão de admin válida → `navigateTo` para a
`login` de admin. Reforço, não fronteira — o backend de admin recusa qualquer request sem token.

### `backendUrl` na build de admin

O `runtimeConfig.public.backendUrl` da imagem de admin aponta para o **host de API de admin**, não
para o backend público. Como já é `runtimeConfig`, isso é configurável por env em runtime
(`NUXT_PUBLIC_BACKEND_URL`) — a mesma imagem de admin serve qualquer ambiente. Cookies entre
`admin.estud.com.br` e a URL do host de admin exigem atenção a `SameSite`/domínio do cookie (ver
*Decisões em aberto*); num setup efêmero, colocar os dois sob o mesmo domínio/porta via proxy elimina
o problema.

## Docker e o fluxo de dois containers

`Dockerfile.web` já usa `ARG → ENV → pnpm build`. Uma adição antes do `RUN pnpm build`:

```dockerfile
ARG ADMIN
ENV ADMIN=$ADMIN
```

- **Imagem pública** (`estud.com.br`): buildada **sem** `--build-arg ADMIN` → admin fora do output.
  É o build que o CI/deploy atual já produz; ganha só o teste de fumaça.
- **Imagem de admin** (`admin.estud.com.br`): buildada com `--build-arg ADMIN=true`, publicada no
  registry **uma vez**. Não rebuilda a cada uso.

Fluxo de uso efêmero:

1. `docker run` da imagem de admin (com `NUXT_PUBLIC_BACKEND_URL` apontando para o host de admin).
2. `docker run` do container do host de API de admin (`Plans/Admin.md`), com `Admin__SecurityKey` e
   `Admin__Users__*`.
3. Operar via `admin.estud.com.br`.
4. Derrubar os dois.

Os jobs Quartz que processam comandos/eventos rodam no backend **público** (que fica no ar), então o
reprocessamento disparado pelo admin é pego no polling normal — nada a subir a mais além dos dois
containers (ver a nota de reprocessamento assíncrono em `Plans/Admin.md`).

## Por que não app separado / Nuxt Layers

| Alternativa | Por que não na v1 |
|---|---|
| App Nuxt separado que `extends` o `Web` (layer) | Layer é **aditivo**, não subtrativo: ou extrai-se um layer base compartilhado (client de API, UI, auth) que os dois estendem, ou o admin herda o `Web` inteiro e filtra. Mais cerimônia (segundo `package.json`, segundo build, extração do layer) sem ganho de segurança, já que a fronteira real é o backend |
| Manter admin no bundle público, só com auth | Simplíssimo e defensável (frontend não é fronteira), mas serve o JS de admin a todo usuário de `estud.com.br` e acopla a UI de admin ao deploy público — exatamente o que o strip por env evita de graça |

O strip por env dá o resultado que você quer (admin fora do build público, imagem de admin
sob demanda) com o menor custo: uma condição no `nuxt.config.ts` e uma linha no Dockerfile.

## O que muda no `Web`

- `Web/nuxt.config.ts` — a constante `admin` + o `ignore` condicional.
- `Dockerfile.web` — `ARG ADMIN` / `ENV ADMIN`.
- **Novos**, todos namespaced: `app/pages/admin/**`, `app/components/admin/**`,
  `app/composables/admin/**`, `app/middleware/admin.ts` (e `app/server/api/admin/**` se houver BFF).
- CI do build público — passo de teste de fumaça (`admin` ausente do `.output`).

Nada fora do namespace `admin/` é tocado. `policy.global.ts`, `useUserAccount`, `useAuth` e as
páginas existentes seguem intactos.

## Testes

Frontend do projeto não tem suíte automatizada de componentes; o que dá para garantir com baixo
custo:

- **Teste de fumaça do build público** (CI) — buildar sem `ADMIN` e afirmar zero ocorrência de rota
  `/admin` e de chunks de página de admin no `.output`. É o teste que prova a decisão central e pega
  regressão de acoplamento (um import de admin a partir de página pública quebra o build aqui).
- **Build de admin sobe** — buildar com `ADMIN=true` e afirmar que as rotas `/admin/*` existem no
  manifesto. Garante que o namespacing e o `ignore` condicional não se anulam.
- **Guarda de rota** (manual/checklist na v1) — acessar `/admin/*` sem sessão de admin redireciona
  para a `login`; o backend de admin recusa requests sem token independentemente da UI.

## Decisões tomadas

| Decisão | Resolução |
|---|---|
| Base de código | **Única** (`Web/`); admin removido do build por env, não app separado |
| Mecanismo de remoção | `ignore` condicional no `nuxt.config.ts`, lido de `process.env.ADMIN` em build-time |
| Momento da decisão | Build-time (imagem pública vs imagem de admin), não runtime |
| Natureza da remoção | Higiene de deploy/bundle — **não** é fronteira de segurança; a fronteira é o backend de admin |
| Default | Fail closed: só `ADMIN=true` inclui; ausência remove |
| Estrutura | Tudo sob `admin/` (`pages`, `components`, `composables`, `middleware`, `server/api`) |
| Auth do frontend | Cookie e login próprios (`admin/login`), separados do app; `policy.global.ts` inalterado |
| `backendUrl` da imagem de admin | Aponta para o host de API de admin, via `NUXT_PUBLIC_BACKEND_URL` (runtime) |
| Docker | `ARG ADMIN`/`ENV ADMIN` antes do `pnpm build`; imagem de admin publicada uma vez, usada sob demanda |
| Verificação | Teste de fumaça no CI: `admin` ausente do `.output` do build público |

## Decisões em aberto

| Decisão | Opções | Sugestão |
|---|---|---|
| Cookie entre `admin.estud.com.br` e host de admin | subdomínios distintos (SameSite/domínio) × mesmo domínio via proxy | Mesmo domínio/proxy no setup efêmero — elimina a questão de cookie cross-site |
| SSR na área de admin | SSR × SPA-only (`ssr: false` por rota) | SPA-only para `/admin/*` — sem SEO a atender, simplifica auth e reduz superfície |
| Checagem leve de sessão de admin | endpoint dedicado no host de admin (tipo `/users/logged`) × validar no primeiro fetch | Endpoint dedicado, espelhando o padrão do app |
| `composables/admin/` vs `app/admin/` com import explícito | pasta sob `composables/` + `ignore` × subdir próprio | `composables/admin/**` no `ignore` — mantém o auto-import consistente com o resto |
| Reuso via Nuxt Layer no futuro | manter monolito do Web × extrair layer base | Monolito na v1; extrair layer só se surgir um terceiro consumidor da UI |

## Ordem de implementação

1. `nuxt.config.ts`: constante `admin` + `ignore` condicional (ainda sem nenhuma página de admin —
   inócuo). `Dockerfile.web`: `ARG ADMIN`/`ENV ADMIN`.
2. Teste de fumaça no CI do build público (com o namespace ainda vazio, passa trivialmente) — trava
   o comportamento antes de existir código de admin.
3. `middleware/admin.ts` + `composables/admin/useAdminAuth.ts` + `pages/admin/login.vue` contra o
   `admin/login` do host de admin. Primeira prova ponta a ponta da auth separada.
4. `composables/admin/useAdminApi.ts` (client apontando para o `backendUrl` de admin) + as páginas
   de leitura: `stats`, `institutions`, `users`.
5. Páginas de background: `commands`, `domain-events`, `command-batches`, com os filtros de
   `status=Error` (o caso de uso principal, `Plans/Admin.md`).
6. Componentes e ações de reprocessamento (individual + lote), consumindo os endpoints de
   `ReprocessCommand`/`ReprocessDomainEvent`.
7. Build de teste com `ADMIN=true` publicando a imagem de admin; validar o fluxo dos dois containers
   efêmeros contra um ambiente real.
8. Documentar no `CLAUDE.md` (seção Frontend): tudo de admin vive sob `app/**/admin/`, é removido do
   build público pelo `ignore` quando `ADMIN !== 'true'`, e não deve ser importado por código fora
   do namespace.
```
