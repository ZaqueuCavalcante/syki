<script setup lang="ts">
import type { CalendarItem, DayType, GetInstitutionCalendarOut } from '~/types/calendar'

const config = useRuntimeConfig()

const year = ref(new Date().getFullYear())

const { data, status, refresh } = await useFetch<GetInstitutionCalendarOut>(`${config.public.backendUrl}/calendar/institution`, {
  credentials: 'include',
  server: false,
  query: { year },
})

const dayModalOpen = ref(false)
const selectedDay = ref<CalendarItem | null>(null)

function selectDay(item: CalendarItem) {
  selectedDay.value = item
  dayModalOpen.value = true
}

const monthNames = [
  'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
  'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro',
]

const weekDayNames = ['D', 'S', 'T', 'Q', 'Q', 'S', 'S']

const dayTypeStyles: Record<DayType, { label: string, cell: string, dot: string }> = {
  Default: { label: 'Dia letivo', cell: 'text-default hover:bg-elevated', dot: 'bg-elevated' },
  Weekend: { label: 'Fim de semana', cell: 'text-dimmed', dot: 'bg-muted' },
  Vacation: { label: 'Férias', cell: 'bg-info/15 text-info font-medium', dot: 'bg-info' },
  Recess: { label: 'Recesso', cell: 'bg-warning/15 text-warning font-medium', dot: 'bg-warning' },
  Holiday: { label: 'Feriado', cell: 'bg-error/15 text-error font-medium', dot: 'bg-error' },
}

const dayTypes = Object.keys(dayTypeStyles) as DayType[]

interface CalendarCell {
  day: number
  weekDay: number
  item: CalendarItem
}

interface CalendarMonth {
  name: string
  index: number
  offset: number
  cells: CalendarCell[]
}

function weekDayOf(date: string) {
  return new Date(`${date.slice(0, 10)}T00:00:00`).getDay()
}

const todayIso = new Date().toLocaleDateString('sv-SE') // yyyy-MM-dd no fuso local

function isToday(item: CalendarItem) {
  return item.date.slice(0, 10) === todayIso
}

// Sábado e domingo não são editáveis: não faz sentido mudar o tipo de um dia de fim de semana.
function isWeekend(cell: CalendarCell) {
  return cell.weekDay === 0 || cell.weekDay === 6
}

// Os itens vêm ordenados, um por dia do ano, então basta agrupá-los por mês na ordem em que chegam.
const months = computed<CalendarMonth[]>(() => {
  const items = data.value?.items ?? []

  return monthNames.map((name, index) => {
    const cells = items
      .filter(item => Number(item.date.slice(5, 7)) === index + 1)
      .map(item => ({ day: Number(item.date.slice(8, 10)), weekDay: weekDayOf(item.date), item }))

    return {
      name,
      index,
      offset: cells.length ? cells[0]!.weekDay : 0,
      cells,
    }
  })
})

const now = new Date()

// Só destaca o mês atual quando o ano exibido é o ano corrente.
function isCurrentMonth(month: CalendarMonth) {
  return year.value === now.getFullYear() && month.index === now.getMonth()
}

const totals = computed(() => {
  const items = data.value?.items ?? []
  return dayTypes.map(type => ({
    type,
    total: items.filter(item => item.dayType === type).length,
  }))
})

function tooltip(item: CalendarItem) {
  const label = dayTypeStyles[item.dayType].label
  return item.description ? `${label} — ${item.description}` : label
}
</script>

<template>
  <UDashboardPanel id="calendar">
    <template #header>
      <UDashboardNavbar title="Calendário">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="space-y-6 pb-8">
        <div class="flex flex-wrap items-center justify-between gap-4">
          <div class="flex flex-wrap items-center gap-x-6 gap-y-2">
            <div
              v-for="{ type, total } in totals"
              :key="type"
              class="flex items-center gap-2 text-sm text-muted"
            >
              <span :class="['size-2.5 rounded-full', dayTypeStyles[type].dot]" />
              {{ dayTypeStyles[type].label }}
              <span class="tabular-nums text-dimmed">{{ total }}</span>
            </div>
          </div>

          <div class="flex items-center gap-1">
            <UButton
              icon="i-lucide-chevron-left"
              color="neutral"
              variant="ghost"
              @click="() => { year-- }"
            />
            <span class="w-14 text-center text-sm font-medium tabular-nums">{{ year }}</span>
            <UButton
              icon="i-lucide-chevron-right"
              color="neutral"
              variant="ghost"
              @click="() => { year++ }"
            />
          </div>
        </div>

        <div v-if="status === 'pending'" class="flex justify-center py-16">
          <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
        </div>

        <div v-else class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          <div
            v-for="month in months"
            :key="month.name"
            class="rounded-lg border border-default p-3"
          >
            <p class="mb-2">
              <span
                class="inline-flex items-center rounded-full px-2.5 py-0.5 text-sm font-semibold"
                :class="isCurrentMonth(month) ? 'bg-primary text-inverted' : 'text-highlighted'"
              >
                {{ month.name }}
              </span>
            </p>

            <div class="grid grid-cols-7 gap-0.5 text-center">
              <span
                v-for="(weekDay, index) in weekDayNames"
                :key="index"
                class="py-1 text-xs text-dimmed"
              >
                {{ weekDay }}
              </span>

              <span v-for="blank in month.offset" :key="`blank-${blank}`" />

              <UTooltip
                v-for="cell in month.cells"
                :key="cell.day"
                :text="tooltip(cell.item)"
              >
                <span
                  v-if="isWeekend(cell)"
                  :class="[
                    'flex h-7 w-full items-center justify-center rounded text-xs tabular-nums',
                    dayTypeStyles[cell.item.dayType].cell,
                    isToday(cell.item) && 'ring-2 ring-inset ring-primary font-semibold',
                  ]"
                >
                  {{ cell.day }}
                </span>
                <button
                  v-else
                  type="button"
                  :class="[
                    'flex h-7 w-full cursor-pointer items-center justify-center rounded text-xs tabular-nums',
                    dayTypeStyles[cell.item.dayType].cell,
                    isToday(cell.item) && 'ring-2 ring-inset ring-primary font-semibold',
                  ]"
                  @click="(e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); selectDay(cell.item) }"
                >
                  {{ cell.day }}
                </button>
              </UTooltip>
            </div>
          </div>
        </div>
      </div>
    </template>
  </UDashboardPanel>

  <CalendarDayModal
    v-model:open="dayModalOpen"
    :day="selectedDay"
    @saved="refresh()"
  />
</template>
