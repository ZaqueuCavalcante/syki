<script setup lang="ts">
import { NuxtLink } from '#components'
import type { AgendaDiscipline, AgendaDay } from '~/types/agenda'

const props = defineProps<{
  days: AgendaDay[]
}>()

// ── Dias da semana ────────────────────────────────────────────
const weekDays = [
  { key: 'Monday', label: 'Segunda', short: 'Seg' },
  { key: 'Tuesday', label: 'Terça', short: 'Ter' },
  { key: 'Wednesday', label: 'Quarta', short: 'Qua' },
  { key: 'Thursday', label: 'Quinta', short: 'Qui' },
  { key: 'Friday', label: 'Sexta', short: 'Sex' },
  { key: 'Saturday', label: 'Sábado', short: 'Sáb' },
]

// ── Parsing de horários ───────────────────────────────────────
// "H07_15" -> minutos desde a meia-noite (7*60 + 15 = 435)
function toMinutes(hour: string): number {
  const match = /^H(\d{2})_(\d{2})$/.exec(hour)
  if (!match) return 0
  return Number(match[1]) * 60 + Number(match[2])
}

function formatHour(hour: string): string {
  const match = /^H(\d{2})_(\d{2})$/.exec(hour)
  if (!match) return hour
  return `${match[1]}:${match[2]}`
}

// ── Paleta de cores por disciplina ────────────────────────────
const palette = [
  'bg-blue-100 dark:bg-blue-950/50 border-blue-500 text-blue-900 dark:text-blue-100',
  'bg-emerald-100 dark:bg-emerald-950/50 border-emerald-500 text-emerald-900 dark:text-emerald-100',
  'bg-violet-100 dark:bg-violet-950/50 border-violet-500 text-violet-900 dark:text-violet-100',
  'bg-amber-100 dark:bg-amber-950/50 border-amber-500 text-amber-900 dark:text-amber-100',
  'bg-rose-100 dark:bg-rose-950/50 border-rose-500 text-rose-900 dark:text-rose-100',
  'bg-cyan-100 dark:bg-cyan-950/50 border-cyan-500 text-cyan-900 dark:text-cyan-100',
  'bg-teal-100 dark:bg-teal-950/50 border-teal-500 text-teal-900 dark:text-teal-100',
  'bg-indigo-100 dark:bg-indigo-950/50 border-indigo-500 text-indigo-900 dark:text-indigo-100',
  'bg-fuchsia-100 dark:bg-fuchsia-950/50 border-fuchsia-500 text-fuchsia-900 dark:text-fuchsia-100',
  'bg-orange-100 dark:bg-orange-950/50 border-orange-500 text-orange-900 dark:text-orange-100',
  'bg-lime-100 dark:bg-lime-950/50 border-lime-500 text-lime-900 dark:text-lime-100',
  'bg-sky-100 dark:bg-sky-950/50 border-sky-500 text-sky-900 dark:text-sky-100',
  'bg-pink-100 dark:bg-pink-950/50 border-pink-500 text-pink-900 dark:text-pink-100',
]

// Cada disciplina distinta recebe uma cor própria, atribuída por ordem
// de aparição (só reaproveita cor se houver mais disciplinas que cores).
const colorByName = computed(() => {
  const map = new Map<string, string>()
  for (const day of props.days) {
    for (const item of day.disciplines) {
      if (!map.has(item.name)) map.set(item.name, palette[map.size % palette.length]!)
    }
  }
  return map
})

function colorFor(name: string): string {
  return colorByName.value.get(name) ?? palette[0]!
}

// ── Relógio reativo (atualiza o indicador de "agora" a cada minuto) ──
const now = ref(new Date())
let timer: ReturnType<typeof setInterval> | undefined
onMounted(() => {
  timer = setInterval(() => { now.value = new Date() }, 60_000)
})
onBeforeUnmount(() => {
  if (timer) clearInterval(timer)
})

const nowMinutes = computed(() => now.value.getHours() * 60 + now.value.getMinutes())

// ── Dia atual (indicador tipo Google Calendar) ────────────────
const todayKey = computed(() => {
  const keys = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
  return keys[now.value.getDay()] ?? null
})

// ── Layout do grid ────────────────────────────────────────────
const HOUR_HEIGHT = 64 // px por hora

const disciplinesByDay = computed(() => {
  const map = new Map<string, AgendaDiscipline[]>()
  for (const d of props.days) map.set(d.day, d.disciplines)
  return map
})

// Segunda a sexta sempre aparecem; sábado só quando tem aula.
const visibleDays = computed(() =>
  weekDays.filter(day =>
    day.key !== 'Saturday' || (disciplinesByDay.value.get('Saturday')?.length ?? 0) > 0,
  ),
)

const gridColumns = computed(() => `56px repeat(${visibleDays.value.length}, 1fr)`)
const dayColumns = computed(() => `repeat(${visibleDays.value.length}, 1fr)`)

const hasAny = computed(() =>
  props.days.some(d => d.disciplines.length > 0),
)

// Faixa de horas exibida (do início mais cedo ao fim mais tarde)
const range = computed(() => {
  let min = Infinity
  let max = -Infinity
  for (const d of props.days) {
    for (const item of d.disciplines) {
      min = Math.min(min, toMinutes(item.start))
      max = Math.max(max, toMinutes(item.end))
    }
  }
  if (!isFinite(min) || !isFinite(max)) {
    min = 7 * 60
    max = 12 * 60
  }
  const startHour = Math.floor(min / 60)
  const endHour = Math.ceil(max / 60)
  return { startHour, endHour }
})

const hourLabels = computed(() => {
  const labels: string[] = []
  for (let h = range.value.startHour; h <= range.value.endHour; h++) {
    labels.push(`${h.toString().padStart(2, '0')}:00`)
  }
  return labels
})

const gridHeight = computed(() =>
  (range.value.endHour - range.value.startHour) * HOUR_HEIGHT,
)

const BLOCK_GAP = 3 // px de folga vertical pra o card não colar na linha da hora

function blockStyle(item: AgendaDiscipline) {
  const startMin = toMinutes(item.start)
  const endMin = toMinutes(item.end)
  const top = ((startMin - range.value.startHour * 60) / 60) * HOUR_HEIGHT
  const height = Math.max(((endMin - startMin) / 60) * HOUR_HEIGHT, 24)
  return {
    top: `${top + BLOCK_GAP}px`,
    minHeight: `${height - BLOCK_GAP * 2}px`,
  }
}

// ── Indicador de "agora" (linha da hora atual sobre a coluna de hoje) ──
const todayIndex = computed(() => visibleDays.value.findIndex(d => d.key === todayKey.value))

const nowLine = computed(() => {
  if (todayIndex.value < 0) return null
  const startMin = range.value.startHour * 60
  const endMin = range.value.endHour * 60
  if (nowMinutes.value < startMin || nowMinutes.value > endMin) return null
  const top = ((nowMinutes.value - startMin) / 60) * HOUR_HEIGHT
  const n = visibleDays.value.length
  // Borda direita da coluna de hoje (acompanha o dia atual conforme a semana avança).
  const frac = (todayIndex.value + 1) / n
  return {
    top,
    // Posição no grid externo (eixo de 56px + grade dos dias). Ancora a borda direita
    // do relógio na linha vertical direita da coluna de hoje, com 4px de folga, pra ele
    // ficar totalmente contido dentro do dia atual (ver -translate-x-full no template).
    iconLeft: `calc(56px + (100% - 56px) * ${frac} - 8px)`,
  }
})
</script>

<template>
  <div v-if="hasAny" class="shrink-0 overflow-x-auto">
    <div class="min-w-180 pb-3">
      <!-- Cabeçalho: dias da semana -->
      <div class="grid" :style="{ gridTemplateColumns: gridColumns }">
        <div />
        <div
          v-for="day in visibleDays"
          :key="day.key"
          class="px-2 py-2 text-center"
        >
          <span
            class="inline-flex items-center rounded-full px-2.5 py-0.5 text-sm font-semibold"
            :class="day.key === todayKey ? 'bg-primary text-inverted' : 'text-highlighted'"
          >
            {{ day.label }}
          </span>
        </div>
      </div>

      <!-- Corpo: eixo de horas + grade dos dias -->
      <div class="relative grid" :style="{ gridTemplateColumns: '56px 1fr' }">
        <!-- Eixo de horas -->
        <div class="relative" :style="{ height: `${gridHeight}px` }">
          <div
            v-for="(label, i) in hourLabels"
            :key="label"
            class="absolute -translate-y-1/2 right-2 text-xs text-muted tabular-nums"
            :style="{ top: `${i * HOUR_HEIGHT}px` }"
          >
            {{ label }}
          </div>
        </div>

        <!-- Grade dos dias (moldura com quinas arredondadas) -->
        <div
          class="relative grid border border-default rounded-lg overflow-hidden"
          :style="{ gridTemplateColumns: dayColumns }"
        >
        <!-- Colunas dos dias -->
        <div
          v-for="(day, dayIdx) in visibleDays"
          :key="day.key"
          class="relative"
          :class="{ 'bg-primary/5': day.key === todayKey, 'border-l border-default': dayIdx > 0 }"
          :style="{ height: `${gridHeight}px` }"
        >
          <!-- Linhas de grade por hora -->
          <div
            v-for="i in (range.endHour - range.startHour - 1)"
            :key="i"
            class="absolute inset-x-0 border-t border-default/60"
            :style="{ top: `${i * HOUR_HEIGHT}px` }"
          />

          <!-- Blocos de aula (viram link quando a aula tem turma) -->
          <component
            :is="item.classId ? NuxtLink : 'div'"
            v-for="(item, idx) in (disciplinesByDay.get(day.key) ?? [])"
            :key="`${item.classId}-${idx}`"
            :to="item.classId ? `/classes/${item.classId}` : undefined"
            class="absolute inset-x-1 block rounded-md border-l-4 px-2 py-1 shadow-sm"
            :class="[
              colorFor(item.name),
              item.classId
                ? 'transition-shadow hover:shadow-md focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-current'
                : '',
            ]"
            :style="blockStyle(item)"
          >
            <div class="text-xs font-semibold leading-tight wrap-break-word">{{ item.name }}</div>
            <div class="text-[11px] opacity-80 tabular-nums">
              {{ formatHour(item.start) }} – {{ formatHour(item.end) }}
            </div>
          </component>
        </div>

        </div>

        <!-- Indicador de "agora": relógio marcando o horário atual, centrado sobre a
             linha vertical direita da coluna de hoje. Fica fora do container com
             overflow-hidden pra não ser cortado. -->
        <UIcon
          v-if="nowLine"
          name="i-lucide-clock"
          class="pointer-events-none absolute z-10 size-5 -translate-x-full -translate-y-1/2 text-primary"
          :style="{ top: `${nowLine.top}px`, left: nowLine.iconLeft }"
        />
      </div>
    </div>
  </div>

  <div v-else class="flex flex-col items-center justify-center py-16 text-center">
    <UIcon name="i-lucide-calendar-x" class="size-10 text-muted mb-3" />
    <p class="text-muted">Nenhuma aula na sua agenda.</p>
  </div>
</template>
