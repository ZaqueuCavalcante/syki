# Plano — Reorganização da sidebar do Manager

A sidebar de um Manager com o role **Diretor** (todas as permissões) exibe 18 itens numa lista plana, sem hierarquia nem agrupamento. Este plano agrupa os itens por domínio, renomeia o que está ambíguo e, opcionalmente, consolida páginas irmãs em abas.

## Estado atual do sistema (base para o plano)

- Sidebar em `Web/app/layouts/default.vue`: `allLinks` (linhas 12-34) é um array plano de `{ label, icon, to, policy }`, filtrado por `can(policy)` no computed `links` (linhas 54-75).
- Command palette (`groups`, linhas 77-91) reaproveita `allLinks` num único grupo `"Go to"`.
- Policies de página em `Web/app/policies/store.ts`; guarda de rota em `Web/app/middleware/policy.global.ts` (mapa `routePolicies`).
- Role `Diretor` em `Back/Auth/Roles/EstudDefaultRoles.cs:8` — 19 permissões.
- Itens visíveis hoje pro Manager (18):
  `Home · Campi · Disciplinas · Cursos · Grades · Períodos · Matrículas · Ofertas · Turmas · Salas · Professores · Alunos · Responsáveis · Calendário · Segurança · Integrações · Notificações · Configurações`
  (`Agenda`, `Frequência` e `Filhos` são de Teacher/Student/Parent e não entram aqui.)
- Padrão de página com abas já existe em `security.vue:4-17` e `integrations.vue:4-14`: `UDashboardToolbar` + `UNavigationMenu highlight` + `<NuxtPage />`.

## Problemas identificados

| # | Problema | Evidência |
|---|---|---|
| 1 | Lista plana de 18 itens estoura o "7±2" — vira varredura visual, não navegação | `allLinks` é um array único |
| 2 | Ordem sem lógica de domínio | `Campi` (infra) antes de `Disciplinas` (catálogo); `Calendário` entre `Responsáveis` e `Segurança` |
| 3 | Config/admin misturado com operação diária | `Segurança`, `Integrações`, `Configurações` (uso raro) competem com `Turmas`, `Matrículas` (uso diário) |
| 4 | Páginas que são a mesma coisa aparecem separadas | `AccessEnrollmentsPage` e `AccessPeriodsPage` usam a **mesma** permissão `ManagePeriods` (`store.ts:142-147`) |
| 5 | "Notificações" é ambíguo | Item da sidebar (notificações da instituição) **e** sino de notificações pessoais no topo (`default.vue:146`) |
| 6 | Command palette repete a lista plana | `groups` tem um único grupo `"Go to"` |

---

## Estrutura proposta

Agrupamento pelo ciclo de vida do domínio acadêmico — catálogo → operação → pessoas → infra → sistema:

```
  Home

  ACADÊMICO              ← o que a instituição oferece (catálogo, muda pouco)
    Cursos
    Grades
    Disciplinas

  SECRETARIA             ← o que está rodando agora (uso diário)
    Turmas
    Ofertas
    Períodos
    Matrículas
    Calendário
    Comunicados

  PESSOAS
    Alunos
    Professores
    Responsáveis

  INFRAESTRUTURA
    Campi
    Salas

  SISTEMA                ← uso raro, empurrado pro fim
    Segurança
    Integrações
    Configurações
```

Decisões menos óbvias:

- **Acadêmico vs. Secretaria** contrastam por *função*, não por tempo: o que a instituição oferece vs. quem opera o dia a dia disso. "Secretaria" é o vocabulário que o próprio usuário do SGA já usa.
- **Ofertas** fica em Secretaria, não em Acadêmico: `CreateCourseOfferingIn` exige `AcademicPeriodId` + `CampusId` — é execução, não catálogo.
- **Calendário** fica em Secretaria: define dias letivos/feriados do período corrente.
- **Comunicados** (ex-"Notificações") fica em Secretaria: é conteúdo, não configuração — era o único item de "Sistema" que não era ajuste.
- **Home** fica solta no topo, sem grupo — é destino, não categoria.

---

## Fase 1 — Agrupamento visual (sem mexer em rotas)

O `UNavigationMenu` do Nuxt UI 4 já suporta isso nativamente, sem componente novo:

- `items` como `NavigationMenuItem[][]` renderiza um `separator` entre listas (`NavigationMenu.vue:333`) — e o separator **não** é escondido no modo colapsado.
- Itens com `type: 'label'` renderizam só quando `vertical && !collapsed` (`NavigationMenu.vue:184`) — os títulos dos grupos somem sozinhos ao colapsar a sidebar, sobrando só os separadores entre os clusters de ícones. É exatamente o comportamento desejado, de graça.

### 1.1 Estrutura de dados

Em `Web/app/layouts/default.vue`, trocar `allLinks` por `sidebarGroups`:

```ts
const sidebarGroups = [
  { label: null,             items: [ /* Home */ ] },
  { label: 'Acadêmico',      items: [ /* Cursos, Grades, Disciplinas */ ] },
  { label: 'Secretaria',     items: [ /* Períodos, Ofertas, Matrículas, Turmas, Calendário, Comunicados */ ] },
  { label: 'Pessoas',        items: [ /* Alunos, Professores, Responsáveis */ ] },
  { label: 'Infraestrutura', items: [ /* Campi, Salas */ ] },
  { label: 'Sistema',        items: [ /* Segurança, Integrações, Configurações */ ] },
]
```

Cada item mantém o shape atual (`label`, `icon`, `to`, `policy`).

### 1.2 Computed `links`

Reescrever para retornar `NavigationMenuItem[][]`:

- filtrar os itens de cada grupo por `can(policy).value` (lógica atual, `default.vue:56`);
- **descartar grupos que ficaram vazios** — crítico: um role customizado com poucas permissões não pode ver títulos de seção órfãos;
- prefixar o item `type: 'label'` só quando o grupo tem label e sobrou ≥1 item.

### 1.3 Turmas do professor

O bloco `canSeeTeacherClasses` (`default.vue:65-72`) vira seu próprio grupo no array externo, em vez de um `push` na lista plana.

### 1.4 Command palette

Atualizar o computed `groups` (`default.vue:77-91`) para emitir **um grupo por seção** (`id: 'academico'`, `label: 'Acadêmico'`, ...) em vez do único `"Go to"`. Reaproveita a mesma estrutura e melhora o `Ctrl+K` de graça.

Nada de rota, policy ou middleware muda nesta fase. Risco baixo, reversível.

---

## Fase 2 — Rename: Notificações → Comunicados

Resolve o problema #5. É **só texto de interface**: a rota continua `/notifications` e a policy continua `AccessNotificationsPage`. O projeto já usa rota em inglês + label em português (`/course-offerings` → "Ofertas", `/enrollments` → "Matrículas"), então renomear a rota seria churn sem ganho — mexeria no mapa do `policy.global.ts`, no nome da policy no front e no back.

| Arquivo | Ocorrências |
|---|---|
| `app/layouts/default.vue:32` | label da sidebar |
| `app/pages/notifications/index.vue` | `:56` título da navbar; `:74` e `:79` labels do botão "Notificação" |
| `app/pages/notifications/[id].vue` | `:53` título fallback; `:73` "não encontrada"; `:79` "Dados da notificação" |
| `app/components/notifications/CreateModal.vue` | `:51` toast; `:55` mensagem de erro; `:66` título; `:68` descrição; `:109` label do botão |

Cuidado com concordância: "Notificação criada" → "Comunicado criado", "não encontrada" → "não encontrado".

**Não mexer:** `app/components/NotificationsSlideover.vue` e o sino em `default.vue:146` continuam "Notificações" — é justamente a distinção que o rename cria. Nomes de arquivo, componentes e endpoints também ficam como estão.

---

## Fase 3 — Consolidar páginas irmãs em abas (opcional)

Segue o padrão já existente em `security.vue` / `integrations.vue`. Reduz 18 → 16 itens.

### 3.1 `Períodos` + `Matrículas` → página `Períodos` com abas

Justificativa forte: as duas páginas já dependem da **mesma** permissão (`ManagePeriods`), e período de matrícula é um sub-conceito do período letivo.

- criar `pages/periods/index.vue` (períodos) + `pages/periods/enrollments.vue`;
- converter `pages/periods.vue` no shell com abas;
- em `middleware/policy.global.ts`, trocar a entrada `'/enrollments'` por `'/periods/enrollments'`;
- adicionar redirect `/enrollments` → `/periods/enrollments` (links antigos/bookmarks).

### 3.2 `Campi` + `Salas` → página `Infraestrutura` com abas

- mesma mecânica;
- **atenção**: já existem `pages/campus/[id].vue` e `pages/classrooms/[classroomId]` — as rotas de detalhe precisam ser realocadas ou mantidas fora do shell;
- são permissões distintas (`ManageCampi` / `ManageClassrooms`), então cada aba precisa da sua policy e a página precisa de uma `AccessInfrastructurePage` com `hasAnyPermission(...)` — o padrão que `AccessSecurityPage` já usa (`store.ts:299-305`).

### 3.3 Descartado

**Não** consolidar Alunos/Professores/Responsáveis em abas: são listas grandes com filtros próprios e rotas de detalhe já estabelecidas (`/students/[studentId]`, etc.). O agrupamento visual da Fase 1 resolve.

---

## Ordem de execução e validação

1. **Fase 1 + Fase 2** juntas — arquivos de front apenas, sem mudança de rota.
2. **Fase 3.1** (Períodos + Matrículas) — precisa de teste de rota/redirect.
3. **Fase 3.2** (Campi + Salas) — só se a 3.1 validar bem.

Validação manual (não há teste automatizado cobrindo a sidebar hoje):

- sidebar expandida e colapsada;
- usuário Diretor (todos os grupos) **e** usuário com role restrito (grupos vazios devem sumir, sem títulos órfãos);
- `Ctrl+K` com os grupos novos.

O toolchain do `Web/` não roda no WSL — `typecheck` / `lint` / `build` precisam rodar no Windows.
