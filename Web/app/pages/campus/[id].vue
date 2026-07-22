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
  { key: 'Morning', label: 'Matutino', window: '07:00 – 12:00', duration: 300 },
  { key: 'Afternoon', label: 'Vespertino', window: '12:00 – 18:00', duration: 360 },
  { key: 'Evening', label: 'Noturno', window: '18:00 – 24:00', duration: 360 },
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

// ── Totais por turno (stats) ──────────────────────────────────────────────────
const shiftTotals = computed(() =>
  shifts.map((shift) => {
    const cells = data.value.cells.filter(c => c.shift === shift.key)
    const used = cells.reduce((s, c) => s + c.usedMinutes, 0)
    const available = cells.reduce((s, c) => s + c.availableMinutes, 0)
    return {
      ...shift,
      rate: available > 0 ? round2((used / available) * 100) : 0,
    }
  }),
)

// ── Dia atual (destaque da coluna, espelho do Week.vue) ───────────────────────
const todayKey = computed(() => {
  const keys = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
  return keys[new Date().getDay()] ?? null
})

// ── Rampa sequencial de intensidade do primary (ocupação baixa ≠ ruim) ────────
function cellClass(rate: number): string {
  if (rate === 0) return 'bg-elevated text-muted'
  if (rate < 20) return 'bg-primary/10'
  if (rate < 40) return 'bg-primary/25'
  if (rate < 60) return 'bg-primary/40'
  if (rate < 80) return 'bg-primary/60 text-inverted'
  return 'bg-primary/80 text-inverted'
}

// ── Formatação de minutos → "3h 30min" ────────────────────────────────────────
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

// ── Drilldown selecionado ─────────────────────────────────────────────────────
const selected = ref<{ day: string, shift: string } | null>({ day: 'Monday', shift: 'Morning' })

function selectCell(day: string, shift: string) {
  selected.value = { day, shift }
}

const selectedCell = computed(() =>
  selected.value ? cellFor(selected.value.day, selected.value.shift) : undefined,
)

const selectedClassrooms = computed(() =>
  [...(selectedCell.value?.classrooms ?? [])].sort((a, b) => b.rate - a.rate),
)

const breadcrumb = [
  { label: 'Campi', to: '/campi', icon: 'i-lucide-map-pin' },
  { label: 'Ocupação' },
]
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
        <!-- Cabeçalho + stats -->
        <div class="flex flex-col gap-5">
          <div class="flex flex-col gap-1">
            <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
              {{ data.campus }}
            </h1>
            <p class="text-sm text-muted">
              Ocupação das salas por dia da semana e turno — medida pelo tempo agendado
              sobre o tempo disponível.
            </p>
          </div>

          <div class="grid grid-cols-2 gap-4 sm:grid-cols-5">
            <div class="rounded-xl border border-default bg-elevated/40 px-4 py-3">
              <span class="text-2xl font-bold text-primary leading-none">{{ formatRate(data.overallRate) }}</span>
              <span class="mt-1 block text-xs text-muted">Taxa geral</span>
            </div>
            <div class="rounded-xl border border-default bg-elevated/40 px-4 py-3">
              <span class="text-2xl font-bold text-highlighted leading-none">{{ data.totalClassrooms }}</span>
              <span class="mt-1 block text-xs text-muted">Salas</span>
            </div>
            <div
              v-for="shift in shiftTotals"
              :key="shift.key"
              class="rounded-xl border border-default bg-elevated/40 px-4 py-3"
            >
              <span class="text-2xl font-bold text-highlighted leading-none">{{ formatRate(shift.rate) }}</span>
              <span class="mt-1 block text-xs text-muted">{{ shift.label }}</span>
            </div>
          </div>
        </div>

        <!-- Heatmap -->
        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Mapa de ocupação
          </h2>

          <div class="overflow-x-auto">
            <div class="grid min-w-140 grid-cols-[auto_repeat(6,1fr)] gap-1.5">
              <!-- Header: dias da semana -->
              <div />
              <div
                v-for="day in weekDays"
                :key="day.key"
                class="px-1 pb-1 text-center"
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
                <div class="flex flex-col justify-center pr-2 text-right">
                  <span class="text-sm font-medium text-highlighted">{{ shift.label }}</span>
                  <span class="text-[11px] text-muted tabular-nums">{{ shift.window }}</span>
                </div>

                <button
                  v-for="day in weekDays"
                  :key="`${day.key}-${shift.key}`"
                  type="button"
                  class="flex h-16 flex-col items-center justify-center rounded-lg text-sm font-semibold transition-all focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary"
                  :class="[
                    cellClass(cellFor(day.key, shift.key)?.rate ?? 0),
                    selected?.day === day.key && selected?.shift === shift.key ? 'ring-2 ring-primary' : '',
                  ]"
                  @click="() => { selectCell(day.key, shift.key) }"
                >
                  <span class="tabular-nums">{{ formatRate(cellFor(day.key, shift.key)?.rate ?? 0) }}</span>
                </button>
              </template>
            </div>
          </div>
        </section>

        <!-- Drilldown inline (micro) -->
        <section v-if="selectedCell" class="flex flex-col gap-3">
          <div class="flex items-baseline gap-2">
            <h2 class="font-semibold text-highlighted">
              {{ dayLabels[selectedCell.day] }} · {{ shiftLabels[selectedCell.shift] }}
            </h2>
            <span class="text-sm text-muted">
              {{ formatMinutes(selectedCell.usedMinutes) }} de {{ formatMinutes(selectedCell.availableMinutes) }}
            </span>
          </div>

          <div class="flex flex-col divide-y divide-default rounded-xl border border-default overflow-hidden">
            <div
              v-for="room in selectedClassrooms"
              :key="room.id"
              class="flex items-center gap-4 px-4 py-3"
            >
              <span class="w-32 shrink-0 truncate text-sm font-medium text-highlighted">{{ room.name }}</span>
              <span class="w-20 shrink-0 text-xs text-muted tabular-nums">{{ formatMinutes(room.usedMinutes) }}</span>
              <div class="h-1.5 flex-1 overflow-hidden rounded-full bg-elevated">
                <div
                  class="h-full rounded-full bg-primary transition-all"
                  :style="{ width: `${room.rate}%` }"
                />
              </div>
              <UBadge
                :label="formatRate(room.rate)"
                :color="room.rate > 0 ? 'primary' : 'neutral'"
                variant="subtle"
                class="w-12 shrink-0 justify-center tabular-nums"
              />
            </div>
          </div>
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
