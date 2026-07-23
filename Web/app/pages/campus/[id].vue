<script setup lang="ts">
// ⚠️ MOCK — esta página usa dados fabricados no lugar da API
// (GET /campi/{id}/occupancy). Serve só pra avaliar o visual do frontend.

interface OccupancyClassroom {
  id: number
  name: string
  usedMinutes: number
  rate: number
}
interface OccupancyCell {
  day: string
  shift: string
  availableMinutes: number
  usedMinutes: number
  rate: number
  classrooms: OccupancyClassroom[]
}
interface OccupancyOut {
  campusId: number
  campus: string
  totalClassrooms: number
  overallRate: number
  cells: OccupancyCell[]
}

const route = useRoute()
const campusId = Number(route.params.id)

// ── Constantes compartilhadas (serão promovidas pra utils/classes.ts) ─────────
const weekDays = [
  { key: 'Monday', label: 'Segunda', short: 'Seg' },
  { key: 'Tuesday', label: 'Terça', short: 'Ter' },
  { key: 'Wednesday', label: 'Quarta', short: 'Qua' },
  { key: 'Thursday', label: 'Quinta', short: 'Qui' },
  { key: 'Friday', label: 'Sexta', short: 'Sex' },
  { key: 'Saturday', label: 'Sábado', short: 'Sáb' },
]
const shifts = [
  { key: 'Morning', label: 'Matutino', window: '07h–12h', duration: 300 },
  { key: 'Afternoon', label: 'Vespertino', window: '12h–18h', duration: 360 },
  { key: 'Evening', label: 'Noturno', window: '18h–24h', duration: 360 },
]
const shiftLabels: Record<string, string> = {
  Morning: 'Matutino',
  Afternoon: 'Vespertino',
  Evening: 'Noturno',
}
const dayLabels: Record<string, string> = {
  Monday: 'Segunda',
  Tuesday: 'Terça',
  Wednesday: 'Quarta',
  Thursday: 'Quinta',
  Friday: 'Sexta',
  Saturday: 'Sábado',
}
const dayShort: Record<string, string> = Object.fromEntries(weekDays.map(d => [d.key, d.short]))

// ── MOCK: gerador de ocupação ─────────────────────────────────────────────────
const classrooms = [
  { id: 1, name: 'Sala 101' },
  { id: 2, name: 'Sala 102' },
  { id: 3, name: 'Lab 201' },
  { id: 4, name: 'Auditório' },
]

// Padrão de uso (0–1) por sala × dia × turno — fabricado pra dar variedade visual.
// [sala][dia 0..5][turno 0..2]
const usagePattern: number[][][] = [
  // Sala 101 — bem usada de manhã/tarde nos dias úteis
  [[0.9, 0.8, 0.3], [0.85, 0.7, 0.4], [0.9, 0.75, 0.2], [0.8, 0.65, 0.5], [0.7, 0.6, 0.3], [0.2, 0, 0]],
  // Sala 102 — foco noturno
  [[0.4, 0.5, 0.85], [0.3, 0.6, 0.9], [0.5, 0.55, 0.8], [0.35, 0.5, 0.95], [0.4, 0.45, 0.7], [0, 0, 0]],
  // Lab 201 — uso irregular
  [[0.2, 0.9, 0.6], [0.1, 0.85, 0.55], [0, 0.7, 0.4], [0.15, 0.8, 0.6], [0.05, 0.6, 0.3], [0.1, 0.2, 0]],
  // Auditório — pouco usado, alguns picos
  [[0, 0.3, 0.2], [0, 0, 0.5], [0.1, 0.2, 0], [0, 0.4, 0.3], [0, 0, 0.1], [0, 0, 0]],
]

function round2(n: number) {
  return Math.round(n * 100) / 100
}

const mock = computed<OccupancyOut>(() => {
  const cells: OccupancyCell[] = []
  let totalUsed = 0
  let totalAvailable = 0

  weekDays.forEach((day, di) => {
    shifts.forEach((shift, si) => {
      const cellClassrooms: OccupancyClassroom[] = classrooms.map((c, ci) => {
        const frac = usagePattern[ci]?.[di]?.[si] ?? 0
        const used = Math.round(frac * shift.duration)
        return {
          id: c.id,
          name: c.name,
          usedMinutes: used,
          rate: round2((used / shift.duration) * 100),
        }
      })
      const used = cellClassrooms.reduce((s, c) => s + c.usedMinutes, 0)
      const available = classrooms.length * shift.duration
      totalUsed += used
      totalAvailable += available
      cells.push({
        day: day.key,
        shift: shift.key,
        availableMinutes: available,
        usedMinutes: used,
        rate: available > 0 ? round2((used / available) * 100) : 0,
        classrooms: cellClassrooms,
      })
    })
  })

  return {
    campusId,
    campus: 'Campus Central',
    totalClassrooms: classrooms.length,
    overallRate: totalAvailable > 0 ? round2((totalUsed / totalAvailable) * 100) : 0,
    cells,
  }
})

const data = mock
const status = ref<'success'>('success')
const error = ref(null)

// ── Lookup de célula por dia+turno ────────────────────────────────────────────
function cellFor(dayKey: string, shiftKey: string): OccupancyCell | undefined {
  return data.value.cells.find(c => c.day === dayKey && c.shift === shiftKey)
}

// ── Agregado por turno (vai no eixo esquerdo do grid) ─────────────────────────
const shiftRate = computed<Record<string, number>>(() => {
  const out: Record<string, number> = {}
  for (const shift of shifts) {
    const cells = data.value.cells.filter(c => c.shift === shift.key)
    const used = cells.reduce((s, c) => s + c.usedMinutes, 0)
    const available = cells.reduce((s, c) => s + c.availableMinutes, 0)
    out[shift.key] = available > 0 ? round2((used / available) * 100) : 0
  }
  return out
})

// ── Insights derivados (stats) ────────────────────────────────────────────────
const peakCell = computed(() =>
  data.value.cells.reduce((a, b) => (b.rate > a.rate ? b : a), data.value.cells[0]!),
)
const freeSlots = computed(() => data.value.cells.filter(c => c.rate === 0).length)

// ── Dia atual (destaque da coluna, espelho do Week.vue) ───────────────────────
const todayKey = computed(() => {
  const keys = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
  return keys[new Date().getDay()] ?? null
})

// ── Formatação ────────────────────────────────────────────────────────────────
function formatMinutes(minutes: number): string {
  if (minutes <= 0) return '0min'
  const h = Math.floor(minutes / 60)
  const m = minutes % 60
  if (h === 0) return `${m}min`
  if (m === 0) return `${h}h`
  return `${h}h ${m}min`
}
function formatRate(rate: number): string {
  return `${rate.toFixed(0)}%`
}

// Rampa sequencial de intensidade do primary — ocupação baixa não é "ruim",
// então nada de vermelho/verde: só uma escala de saturação.
function cellClass(rate: number): string {
  if (rate === 0) return 'bg-elevated text-dimmed ring-1 ring-inset ring-default/60'
  if (rate < 20) return 'bg-primary/10 text-highlighted'
  if (rate < 40) return 'bg-primary/25 text-highlighted'
  if (rate < 60) return 'bg-primary/40 text-highlighted'
  if (rate < 80) return 'bg-primary/60 text-inverted'
  return 'bg-primary/85 text-inverted'
}

// ── Drilldown selecionado ─────────────────────────────────────────────────────
const selected = ref<{ day: string, shift: string }>({ day: 'Monday', shift: 'Morning' })

function selectCell(day: string, shift: string) {
  selected.value = { day, shift }
}
function isSelected(day: string, shift: string) {
  return selected.value.day === day && selected.value.shift === shift
}

const selectedCell = computed(() => cellFor(selected.value.day, selected.value.shift))
const selectedClassrooms = computed(() =>
  [...(selectedCell.value?.classrooms ?? [])].sort((a, b) => b.rate - a.rate),
)

// ── Animação de preenchimento no load (salas "enchendo") ──────────────────────
const filled = ref(false)
onMounted(() => {
  requestAnimationFrame(() => { filled.value = true })
})

const breadcrumb = [
  { label: 'Campi', to: '/campi', icon: 'i-lucide-map-pin' },
  { label: 'Ocupação' },
]

// Degraus da legenda — um por faixa da rampa do cellClass
const legendSteps = [0, 10, 30, 50, 70, 90]
</script>

<template>
  <UDashboardPanel id="campus-occupancy">
    <template #header>
      <UDashboardNavbar>
        <template #leading>
          <UButton icon="i-lucide-arrow-left" color="neutral" variant="ghost" to="/campi" aria-label="Voltar" />
        </template>
        <template #title>
          <UBreadcrumb :items="breadcrumb" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>

      <div v-else-if="error || !data" class="flex flex-col items-center gap-4 py-12">
        <UIcon name="i-lucide-triangle-alert" class="size-16 text-muted" />
        <p class="text-muted text-sm">
          Campus não encontrado
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/campi" />
      </div>

      <!-- Empty state: campus sem salas -->
      <div v-else-if="data.totalClassrooms === 0" class="flex flex-col items-center gap-4 py-16 text-center">
        <UIcon name="i-lucide-door-closed" class="size-16 text-muted" />
        <div class="flex flex-col gap-1">
          <p class="font-medium text-highlighted">
            Nenhuma sala neste campus
          </p>
          <p class="text-muted text-sm">
            Cadastre salas para acompanhar a ocupação por dia e turno.
          </p>
        </div>
        <UButton icon="i-lucide-plus" label="Salas" to="/classrooms" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <!-- Cabeçalho -->
        <div class="flex flex-col gap-1">
          <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
            {{ data.campus }}
          </h1>
          <p class="text-sm text-muted">
            Quanto do tempo de cada sala está reservado, por dia e turno.
          </p>
        </div>

        <!-- Stats: insights, não totais crus -->
        <div class="grid grid-cols-2 gap-3 lg:grid-cols-4">
          <div class="flex items-center gap-4 rounded-xl border border-primary/25 bg-primary/[0.04] px-4 py-4">
            <div class="flex flex-col leading-none">
              <span class="text-4xl font-bold tabular-nums tracking-tight text-primary">{{ formatRate(data.overallRate) }}</span>
              <span class="mt-2 text-xs font-medium text-muted">Ocupação geral</span>
            </div>
          </div>

          <div class="flex flex-col justify-center rounded-xl border border-default bg-elevated/40 px-4 py-4">
            <span class="text-2xl font-bold tabular-nums leading-none text-highlighted">{{ data.totalClassrooms }}</span>
            <span class="mt-2 text-xs text-muted">Salas no campus</span>
          </div>

          <div class="flex flex-col justify-center rounded-xl border border-default bg-elevated/40 px-4 py-4">
            <span class="text-sm font-semibold text-highlighted">
              {{ dayShort[peakCell.day] }} · {{ shiftLabels[peakCell.shift] }}
            </span>
            <span class="mt-0.5 text-xl font-bold tabular-nums leading-none text-primary">{{ formatRate(peakCell.rate) }}</span>
            <span class="mt-2 text-xs text-muted">Horário de pico</span>
          </div>

          <div class="flex flex-col justify-center rounded-xl border border-default bg-elevated/40 px-4 py-4">
            <span class="text-2xl font-bold tabular-nums leading-none text-highlighted">
              {{ freeSlots }}<span class="text-base font-medium text-muted"> / 18</span>
            </span>
            <span class="mt-2 text-xs text-muted">Turnos livres</span>
          </div>
        </div>

        <!-- Mapa de calor: dia × turno -->
        <section class="flex flex-col gap-4">
          <div class="flex flex-wrap items-center justify-between gap-3">
            <h2 class="font-semibold text-highlighted">
              Mapa de ocupação
            </h2>
            <!-- Legenda: rampa de intensidade -->
            <div class="flex items-center gap-2 text-xs text-muted">
              <span>0%</span>
              <div class="flex items-center gap-1">
                <div
                  v-for="step in legendSteps"
                  :key="step"
                  class="size-4 rounded-sm"
                  :class="cellClass(step)"
                />
              </div>
              <span>100%</span>
            </div>
          </div>

          <!-- p-1/-m-1: o ring de hover/seleção é desenhado FORA da caixa da célula,
               então o container de scroll precisa de folga pra não recortá-lo nas bordas. -->
          <div class="overflow-x-auto p-1 -m-1">
            <div class="grid min-w-160 grid-cols-[auto_repeat(6,minmax(0,1fr))] gap-2">
              <!-- Header: dias da semana -->
              <div />
              <div
                v-for="day in weekDays"
                :key="day.key"
                class="pb-1 text-center"
              >
                <span
                  class="inline-flex items-center rounded-full px-2.5 py-0.5 text-sm font-semibold"
                  :class="day.key === todayKey ? 'bg-primary text-inverted' : 'text-highlighted'"
                >
                  {{ day.short }}
                </span>
              </div>

              <!-- Linhas por turno -->
              <template v-for="shift in shifts" :key="shift.key">
                <!-- Eixo esquerdo: rótulo do turno + agregado (dupla função) -->
                <div class="flex flex-col justify-center pr-3 text-right">
                  <span class="text-sm font-medium text-highlighted">{{ shift.label }}</span>
                  <span class="text-[11px] text-muted tabular-nums">{{ shift.window }}</span>
                  <span class="mt-0.5 text-xs font-semibold tabular-nums text-primary">{{ formatRate(shiftRate[shift.key] ?? 0) }}</span>
                </div>

                <!-- Célula do mapa de calor -->
                <button
                  v-for="day in weekDays"
                  :key="`${day.key}-${shift.key}`"
                  type="button"
                  class="flex h-16 items-center justify-center rounded-lg text-sm font-semibold tabular-nums transition-shadow hover:ring-2 hover:ring-primary/50 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary"
                  :class="[
                    cellClass(cellFor(day.key, shift.key)?.rate ?? 0),
                    isSelected(day.key, shift.key) ? 'ring-2 ring-primary' : '',
                  ]"
                  @click="() => { selectCell(day.key, shift.key) }"
                >
                  {{ formatRate(cellFor(day.key, shift.key)?.rate ?? 0) }}
                </button>
              </template>
            </div>
          </div>
        </section>

        <!-- Drilldown inline (micro) -->
        <section v-if="selectedCell" class="flex flex-col gap-4 rounded-xl border border-default bg-elevated/30 p-5">
          <div class="flex items-center justify-between gap-3">
            <div class="flex flex-col gap-0.5">
              <h2 class="font-semibold text-highlighted">
                {{ dayLabels[selectedCell.day] }} · {{ shiftLabels[selectedCell.shift] }}
              </h2>
              <span class="text-sm text-muted">
                {{ formatMinutes(selectedCell.usedMinutes) }} reservados de {{ formatMinutes(selectedCell.availableMinutes) }}
              </span>
            </div>
            <span class="text-2xl font-bold tabular-nums text-primary">{{ formatRate(selectedCell.rate) }}</span>
          </div>

          <div class="flex flex-col gap-3">
            <div
              v-for="room in selectedClassrooms"
              :key="room.id"
              class="flex items-center gap-4"
            >
              <span class="w-28 shrink-0 truncate text-sm font-medium text-highlighted">{{ room.name }}</span>
              <div class="h-2 flex-1 overflow-hidden rounded-full bg-elevated ring-1 ring-inset ring-default/60">
                <div
                  class="h-full rounded-full bg-primary transition-[width] duration-500 ease-out motion-reduce:transition-none"
                  :style="{ width: filled ? `${room.rate}%` : '0%' }"
                />
              </div>
              <span class="w-16 shrink-0 text-right text-xs text-muted tabular-nums">{{ formatMinutes(room.usedMinutes) }}</span>
              <span
                class="w-11 shrink-0 text-right text-sm font-semibold tabular-nums"
                :class="room.rate > 0 ? 'text-highlighted' : 'text-dimmed'"
              >{{ formatRate(room.rate) }}</span>
            </div>
          </div>
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
