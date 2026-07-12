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
  Vacation: { label: 'Férias', cell: 'bg-info/15 text-info font-medium', dot: 'bg-info' },
  Recess: { label: 'Recesso', cell: 'bg-warning/15 text-warning font-medium', dot: 'bg-warning' },
  Holiday: { label: 'Feriado', cell: 'bg-error/15 text-error font-medium', dot: 'bg-error' },
}

const dayTypes = Object.keys(dayTypeStyles) as DayType[]

interface CalendarCell {
  day: number
  item: CalendarItem
}

interface CalendarMonth {
  name: string
  offset: number
  cells: CalendarCell[]
}

// Os itens vêm ordenados, um por dia do ano, então basta agrupá-los por mês na ordem em que chegam.
const months = computed<CalendarMonth[]>(() => {
  const items = data.value?.items ?? []

  return monthNames.map((name, index) => {
    const cells = items
      .filter(item => Number(item.date.slice(5, 7)) === index + 1)
      .map(item => ({ day: Number(item.date.slice(8, 10)), item }))

    return {
      name,
      offset: cells.length ? new Date(`${cells[0]!.item.date.slice(0, 10)}T00:00:00`).getDay() : 0,
      cells,
    }
  })
})

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

        <template #right>
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
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-16">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>

      <div v-else class="space-y-6 pb-8">
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

        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          <div
            v-for="month in months"
            :key="month.name"
            class="rounded-lg border border-default p-3"
          >
            <p class="mb-2 text-sm font-semibold">
              {{ month.name }}
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
                <button
                  type="button"
                  :class="[
                    'flex size-7 cursor-pointer items-center justify-center rounded text-xs tabular-nums',
                    dayTypeStyles[cell.item.dayType].cell,
                  ]"
                  @click="($event.currentTarget as HTMLElement).blur(); selectDay(cell.item)"
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
