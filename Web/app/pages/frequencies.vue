<script setup lang="ts">
import { breakpointsTailwind, useBreakpoints } from '@vueuse/core'
import type { AttendanceDay, GetStudentAttendanceCalendarOut, StudentDayAttendanceStatus } from '~/types/frequencies'

interface Cell {
  date: string | null
  status: StudentDayAttendanceStatus | null
}

interface Grid {
  key: string
  weeks: Cell[][]
  months: string[]
}

const MONTHS = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
// GitHub mostra só algumas linhas rotuladas (índices 1, 3, 5).
const WEEKDAYS = ['', 'Seg', '', 'Qua', '', 'Sex', '']

const config = useRuntimeConfig()

// Calendário de frequência do aluno logado (ano corrente).
const { data, status } = await useAsyncData<GetStudentAttendanceCalendarOut>(
  'student-attendance-calendar',
  () => $fetch<GetStudentAttendanceCalendarOut>(
    `${config.public.backendUrl}/students/attendances/calendar`,
    { credentials: 'include' }
  ),
  { server: false }
)

// Normaliza os itens da API pra grid: reduz a data pra 'YYYY-MM-DD'.
const days = computed<AttendanceDay[]>(() =>
  (data.value?.items ?? []).map(item => ({ date: item.date.slice(0, 10), status: item.status }))
)

function toIso(d: Date): string {
  const y = d.getFullYear()
  const m = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${y}-${m}-${day}`
}

// Monta as colunas (semanas) x linhas (dias da semana) pra um conjunto de dias,
// igual ao mapa de commits do GitHub, junto dos rótulos de mês de cada semana.
function buildGrid(items: AttendanceDay[]): { weeks: Cell[][], months: string[] } {
  if (items.length === 0) return { weeks: [], months: [] }

  const byDate = new Map<string, StudentDayAttendanceStatus>()
  for (const d of items) byDate.set(d.date, d.status)

  const sorted = [...byDate.keys()].sort()
  const first = new Date(`${sorted[0]}T00:00:00`)
  const last = new Date(`${sorted[sorted.length - 1]}T00:00:00`)

  // Alinha o início ao domingo da primeira semana e o fim ao sábado da última.
  const start = new Date(first)
  start.setDate(first.getDate() - first.getDay())
  const end = new Date(last)
  end.setDate(last.getDate() + (6 - last.getDay()))

  const weeks: Cell[][] = []
  let col: Cell[] = []
  for (const d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
    const iso = toIso(d)
    const hit = byDate.has(iso)
    col.push({ date: hit ? iso : null, status: hit ? byDate.get(iso)! : null })
    if (d.getDay() === 6) {
      weeks.push(col)
      col = []
    }
  }
  if (col.length > 0) weeks.push(col)

  // Rótulo de mês na primeira semana em que o mês aparece.
  const months = weeks.map((week, i) => {
    const firstReal = week.find(c => c.date)
    if (!firstReal) return ''
    const month = new Date(`${firstReal.date}T00:00:00`).getMonth()
    const prev = i > 0 ? weeks[i - 1]?.find(c => c.date) : undefined
    const prevMonth = prev ? new Date(`${prev.date}T00:00:00`).getMonth() : -1
    return month !== prevMonth ? MONTHS[month] : ''
  })

  return { weeks, months }
}

// Agrupa os dias por mês-calendário ('YYYY-MM'), em ordem cronológica.
const monthGroups = computed<AttendanceDay[][]>(() => {
  const map = new Map<string, AttendanceDay[]>()
  for (const d of days.value) {
    const key = d.date.slice(0, 7)
    if (!map.has(key)) map.set(key, [])
    map.get(key)!.push(d)
  }
  return [...map.keys()].sort().map(k => map.get(k)!)
})

// Quantas grids exibir, conforme a largura da tela:
//   >= 2xl → 1 grid  (o ano inteiro)
//   >= xl  → 2 grids (6 meses cada)
//   >= sm  → 4 grids (3 meses cada)
//   < sm   → 12 grids (1 mês cada — mobile)
const breakpoints = useBreakpoints(breakpointsTailwind)
const is2xl = breakpoints.greaterOrEqual('2xl')
const isXl = breakpoints.greaterOrEqual('xl')
const isSm = breakpoints.greaterOrEqual('sm')

const gridCount = computed(() => {
  if (is2xl.value) return 1
  if (isXl.value) return 2
  if (isSm.value) return 4
  return 12
})

// Divide os meses em `gridCount` blocos contíguos e monta uma grid contínua pra cada um.
const grids = computed<Grid[]>(() => {
  const groups = monthGroups.value
  if (groups.length === 0) return []

  const perGrid = Math.ceil(groups.length / gridCount.value)
  const result: Grid[] = []
  for (let i = 0; i < groups.length; i += perGrid) {
    const chunkDays = groups.slice(i, i + perGrid).flat()
    const { weeks, months } = buildGrid(chunkDays)
    result.push({ key: chunkDays[0]!.date, weeks, months })
  }
  return result
})

function cellClass(cellStatus: StudentDayAttendanceStatus | null): string {
  switch (cellStatus) {
    case 'NoClass': return 'bg-elevated' // sem aula — o mais apagado possível
    case 'Undefined': return 'bg-accented' // aula sem frequência definida
    case 'Present': return 'bg-success' // presença
    case 'Absent': return 'bg-error' // falta
    default: return 'bg-transparent' // padding fora do intervalo
  }
}

const STATUS_LABEL: Record<StudentDayAttendanceStatus, string> = {
  NoClass: 'Sem aula',
  Undefined: 'Aguardando registro',
  Present: 'Presença',
  Absent: 'Falta'
}

function cellTooltip(cell: Cell): string {
  if (!cell.date) return ''
  return `${cell.date} — ${STATUS_LABEL[cell.status as StudentDayAttendanceStatus]}`
}

const legend: { status: StudentDayAttendanceStatus, label: string }[] = [
  { status: 'NoClass', label: 'Sem aula' },
  { status: 'Undefined', label: 'Aguardando' },
  { status: 'Present', label: 'Presença' },
  { status: 'Absent', label: 'Falta' }
]
</script>

<template>
  <UDashboardPanel id="frequencies">
    <template #header>
      <UDashboardNavbar title="Frequência">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-16">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>

      <UPageCard v-else variant="subtle">
        <div class="flex flex-col gap-6">
          <!-- Uma ou mais grids, empilhadas, conforme a largura da tela -->
          <div
            v-for="grid in grids"
            :key="grid.key"
            class="flex items-stretch gap-2"
          >
            <!-- Coluna de rótulos dos dias da semana (eixo Y) -->
            <div class="flex shrink-0 flex-col gap-1">
              <div class="h-4" />
              <div class="flex flex-1 flex-col gap-[3px]">
                <div
                  v-for="(label, i) in WEEKDAYS"
                  :key="i"
                  class="flex flex-1 items-center text-[10px] leading-none text-muted"
                >
                  {{ label }}
                </div>
              </div>
            </div>

            <!-- Grid de semanas (eixo X) — ocupa todo o width disponível -->
            <div class="flex min-w-0 flex-1 flex-col gap-1">
              <!-- Rótulos de mês -->
              <div class="flex h-4 items-center gap-[3px]">
                <div
                  v-for="(label, i) in grid.months"
                  :key="i"
                  class="min-w-0 flex-1 whitespace-nowrap text-center text-[10px] leading-none text-muted"
                >
                  {{ label }}
                </div>
              </div>

              <!-- Quadradinhos -->
              <div class="flex gap-[3px]">
                <div
                  v-for="(week, wi) in grid.weeks"
                  :key="wi"
                  class="flex flex-1 flex-col gap-[3px]"
                >
                  <UTooltip
                    v-for="(cell, di) in week"
                    :key="di"
                    :text="cellTooltip(cell)"
                    :disabled="!cell.date"
                  >
                    <div
                      class="aspect-square w-full rounded-[2px] ring-1 ring-inset ring-default/40"
                      :class="cell.date ? cellClass(cell.status) : 'bg-transparent ring-transparent'"
                    />
                  </UTooltip>
                </div>
              </div>
            </div>
          </div>

          <!-- Legenda -->
          <div class="flex flex-wrap items-center gap-4 text-xs text-muted">
            <div
              v-for="item in legend"
              :key="item.status"
              class="flex items-center gap-1.5"
            >
              <div
                class="size-3 rounded-[2px] ring-1 ring-inset ring-default/40"
                :class="cellClass(item.status)"
              />
              <span>{{ item.label }}</span>
            </div>
          </div>
        </div>
      </UPageCard>
    </template>
  </UDashboardPanel>
</template>
