# Frontend — Syki

## Tech Stack

| Camada | Tecnologia |
|---|---|
| Framework | **Nuxt 4.4.2** (Vue 3 + SSR) |
| Linguagem | **TypeScript 6.0.3** |
| UI Components | **@nuxt/ui 4.7.1** |
| Estilização | **Tailwind CSS 4.2.4** (tema violeta/zinc) |
| Utilitários Vue | **@vueuse/core** (shortcuts, breakpoints, composables compartilhados) |
| Tabelas | **@tanstack/table-core** (paginação, filtros, visibilidade de colunas) |
| Gráficos | **@unovis/vue** |
| Validação de forms | **Zod 4.4.3** |
| Datas | **date-fns 4.1.0** |
| Ícones | **Lucide** via Iconify (`i-lucide-*`) |
| Package manager | **pnpm** |

---

## Estrutura de Pastas

```
Web/
├── app/
│   ├── assets/css/       # main.css com Tailwind e tema customizado
│   ├── components/       # Componentes reutilizáveis (por feature)
│   ├── composables/      # Lógica compartilhada (Vue composition)
│   ├── layouts/          # Templates de layout
│   ├── pages/            # Rotas automáticas do Nuxt
│   ├── types/            # Definições de tipos TypeScript
│   ├── utils/            # Funções utilitárias
│   ├── app.vue           # Componente raiz
│   └── app.config.ts     # Tema (cores violet/zinc)
├── server/api/           # Mock endpoints do servidor Nuxt
├── nuxt.config.ts        # Configuração principal
└── .env                  # NUXT_PUBLIC_BACKEND_URL=http://localhost:5001
```

---

## Páginas e Rotas

| Rota | Arquivo | Descrição |
|---|---|---|
| `/` | `pages/index.vue` | Dashboard com stats, gráficos, tabela de vendas |
| `/inbox` | `pages/inbox.vue` | Inbox com lista/detalhe split view |
| `/customers` | `pages/customers.vue` | Tabela com filtros, seleção, colunas configuráveis |
| `/settings` | `pages/settings.vue` | Página hub de configurações |
| `/settings/members` | `pages/settings/members.vue` | Gestão de membros da equipe |
| `/settings/notifications` | `pages/settings/notifications.vue` | Preferências de notificação |
| `/settings/security` | `pages/settings/security.vue` | Configurações de segurança |
| `/register` | `pages/register.vue` | Cadastro por email |
| `/magic-link` | `pages/magic-link.vue` | Handler de login via magic link |
| `/landing` | `pages/landing.vue` | Página de marketing (PT-BR) |

**2 layouts:** `default.vue` (dashboard com sidebar) e `landing.vue` (marketing/auth).

---

## Componentes (por feature)

```
components/
├── NotificationsSlideover.vue    # Painel de notificações
├── TeamsMenu.vue                 # Dropdown de seleção de time
├── UserMenu.vue                  # Menu do usuário (dark mode + logout)
├── customers/
│   ├── AddModal.vue              # Modal de criação (form com Zod)
│   └── DeleteModal.vue           # Confirmação de deleção
├── home/
│   ├── HomeChart.client.vue      # Gráfico (cliente)
│   ├── HomeChart.server.vue      # Gráfico (SSR)
│   ├── HomeDateRangePicker.vue   # Seletor de período
│   ├── HomePeriodSelect.vue      # Período (diário/semanal/mensal)
│   ├── HomeSales.vue             # Tabela de vendas
│   └── HomeStats.vue             # Cards de estatísticas
├── inbox/
│   ├── InboxList.vue             # Lista com filtros + atalhos de teclado
│   └── InboxMail.vue             # Detalhe do email
└── settings/
    └── MembersList.vue           # Lista de membros
```

---

## Composables

**`useUserAccount.ts`** — busca e compartilha dados da conta autenticada (`GET /users/account`), usando `createSharedComposable` do VueUse (estado único global).

**`useDashboard.ts`** — gerencia estado da UI do dashboard (slideover de notificações) e define atalhos de teclado globais:
- `g+h` → Home, `g+i` → Inbox, `g+c` → Customers, `g+s` → Settings, `n` → Toggle notificações
- Setas ↑↓ navegam o inbox

Não usa Pinia/Vuex — estado compartilhado via composables e `createSharedComposable`.

---

## Integração com o Backend

**URL base:** variável de ambiente `NUXT_PUBLIC_BACKEND_URL` (default: `http://localhost:5001`).

**Chamadas diretas ao backend:**
- `POST /users/register` — cadastro por email
- `POST /identity/magic-link-login` — autenticação via token
- `GET /users/account` — dados do usuário logado

**Server routes do Nuxt** (`/server/api/`): ainda retornam dados mockados (clientes, emails, membros, notificações) — integração backend ainda em andamento.

**Cliente HTTP:** `$fetch` e `useFetch` nativos do Nuxt, com `credentials: 'include'` para enviar cookies de sessão.

---

## Autenticação

Fluxo **Magic Link** (sem senha):
1. Usuário digita email em `/register`
2. Backend envia email com link contendo `?token=...`
3. `/magic-link` captura o token, envia para `/identity/magic-link-login`
4. Backend seta cookie de sessão
5. Redirect para o dashboard

Autenticação é cookie-based; sem middleware de guarda de rotas visível no frontend — controle feito no backend.

---

## Padrões e Convenções

- **Render functions com `h()`:** usadas em células de tabela complexas
- **Zod:** validação de todos os formulários antes de submeter
- **Toast:** feedback de todas as ações via `useToast()`
- **TanStack Table:** filtragem, paginação e visibilidade de colunas nas tabelas
- **Dark mode:** suportado via Nuxt UI + Tailwind
- **Responsivo:** mobile-first, sidebar colapsável, slideover em mobile
- **Nomeação:** componentes PascalCase, composables com prefixo `use`, páginas lowercase-hyphen

---

## Estado Atual

Integração backend/frontend parcial — auth (register + magic link) já conectada; dados do dashboard ainda mockados no `server/api/`.
