# Plano — Frontend da feature Timetabling

## Decisões de escopo (v1)

- **Edição manual:** rodar → revisar → realocar por evento (modal com validação de conflito ao vivo, sem drag-and-drop).
- **Backend:** front-only contra um contrato existente/assumido (o solver `TimetablingSolver.cs` ainda é stub; os endpoints são premissa).
- **Commit:** preview efêmero (vive no cliente) → botão Confirmar cria as `Class` + `Schedule` reais.

## 1. Tese de design

Isto não é uma landing — é uma **ferramenta de operador** dentro de um dashboard Nuxt UI já estabelecido (primary `violet`, neutral `zinc`, `UDashboardPanel`). A identidade visual já existe; inventar outra brigaria com o resto do app. Então **a assinatura é a interação, não a paleta**: uma *mesa de alocação* onde o resultado do solver guloso vive como blocos coloridos numa grade semanal e — a parte incomum — os **eventos não-alocados são cidadãos de primeira classe**, ancorados numa bandeja fixa que não some até você resolvê-los. A falha do algoritmo é o herói da tela, não uma nota de rodapé.

Reaproveita o vocabulário visual que já funciona:

- A grade estilo Google Calendar de `app/components/agenda/Week.vue` (posicionamento absoluto por minutos, linha do "agora", moldura arredondada).
- A paleta por-disciplina daquele mesmo arquivo (13 cores estáveis por ordem de aparição).
- Cores semânticas de estado do `app/pages/calendar.vue` (`info`/`warning`/`error` com `/15` de fundo) para os estados de conflito.

**O risco justificável:** um único bloco de aula aparece simultaneamente em três lentes (coorte, professor, sala). Quando você troca o foco entre lentes, o mesmo evento acende nas três — deixando o operador *sentir* as três restrições duras ao mesmo tempo. É a tradução direta dos 3 mapas de ocupação do `Timetabling.md`.

## 2. Escopo, rota e navegação

O solver roda por `(InstitutionId, CampusId, AcademicPeriodId)` — o `InstitutionId` vem do `ctx.RequestUser`, então a UI precisa de **dois seletores: Campus + Período letivo**. Sem os dois, nada roda.

- Rota: `app/pages/timetabling/index.vue`
- Nav: novo item em `app/layouts/default.vue` — `{ label: 'Horários', icon: 'i-lucide-calendar-clock', to: '/timetabling', policy: 'AccessTimetablingPage' }`, posicionado logo após **Ofertas** / antes de **Turmas** (é o passo que produz Turmas).
- Policy nova `AccessTimetablingPage` em `app/policies/` + atalho opcional em `useDashboard.ts`.

## 3. Contrato de API que o front assume

Front-only, mas os tipos precisam de forma. O front consome **três** operações (nomes/rotas a confirmar com quem define o backend):

| Op | Método/rota | Papel |
|---|---|---|
| **Preflight** | `GET /timetabling/preflight?campusId&periodId` | Diagnóstico ANTES de rodar: coortes derivados, disciplinas sem professor elegível, salas insuficientes, cargas horárias. Alimenta a fase "Preparação". |
| **Solve** | `POST /timetabling/solve` `{ campusId, periodId, options }` | Roda o guloso. Devolve **preview efêmero não persistido**: `classes[]` (cada uma com discipline/cohort/vacancies + `schedules[]` resolvidos) **e** `unallocated[]` com motivo. |
| **Commit** | `POST /timetabling/commit` `{ campusId, periodId, classes[] }` | Persiste o preview (possivelmente editado) como `Class` + `Schedule` reais, numa transação. |

O front trata Solve/Commit como stateless: o **preview vive no cliente** (o "back-end" do preview é a memória do Pinia/ref da página), e o Commit manda de volta o preview inteiro já com as edições manuais aplicadas. Isso mantém o backend simples e casa com a decisão "preview efêmero".

`options` do Solve (mapeia o `Timetabling.md`): `{ ordering: 'mostConstrainedFirst' | 'sequential' }` — expõe a "melhoria barata" (graph coloring guloso) como um toggle.

## 4. Máquina de estados da tela

```
   ┌─ Idle ──────────── faltando campus/período
   │
   ├─ Ready ─────────── escopo escolhido; preflight carregado (pode ter avisos)
   │        │
   │        └── (Rodar) ──▶ Solving (spinner + skeleton da grade)
   │
   ├─ Solved:Full ───── 0 não-alocados       → pode commitar direto
   ├─ Solved:Partial ── N não-alocados        → bandeja em destaque
   │        │
   │        └── (editar/realocar evento) ──▶ Dirty (preview local mudou)
   │
   ├─ Committing ────── gravando as turmas
   └─ Committed ─────── sucesso; oferece ir pra /classes
```

Vetores de mudança que a tela precisa absorver sem perder o trabalho: trocar escopo (invalida o preview → confirma descarte), re-rodar (idem), reordenar option, editar um evento, resolver um não-alocado, commitar. Cada um transita explicitamente entre esses estados. Trocar campus/período com preview sujo → `UModal` de confirmação ("descartar horários gerados?").

## 5. Anatomia da tela — quatro zonas

```
┌──────────────────────────────────────────────────────────────────┐
│ ScopeBar:  [Campus ▾] [Período ▾]   ordenação:[+restrito▾]  [Rodar]│  ← sempre visível
├──────────────────────────────────────────────────────────────────┤
│ HealthStrip: 42 aulas · 12 coortes · 3 não-alocadas · 2 avisos     │  ← resumo do preview
├───────────────┬──────────────────────────────────────────────────┤
│ Lente:        │                                                    │
│ ○ Coorte      │         GRADE SEMANAL (recurso selecionado)        │
│ ○ Professor   │         Seg  Ter  Qua  Qui  Sex                    │
│ ● Sala        │    07h [BD]      [POO]                             │
│               │    08h [BD]      [POO]     ⚠ conflito              │
│ ┌ lista ─────┐│    09h                                            │
│ │ Sala A  ●● ││    ...                                            │
│ │ Sala B  ●  ││                                                    │
│ │ Sala C ⚠   ││                                                    │
│ └────────────┘│                                                    │
├───────────────┴──────────────────────────────────────────────────┤
│ Bandeja NÃO-ALOCADOS (3):  [Cálculo I — sem sala livre 19h ▸]  ... │  ← fixa até esvaziar
├──────────────────────────────────────────────────────────────────┤
│                                        [Descartar]  [Confirmar N turmas]│
└──────────────────────────────────────────────────────────────────┘
```

**Zona 1 — ScopeBar** (fixa no `#header` do `UDashboardNavbar`). Campus + Período + ordenação + botão Rodar. Rodar fica `disabled` sem escopo; vira "Rodar de novo" quando já há preview.

**Zona 2 — HealthStrip.** Contadores derivados do preview (aulas, coortes, não-alocadas, avisos de preflight), no estilo da faixa de legendas de `calendar.vue`. Vermelho quando há não-alocadas.

**Zona 3 — Master-detail com lentes.** O núcleo. Ver §6.

**Zona 4 — Bandeja de não-alocados + barra de commit.** Ver §7 e §8.

## 6. As três lentes (o núcleo)

As 3 restrições duras são as 3 lentes — cada lente é a "agenda" de um recurso:

- **Coorte** (`CourseOffering × Period`): a restrição de aluno derivada da grade.
- **Professor**: inclui os vínculos `TeachersDisciplines`/`TeachersCampi` como restrições duras já filtradas.
- **Sala** (`Classroom`).

Layout **master-detail**: rail esquerdo lista os recursos da lente ativa, cada um com um medidor de ocupação (blocos ●) e um sino ⚠ se participa de algum conflito. Selecionar um recurso renderiza a grade semanal dele à direita — **um componente `TimetablingGrid` derivado do `agenda/Week.vue`**, mas com blocos clicáveis (abrem o modal de realocação, §8) em vez de virarem `NuxtLink`.

Por que master-detail e não uma grade gigante: há muitos coortes/salas; empilhar N grades não escala. E dentro de uma lente **um conflito é visualmente inevitável** — dois blocos no mesmo slot do mesmo recurso = dois cards sobrepostos, que renderiza lado a lado com borda `error` (o predicado é exatamente `Schedule.Conflict`, replicado no cliente).

Detalhe de continuidade: a cor do bloco é a **da disciplina** (mesma paleta, mesmo mapa por nome), estável entre lentes — então "BD" é sempre roxo, seja você olhando pela sala, pelo professor ou pelo coorte.

## 7. Bandeja de não-alocados

Fixa acima da barra de commit, **não colapsa enquanto tiver item**. Cada chip: disciplina + coorte + **motivo legível** vindo do solve (`sem sala livre`, `professor sem horário`, `choque de coorte`). Clicar o chip abre o mesmo modal de realocação (§8) já em modo "encaixar", oferecendo os slots livres. Resolver → o chip sai da bandeja e o bloco entra na grade com uma micro-animação de entrada (respeitando `prefers-reduced-motion`). Bandeja vazia → colapsa num selo verde "Todas as aulas alocadas".

Isto encapsula a limitação central do `Timetabling.md` ("guloso pode falhar mesmo existindo solução") como um fluxo de trabalho, não um erro.

## 8. Realocação por evento + validação de conflito ao vivo

Componente `EventReassignModal` — herda a estrutura do `SchedulesModal.vue` (que já resolve dia/início/fim + a regra 0/1/2 professores) mas opera sobre **um** evento e valida contra o preview inteiro **no cliente, ao vivo**:

- Ao escolher dia/hora/sala/professor, reimplementa `Schedule.Conflict` em TS e checa as 3 ocupações (coorte, professor, sala) contra todos os outros schedules do preview. Conflito → borda/hint `error` imediato, botão Salvar `disabled` (mesmo padrão de `rowBadRange`/`hasErrors` do `SchedulesModal.vue`).
- Regra do professor **reusada exatamente** do `UpdateClassSchedulesService.cs`: 0 professores → slot sem professor; 1 → preenche sozinho; 2 → `USelect` obrigatório por horário. `pickTeacher = teachers.length >= 2`, igual ao `SchedulesModal.vue`.
- Só oferece professores elegíveis (`TeachersDisciplines` ∩ `TeachersCampi`) e salas do campus — restrições duras nunca ficam escolhíveis.
- Salvar **muta o preview local** (estado `Dirty`), reordena os medidores das lentes e re-deriva a HealthStrip. Nada vai ao servidor até o Commit.

Um `useTimetablingPreview` composable centraliza: o preview, os índices de ocupação (3 `Map`s espelhando os 3 `HashSet`s do algoritmo), `conflictsOf(schedule)`, `freeSlotsFor(event)`, e os contadores derivados. É a única fonte de verdade do cliente.

## 9. Fluxo de commit

Barra inferior: `[Descartar]` e `[Confirmar N turmas]`. Confirmar → `UModal` de resumo (N turmas, M horários, avisa se ainda há não-alocados: "3 aulas ficarão sem horário — confirmar mesmo assim?") → `POST /timetabling/commit` com o preview editado → toast de sucesso (padrão do `SchedulesModal`) → oferece navegar pra `/classes`. Erro do servidor → toast com `err.data.message`, preview preservado.

## 10. Arquivos a criar (segue as convenções do repo)

```
app/pages/timetabling/index.vue              # orquestra estados, scope, solve, commit
app/types/timetabling.ts                     # DTOs In/Out + tipos de preview
app/composables/useTimetablingPreview.ts     # fonte de verdade do preview + ocupação + conflitos
app/components/timetabling/ScopeBar.vue       # campus/período/ordenação/rodar
app/components/timetabling/HealthStrip.vue    # contadores derivados
app/components/timetabling/LensSwitcher.vue   # Coorte/Professor/Sala + rail de recursos
app/components/timetabling/TimetablingGrid.vue# grade semanal editável (fork do agenda/Week)
app/components/timetabling/UnallocatedTray.vue# bandeja + chips com motivo
app/components/timetabling/EventReassignModal.vue # realocação + validação ao vivo
app/components/timetabling/CommitModal.vue    # resumo + confirmação
```

Convenções respeitadas: `@click="() => { ... }"` sempre arrow-block (CLAUDE.md); Zod `z.string({ error: '...' }).min(1, ...)` se houver form; `blur()` em botões que abrem overlay; strings via `HasValue()`/`IsEmpty()` se tocar backend.

## 11. Tipos TS (núcleo de `types/timetabling.ts`)

```ts
export interface PreviewSchedule {
  day: string; startAt: string; endAt: string   // 'Monday', 'H07_00'
  classroomId: number | null; classroom: string | null
  teacherId: number | null; teacher: string | null
}
export interface PreviewClass {
  tempId: string                                  // id de cliente até o commit
  disciplineId: number; discipline: string
  cohortId: string; cohort: string                // CourseOffering × Period
  vacancies: number; workload: number
  schedules: PreviewSchedule[]
}
export type UnallocatedReason = 'noRoom' | 'noTeacherSlot' | 'cohortClash' | 'noEligibleTeacher'
export interface UnallocatedEvent {
  disciplineId: number; discipline: string
  cohortId: string; cohort: string
  slotsNeeded: number; reason: UnallocatedReason
}
export interface SolveOut { classes: PreviewClass[]; unallocated: UnallocatedEvent[] }
export type Lens = 'cohort' | 'teacher' | 'classroom'
```

## 12. Fases de construção

1. **Esqueleto + escopo:** página, nav, policy, ScopeBar, máquina de estados, chamada de Solve com dados mockados. Prova o fluxo Idle→Ready→Solved.
2. **Grade read-only:** `TimetablingGrid` (fork do `agenda/Week.vue`) + LensSwitcher + HealthStrip. Ver o preview nas 3 lentes.
3. **Não-alocados:** UnallocatedTray com motivos + estado Partial.
4. **Edição + conflito ao vivo:** `useTimetablingPreview`, `EventReassignModal`, replicação de `Schedule.Conflict` e da regra 0/1/2 professores. Estado Dirty.
5. **Commit:** CommitModal + POST + navegação.
6. **Polimento:** responsivo (grade rola em `overflow-x-auto` como o Week já faz), foco de teclado nos blocos, `reduced-motion`, empty/error states com voz de interface, atalho de teclado.

## 13. Onde gastar a ousadia (e onde segurar)

Uma coisa memorável — o **realce cross-lens do mesmo evento** — e tudo ao redor fica quieto e disciplinado: sem gradientes, sem números decorativos, sem animação além da entrada/saída de blocos na resolução de conflito. A grade herda espaçamento e tipografia do resto do app. Copy no registro do produto: botões dizem o que fazem ("Confirmar 42 turmas", não "Enviar"), motivos de não-alocação explicam e apontam a saída ("sem sala livre às 19h — realocar").
