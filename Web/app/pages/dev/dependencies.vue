<script setup lang="ts">
type GroupId = 'estrutura' | 'pessoas' | 'catalogo' | 'calendario' | 'execucao'

type DepKind = 'required' | 'optional' | 'auto'

interface Dep {
  id: string
  kind?: DepKind
}

interface EntityNode {
  id: string
  label: string
  entity: string
  group: GroupId
  deps: Dep[]
  note?: string
}

interface InfraEntity {
  entity: string
  deps: string
  note: string
}

const groups: Record<GroupId, { label: string, order: number }> = {
  estrutura: { label: 'Estrutura', order: 0 },
  pessoas: { label: 'Pessoas & Acesso', order: 1 },
  catalogo: { label: 'Catálogo Acadêmico', order: 2 },
  calendario: { label: 'Calendário', order: 3 },
  execucao: { label: 'Turmas & Execução', order: 4 },
}

const nodes: EntityNode[] = [
  { id: 'institution', label: 'Instituição', entity: 'Institution', group: 'estrutura', deps: [], note: 'Raiz de todo o sistema. Todas as demais entidades pertencem a uma instituição (multi-tenant via InstitutionId).' },
  { id: 'institutionConfig', label: 'Config. da Instituição', entity: 'InstitutionConfig', group: 'estrutura', deps: [{ id: 'institution' }] },
  { id: 'campus', label: 'Campus', entity: 'Campus', group: 'estrutura', deps: [{ id: 'institution' }] },
  { id: 'classroom', label: 'Sala de Aula', entity: 'Classroom', group: 'estrutura', deps: [{ id: 'campus' }] },

  { id: 'user', label: 'Usuário', entity: 'EstudUser', group: 'pessoas', deps: [{ id: 'institution' }] },
  { id: 'role', label: 'Perfil de Acesso', entity: 'EstudRole', group: 'pessoas', deps: [{ id: 'institution' }] },
  { id: 'teacher', label: 'Professor', entity: 'EstudTeacher', group: 'pessoas', deps: [{ id: 'institution' }, { id: 'user', kind: 'auto' }], note: 'O usuário do professor é criado automaticamente na mesma operação (CreateTeacher).' },
  { id: 'student', label: 'Estudante', entity: 'EstudStudent', group: 'pessoas', deps: [{ id: 'institution' }, { id: 'user', kind: 'auto' }], note: 'O usuário do estudante é criado automaticamente na mesma operação (CreateStudent).' },
  { id: 'teacherCampus', label: 'Professor ↔ Campus', entity: 'TeacherCampus', group: 'pessoas', deps: [{ id: 'teacher' }, { id: 'campus' }], note: 'Vínculo do professor com os campi onde pode lecionar.' },
  { id: 'teacherDiscipline', label: 'Professor ↔ Disciplina', entity: 'TeacherDiscipline', group: 'pessoas', deps: [{ id: 'teacher' }, { id: 'discipline' }], note: 'Vínculo do professor com as disciplinas que pode lecionar.' },

  { id: 'discipline', label: 'Disciplina', entity: 'Discipline', group: 'catalogo', deps: [{ id: 'institution' }] },
  { id: 'course', label: 'Curso', entity: 'Course', group: 'catalogo', deps: [{ id: 'institution' }] },
  { id: 'courseDiscipline', label: 'Curso ↔ Disciplina', entity: 'CourseDiscipline', group: 'catalogo', deps: [{ id: 'course' }, { id: 'discipline' }], note: 'Vínculo da disciplina ao curso. Pré-requisito para a disciplina entrar numa grade curricular.' },
  { id: 'courseCurriculum', label: 'Grade Curricular', entity: 'CourseCurriculum', group: 'catalogo', deps: [{ id: 'course' }, { id: 'courseDiscipline' }], note: 'Só aceita disciplinas já vinculadas ao curso (valida contra CoursesDisciplines).' },
  { id: 'courseOffering', label: 'Oferta de Curso', entity: 'CourseOffering', group: 'catalogo', deps: [{ id: 'campus' }, { id: 'course' }, { id: 'courseCurriculum' }, { id: 'academicPeriod' }], note: 'Oferta de um curso num campus, com uma grade, num período acadêmico.' },

  { id: 'academicPeriod', label: 'Período Acadêmico', entity: 'AcademicPeriod', group: 'calendario', deps: [{ id: 'institution' }] },
  { id: 'enrollmentPeriod', label: 'Período de Matrícula', entity: 'EnrollmentPeriod', group: 'calendario', deps: [{ id: 'institution' }] },
  { id: 'calendarDay', label: 'Dia de Calendário', entity: 'CalendarDay', group: 'calendario', deps: [{ id: 'institution' }] },

  { id: 'class', label: 'Turma', entity: 'Class', group: 'execucao', deps: [{ id: 'discipline' }, { id: 'academicPeriod' }, { id: 'campus', kind: 'optional' }, { id: 'teacher', kind: 'optional' }, { id: 'teacherCampus', kind: 'optional' }, { id: 'teacherDiscipline', kind: 'optional' }], note: 'Campus e professor são opcionais na criação. Se houver professor, ele precisa estar vinculado à disciplina da turma e, se houver campus, também ao campus.' },
  { id: 'schedule', label: 'Horário', entity: 'Schedule', group: 'execucao', deps: [{ id: 'class', kind: 'auto' }], note: 'Criado junto com a turma (lista de horários no CreateClass).' },
  { id: 'classLesson', label: 'Aula', entity: 'ClassLesson', group: 'execucao', deps: [{ id: 'class', kind: 'auto' }], note: 'Geradas automaticamente na criação da turma, a partir dos horários e das datas do período acadêmico.' },
  { id: 'classroomClass', label: 'Sala ↔ Turma', entity: 'ClassroomClass', group: 'execucao', deps: [{ id: 'classroom' }, { id: 'class' }], note: 'Alocação da turma numa sala de aula.' },
  { id: 'studentCourseEnrollment', label: 'Matrícula em Oferta', entity: 'StudentCourseEnrollment', group: 'execucao', deps: [{ id: 'student' }, { id: 'courseOffering' }], note: 'Matrícula do estudante numa oferta de curso.' },
  { id: 'classStudent', label: 'Estudante da Turma', entity: 'ClassStudent', group: 'execucao', deps: [{ id: 'class' }, { id: 'student' }], note: 'A turma precisa estar com status "Em matrícula" e ter vagas disponíveis.' },
  { id: 'classActivity', label: 'Atividade', entity: 'ClassActivity', group: 'execucao', deps: [{ id: 'class' }], note: 'Criada pelo professor vinculado à turma.' },
  { id: 'classActivityWork', label: 'Trabalho de Atividade', entity: 'ClassActivityWork', group: 'execucao', deps: [{ id: 'classActivity' }, { id: 'classStudent' }], note: 'Entrega do estudante matriculado na turma.' },
  { id: 'attendance', label: 'Frequência', entity: 'ClassLessonAttendance', group: 'execucao', deps: [{ id: 'classLesson' }, { id: 'classStudent' }], note: 'Presença do estudante numa aula da turma.' },
  { id: 'studentClassNote', label: 'Nota do Estudante', entity: 'StudentClassNote', group: 'execucao', deps: [{ id: 'classStudent' }], note: 'Nota do estudante numa turma.' },
]

const infraEntities: InfraEntity[] = [
  { entity: 'EstudUserRole', deps: 'Usuário, Perfil de Acesso', note: 'Vínculo do usuário com um perfil.' },
  { entity: 'InstitutionRole', deps: 'Instituição, Perfil de Acesso', note: 'Perfis disponíveis na instituição.' },
  { entity: 'Command / CommandBatch', deps: 'Instituição, Usuário', note: 'Processamento assíncrono (Quartz). Criados pelos services via ctx.AddCommand.' },
  { entity: 'Notification', deps: 'Instituição', note: 'Notificação institucional.' },
  { entity: 'UserNotification', deps: 'Notification, Usuário', note: 'Entrega da notificação a um usuário.' },
  { entity: 'WebhookSubscription', deps: 'Instituição', note: 'Inscrição de webhook.' },
  { entity: 'WebhookCall', deps: 'WebhookSubscription', note: 'Disparo de evento para uma inscrição.' },
  { entity: 'WebhookCallAttempt', deps: 'WebhookCall', note: 'Tentativa de entrega de um disparo.' },
  { entity: 'ReceivedWebhookEvent', deps: '—', note: 'Evento recebido de sistema externo.' },
  { entity: 'UserActivity', deps: 'Usuário', note: 'Trilha de atividades do usuário.' },
  { entity: 'MagicLink / ResetPasswordToken / UserSocialLogin', deps: 'Usuário', note: 'Artefatos de autenticação.' },
  { entity: 'SsoConfiguration', deps: 'Instituição', note: 'Configuração de SSO.' },
  { entity: 'SsoAllowedDomain', deps: 'SsoConfiguration', note: 'Domínios permitidos no SSO.' },
  { entity: 'AuditTrail / AuditChange', deps: '—', note: 'Gerados automaticamente no SaveChanges.' },
]

interface RankedEntity {
  id: string
  reason: string
  rank?: number
}

interface ComplexityTier {
  title: string
  subtitle: string
  items: RankedEntity[]
}

const complexityTiers: ComplexityTier[] = [
  {
    title: 'Nível 0 — Raiz',
    subtitle: 'Não depende de nada.',
    items: [
      { id: 'institution', reason: 'Não depende de nada. É a raiz do multi-tenant; tudo o mais nasce dentro dela.' },
    ],
  },
  {
    title: 'Nível 1 — Só precisam da Instituição',
    subtitle: 'Criação trivial: uma FK e validações de formato.',
    items: [
      { id: 'calendarDay', reason: 'Só a instituição e uma data. Sem regras de domínio relevantes.' },
      { id: 'enrollmentPeriod', reason: 'Instituição + nome + datas; única regra é data de início antes do fim.' },
      { id: 'academicPeriod', reason: 'Mesmo caso: nome e datas válidas.' },
      { id: 'discipline', reason: 'Instituição + nome/código. Validação só de formato.' },
      { id: 'course', reason: 'Instituição + nome/tipo. Validação só de formato.' },
      { id: 'campus', reason: 'Instituição + nome/estado/cidade/capacidade.' },
      { id: 'role', reason: 'Instituição + nome/permissões; valida unicidade do nome e lista de permissões, mas nada estrutural.' },
      { id: 'institutionConfig', reason: '1:1 com a instituição, só setup.' },
    ],
  },
  {
    title: 'Nível 2 — Uma dependência intermediária ou criação composta',
    subtitle: 'Cadeia curta, mas já são o segundo passo de um fluxo.',
    items: [
      { id: 'classroom', reason: 'Precisa de um Campus (que precisa da instituição). Cadeia curta, sem regra extra.' },
      { id: 'student', reason: 'Como pré-requisito só precisa da instituição, mas a criação é composta: o service cria o EstudUser junto, valida e-mail único no sistema inteiro, atribui role e ainda dispara webhooks.' },
      { id: 'teacher', reason: 'Mesmo caso do estudante: criação composta com o usuário, e-mail único e role — mais "trabalho" que os anteriores, embora a cadeia seja rasa.' },
      { id: 'courseDiscipline', reason: 'Só uma junção, mas exige as duas pontas (Curso e Disciplina) criadas antes.' },
      { id: 'teacherCampus', reason: 'Junção Professor + Campus; simples de criar, porém depende do professor já existir.' },
      { id: 'teacherDiscipline', reason: 'Junção Professor + Disciplina; idem.' },
    ],
  },
  {
    title: 'Nível 3 — Regras de consistência entre entidades',
    subtitle: 'O pré-requisito não é só uma FK: é um estado do catálogo.',
    items: [
      { id: 'courseCurriculum', reason: 'Precisa do Curso e das disciplinas já vinculadas ao curso: o service rejeita qualquer disciplina fora de CoursesDisciplines (InvalidDisciplinesList).' },
      { id: 'courseOffering', reason: '4 FKs diretas (Campus, Curso, Grade, Período Acadêmico), e a grade arrasta toda a cadeia do catálogo — transitivamente ~7 entidades antes.' },
    ],
  },
  {
    title: 'Nível 4 — Muitas dependências + validações cruzadas + efeitos colaterais',
    subtitle: 'Services com checagens cruzadas e cadeias transitivas longas.',
    items: [
      { id: 'class', reason: 'A criação mais "inteligente" do sistema: exige Disciplina e Período; se informar Professor, valida vínculo com a disciplina (e com o campus, se houver); valida horários; e gera automaticamente todas as Aulas cruzando horários com as datas do período.' },
      { id: 'studentCourseEnrollment', reason: 'A junção em si é simples, mas transitivamente é uma das cadeias mais longas: Estudante + Oferta completa (campus, curso, grade, disciplinas vinculadas, período) — ~9 entidades antes.' },
    ],
  },
  {
    title: 'Nível 5 — Exigem estado, não só existência',
    subtitle: 'O pré-requisito é uma transição de estado ou identidade específica.',
    items: [
      { id: 'classStudent', reason: 'Não basta Turma e Estudante existirem: a turma precisa estar "Em matrícula" (ReleaseClassForEnrollment) e ter vagas disponíveis.' },
      { id: 'classActivity', reason: 'Precisa da Turma com professor atribuído, e só o próprio professor da turma pode criar (validação de identidade, não só de existência).' },
    ],
  },
  {
    title: 'Nível 6 — Fim da cadeia: dependem de tudo acima',
    subtitle: 'Praticamente todas as entidades acadêmicas precisam existir antes.',
    items: [
      { id: 'studentClassNote', reason: 'Precisa do estudante já matriculado na turma, que arrasta toda a cadeia anterior.' },
      { id: 'attendance', reason: 'Precisa de uma Aula (gerada na turma) + estudante matriculado. Na prática exige a turma iniciada.' },
      { id: 'classActivityWork', reason: 'O mais profundo do grafo: Atividade (turma com professor) + estudante matriculado. Instituição → campus/disciplina/curso/período → professor com vínculos → turma liberada → estudante matriculado → atividade → aí sim o trabalho.' },
    ],
  },
]

let complexityRank = 1
for (const tier of complexityTiers) {
  for (const item of tier.items) item.rank = complexityRank++
}

const NODE_W = 164
const NODE_H = 52
const COL_GAP = 84
const ROW_GAP = 14
const PAD = 16

const nodeById = new Map(nodes.map(n => [n.id, n]))

const layerOf = (() => {
  const memo = new Map<string, number>()
  function calc(id: string): number {
    const cached = memo.get(id)
    if (cached !== undefined) return cached
    const node = nodeById.get(id)
    if (!node || node.deps.length === 0) {
      memo.set(id, 0)
      return 0
    }
    memo.set(id, -1)
    const layer = 1 + Math.max(...node.deps.map(d => calc(d.id)))
    memo.set(id, layer)
    return layer
  }
  return calc
})()

interface Edge {
  from: string
  to: string
  kind: DepKind
}

const edges: Edge[] = nodes.flatMap(n => n.deps.map(d => ({ from: d.id, to: n.id, kind: d.kind ?? 'required' as DepKind })))

const dependentsOf = new Map<string, string[]>()
for (const e of edges) {
  const list = dependentsOf.get(e.from) ?? []
  list.push(e.to)
  dependentsOf.set(e.from, list)
}

const layout = computed(() => {
  const cols: EntityNode[][] = []
  for (const n of nodes) {
    const layer = layerOf(n.id)
    cols[layer] ??= []
    cols[layer].push(n)
  }

  const maxCount = Math.max(...cols.map(c => c.length))
  const height = maxCount * NODE_H + (maxCount - 1) * ROW_GAP + PAD * 2
  const width = PAD * 2 + cols.length * NODE_W + (cols.length - 1) * COL_GAP

  const pos = new Map<string, { x: number, y: number }>()

  cols.forEach((col, layerIndex) => {
    const barycenter = (n: EntityNode): number => {
      const ys = n.deps.map(d => pos.get(d.id)?.y).filter((y): y is number => y !== undefined)
      if (ys.length === 0) return groups[n.group].order * 1000
      return ys.reduce((a, b) => a + b, 0) / ys.length
    }
    col.sort((a, b) => {
      const diff = barycenter(a) - barycenter(b)
      if (diff !== 0) return diff
      return groups[a.group].order - groups[b.group].order
    })

    const colHeight = col.length * NODE_H + (col.length - 1) * ROW_GAP
    let y = (height - colHeight) / 2
    for (const n of col) {
      pos.set(n.id, { x: PAD + layerIndex * (NODE_W + COL_GAP), y })
      y += NODE_H + ROW_GAP
    }
  })

  return { pos, width, height, levels: cols.length }
})

function edgePath(e: Edge): string {
  const s = layout.value.pos.get(e.from)
  const t = layout.value.pos.get(e.to)
  if (!s || !t) return ''
  const sx = s.x + NODE_W
  const sy = s.y + NODE_H / 2
  const tx = t.x
  const ty = t.y + NODE_H / 2
  const dx = Math.max(36, (tx - sx) / 2)
  return `M ${sx} ${sy} C ${sx + dx} ${sy}, ${tx - dx} ${ty}, ${tx} ${ty}`
}

const hovered = ref<string | null>(null)
const pinned = ref<string | null>(null)
const selected = computed(() => pinned.value ?? hovered.value)
const selectedNode = computed(() => selected.value ? nodeById.get(selected.value) ?? null : null)

const related = computed(() => {
  const id = selected.value
  if (!id) return null

  const up = new Set<string>()
  const collectUp = (current: string) => {
    for (const d of nodeById.get(current)?.deps ?? []) {
      if (!up.has(d.id)) {
        up.add(d.id)
        collectUp(d.id)
      }
    }
  }
  collectUp(id)

  const down = new Set<string>()
  const collectDown = (current: string) => {
    for (const next of dependentsOf.get(current) ?? []) {
      if (!down.has(next)) {
        down.add(next)
        collectDown(next)
      }
    }
  }
  collectDown(id)

  return { up, down }
})

function nodeDimmed(id: string): boolean {
  if (!related.value || !selected.value) return false
  if (id === selected.value) return false
  return !related.value.up.has(id) && !related.value.down.has(id)
}

function edgeHighlighted(e: Edge): boolean {
  const sel = selected.value
  if (!sel || !related.value) return false
  const upSide = new Set([sel, ...related.value.up])
  const downSide = new Set([sel, ...related.value.down])
  return (upSide.has(e.from) && upSide.has(e.to)) || (downSide.has(e.from) && downSide.has(e.to))
}

function edgeDimmed(e: Edge): boolean {
  if (!selected.value) return false
  return !edgeHighlighted(e)
}

const kindLabels: Record<DepKind, string> = {
  required: 'obrigatório',
  optional: 'opcional',
  auto: 'criado junto',
}

const directDependents = computed(() => {
  const id = selected.value
  if (!id) return []
  return (dependentsOf.get(id) ?? []).map(depId => nodeById.get(depId)).filter((n): n is EntityNode => !!n)
})

const nodesByGroup = computed(() => {
  const result: { group: GroupId, label: string, items: EntityNode[] }[] = []
  for (const [groupId, info] of Object.entries(groups) as [GroupId, { label: string, order: number }][]) {
    result.push({ group: groupId, label: info.label, items: nodes.filter(n => n.group === groupId) })
  }
  return result
})

function depsSummary(n: EntityNode): string {
  if (n.deps.length === 0) return '—'
  return n.deps.map((d) => {
    const label = nodeById.get(d.id)?.label ?? d.id
    const kind = d.kind && d.kind !== 'required' ? ` (${kindLabels[d.kind]})` : ''
    return `${label}${kind}`
  }).join(', ')
}

function selectNode(id: string) {
  pinned.value = pinned.value === id ? null : id
}

function clearSelection() {
  pinned.value = null
}
</script>

<template>
  <UDashboardPanel id="dev-dependencies">
    <template #header>
      <UDashboardNavbar title="Dependências de Entidades">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="dep-graph space-y-4">
        <div class="flex items-start justify-between gap-4 flex-wrap">
          <p class="text-sm text-muted">
            Ordem de criação das entidades do sistema: uma seta de A para B indica que A precisa existir antes de B ser criada.
            Passe o mouse ou clique numa entidade para destacar seus pré-requisitos e dependentes.
          </p>
        </div>

        <div class="flex items-center gap-x-5 gap-y-2 flex-wrap text-xs">
          <div v-for="(info, id) in groups" :key="id" class="flex items-center gap-1.5">
            <span class="size-2.5 rounded-full shrink-0" :style="{ backgroundColor: `var(--g-${id})` }" />
            <span class="text-toned">{{ info.label }}</span>
          </div>
          <span class="text-muted/60">|</span>
          <div class="flex items-center gap-1.5">
            <svg width="24" height="6" aria-hidden="true"><line x1="0" y1="3" x2="24" y2="3" class="stroke-(--edge-color)" stroke-width="1.5" /></svg>
            <span class="text-muted">obrigatório</span>
          </div>
          <div class="flex items-center gap-1.5">
            <svg width="24" height="6" aria-hidden="true"><line x1="0" y1="3" x2="24" y2="3" class="stroke-(--edge-color)" stroke-width="1.5" stroke-dasharray="5 3" /></svg>
            <span class="text-muted">opcional</span>
          </div>
          <div class="flex items-center gap-1.5">
            <svg width="24" height="6" aria-hidden="true"><line x1="0" y1="3" x2="24" y2="3" class="stroke-(--edge-color)" stroke-width="1.5" stroke-dasharray="2 3" /></svg>
            <span class="text-muted">criado junto / automático</span>
          </div>
        </div>

        <UCard :ui="{ body: 'p-3 sm:p-4' }">
          <div v-if="selectedNode" class="space-y-1.5">
            <div class="flex items-center gap-2 flex-wrap">
              <span class="size-2.5 rounded-full shrink-0" :style="{ backgroundColor: `var(--g-${selectedNode.group})` }" />
              <span class="font-semibold text-highlighted">{{ selectedNode.label }}</span>
              <code class="text-xs text-muted">{{ selectedNode.entity }}</code>
              <UBadge variant="subtle" color="neutral" size="sm">{{ groups[selectedNode.group].label }}</UBadge>
            </div>
            <p v-if="selectedNode.note" class="text-sm text-toned">{{ selectedNode.note }}</p>
            <p class="text-sm">
              <span class="text-muted">Precisa antes:</span>
              <template v-if="selectedNode.deps.length === 0"> <span class="text-toned">nada — é a raiz do sistema</span></template>
              <template v-else>
                <span v-for="(dep, i) in selectedNode.deps" :key="dep.id" class="text-toned">
                  {{ i > 0 ? ', ' : ' ' }}{{ nodeById.get(dep.id)?.label }}<span v-if="dep.kind && dep.kind !== 'required'" class="text-muted"> ({{ kindLabels[dep.kind] }})</span>
                </span>
              </template>
            </p>
            <p class="text-sm">
              <span class="text-muted">Desbloqueia:</span>
              <span v-if="directDependents.length === 0" class="text-toned"> nada</span>
              <span v-else class="text-toned"> {{ directDependents.map(d => d.label).join(', ') }}</span>
            </p>
          </div>
          <p v-else class="text-sm text-muted">
            Nenhuma entidade selecionada. Passe o mouse sobre o grafo para ver os detalhes; clique para fixar a seleção.
          </p>
        </UCard>

        <div class="overflow-x-auto rounded-lg border border-default bg-default">
          <svg
            :width="layout.width"
            :height="layout.height"
            :viewBox="`0 0 ${layout.width} ${layout.height}`"
            role="img"
            aria-label="Grafo de dependências de criação das entidades do sistema"
            @click="clearSelection"
          >
            <g>
              <path
                v-for="(e, i) in edges"
                :key="i"
                :d="edgePath(e)"
                fill="none"
                class="edge"
                :class="{ 'edge-hl': edgeHighlighted(e), 'edge-dim': edgeDimmed(e) }"
                :stroke-dasharray="e.kind === 'optional' ? '5 3' : e.kind === 'auto' ? '2 3' : undefined"
              />
            </g>

            <g
              v-for="n in nodes"
              :key="n.id"
              class="node cursor-pointer"
              :class="{ 'node-dim': nodeDimmed(n.id), 'node-selected': selected === n.id }"
              :style="{ '--node-color': `var(--g-${n.group})` }"
              :transform="`translate(${layout.pos.get(n.id)?.x ?? 0}, ${layout.pos.get(n.id)?.y ?? 0})`"
              @mouseenter="() => { hovered = n.id }"
              @mouseleave="() => { hovered = null }"
              @click="(e) => { e.stopPropagation(); selectNode(n.id) }"
            >
              <rect :width="NODE_W" :height="NODE_H" rx="8" class="node-box" />
              <rect x="0" y="10" width="3" :height="NODE_H - 20" rx="1.5" fill="var(--node-color)" />
              <text x="14" y="22" font-size="12.5" font-weight="600" class="node-label">{{ n.label }}</text>
              <text x="14" y="39" font-size="10" class="node-entity">{{ n.entity }}</text>
            </g>
          </svg>
        </div>

        <div class="space-y-3">
          <h2 class="text-base font-semibold text-highlighted">Tabela de pré-requisitos</h2>
          <div v-for="section in nodesByGroup" :key="section.group" class="space-y-1.5">
            <div class="flex items-center gap-1.5">
              <span class="size-2.5 rounded-full shrink-0" :style="{ backgroundColor: `var(--g-${section.group})` }" />
              <h3 class="text-sm font-medium text-toned">{{ section.label }}</h3>
            </div>
            <div class="overflow-x-auto rounded-lg border border-default">
              <table class="w-full text-sm">
                <thead>
                  <tr class="border-b border-default bg-elevated/50 text-left text-xs text-muted">
                    <th class="px-3 py-2 font-medium">Entidade</th>
                    <th class="px-3 py-2 font-medium">Classe</th>
                    <th class="px-3 py-2 font-medium">Precisa antes</th>
                    <th class="px-3 py-2 font-medium">Observações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="n in section.items" :key="n.id" class="border-b border-default last:border-b-0">
                    <td class="px-3 py-2 text-highlighted font-medium whitespace-nowrap">{{ n.label }}</td>
                    <td class="px-3 py-2 text-muted whitespace-nowrap"><code class="text-xs">{{ n.entity }}</code></td>
                    <td class="px-3 py-2 text-toned">{{ depsSummary(n) }}</td>
                    <td class="px-3 py-2 text-muted">{{ n.note ?? '—' }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div class="space-y-1.5">
          <h2 class="text-base font-semibold text-highlighted">Entidades de infraestrutura</h2>
          <p class="text-sm text-muted">Entidades de suporte (identidade, mensageria, auditoria) que não participam do fluxo acadêmico de criação.</p>
          <div class="overflow-x-auto rounded-lg border border-default">
            <table class="w-full text-sm">
              <thead>
                <tr class="border-b border-default bg-elevated/50 text-left text-xs text-muted">
                  <th class="px-3 py-2 font-medium">Entidade</th>
                  <th class="px-3 py-2 font-medium">Precisa antes</th>
                  <th class="px-3 py-2 font-medium">Observações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="e in infraEntities" :key="e.entity" class="border-b border-default last:border-b-0">
                  <td class="px-3 py-2 text-highlighted whitespace-nowrap"><code class="text-xs">{{ e.entity }}</code></td>
                  <td class="px-3 py-2 text-toned">{{ e.deps }}</td>
                  <td class="px-3 py-2 text-muted">{{ e.note }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="space-y-3">
          <h2 class="text-base font-semibold text-highlighted">Ranking de complexidade de criação</h2>
          <p class="text-sm text-muted">
            Da entidade mais simples pra mais complexa de criar. O critério combina quantas entidades precisam existir antes
            (cadeia transitiva), quantas regras de negócio o service valida na criação e exigências de estado
            (não basta a entidade existir, ela precisa estar num status específico).
          </p>

          <UCard v-for="tier in complexityTiers" :key="tier.title" :ui="{ body: 'p-3 sm:p-4' }">
            <div class="space-y-3">
              <div>
                <h3 class="text-sm font-semibold text-highlighted">{{ tier.title }}</h3>
                <p class="text-xs text-muted">{{ tier.subtitle }}</p>
              </div>
              <ol class="space-y-2">
                <li v-for="item in tier.items" :key="item.id" class="flex gap-2.5 text-sm">
                  <span class="shrink-0 w-6 text-right font-mono text-xs text-muted pt-0.5">{{ item.rank }}.</span>
                  <div>
                    <div class="flex items-center gap-1.5 flex-wrap">
                      <span
                        class="size-2 rounded-full shrink-0"
                        :style="{ backgroundColor: `var(--g-${nodeById.get(item.id)?.group})` }"
                      />
                      <span class="font-medium text-highlighted">{{ nodeById.get(item.id)?.label }}</span>
                      <code class="text-xs text-muted">{{ nodeById.get(item.id)?.entity }}</code>
                    </div>
                    <p class="text-toned">{{ item.reason }}</p>
                  </div>
                </li>
              </ol>
            </div>
          </UCard>

          <UCard :ui="{ body: 'p-3 sm:p-4' }">
            <p class="text-sm text-toned">
              <span class="font-semibold text-highlighted">Resumo do padrão:</span>
              a complexidade cresce em três degraus — primeiro só <span class="italic">existência</span> (níveis 0–2),
              depois <span class="italic">consistência entre entidades</span> (grade só aceita disciplina do curso,
              professor só dá aula do que tem vínculo — níveis 3–4), e por fim <span class="italic">estado e identidade</span>
              (turma em matrícula, com vagas, professor da própria turma — níveis 5–6).
              Horário e Aula ficam fora do ranking porque não são criados diretamente: nascem junto com a Turma.
            </p>
          </UCard>
        </div>
      </div>
    </template>
  </UDashboardPanel>
</template>

<style scoped>
.dep-graph {
  --g-estrutura: #2a78d6;
  --g-pessoas: #008300;
  --g-catalogo: #e87ba4;
  --g-calendario: #eda100;
  --g-execucao: #1baf7a;
  --edge-color: var(--ui-border-accented);
}

.dark .dep-graph {
  --g-estrutura: #3987e5;
  --g-pessoas: #008300;
  --g-catalogo: #d55181;
  --g-calendario: #c98500;
  --g-execucao: #199e70;
}

.edge {
  stroke: var(--edge-color);
  stroke-width: 1.5;
  transition: opacity 0.15s ease, stroke 0.15s ease;
}

.edge-hl {
  stroke: var(--ui-primary);
  stroke-width: 2;
}

.edge-dim {
  opacity: 0.12;
}

.node {
  transition: opacity 0.15s ease;
}

.node-dim {
  opacity: 0.2;
}

.node-box {
  fill: var(--ui-bg-elevated);
  stroke: var(--ui-border-accented);
  stroke-width: 1;
  transition: stroke 0.15s ease;
}

.node:hover .node-box,
.node-selected .node-box {
  stroke: var(--node-color);
  stroke-width: 1.5;
}

.node-label {
  fill: var(--ui-text-highlighted);
}

.node-entity {
  fill: var(--ui-text-muted);
  font-family: var(--font-mono, monospace);
}
</style>
