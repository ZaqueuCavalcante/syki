<script setup lang="ts">
import { ptBR } from 'date-fns/locale'
import { format, parseISO } from 'date-fns'
import { VisXYContainer, VisStackedBar, VisAxis, VisCrosshair, VisTooltip } from '@unovis/vue'

export interface ViewsByDay {
  day: string
  views: number
}

const props = defineProps<{
  viewsByDay: ViewsByDay[]
}>()

const cardRef = useTemplateRef<HTMLElement | null>('cardRef')
const { width } = useElementSize(cardRef)

const data = computed(() => props.viewsByDay.map(item => ({
  date: parseISO(item.day),
  views: item.views,
})))

type DataRecord = { date: Date, views: number }

const x = (_: DataRecord, i: number) => i
const y = (d: DataRecord) => d.views

const total = computed(() => data.value.reduce((acc, { views }) => acc + views, 0))

const formatDate = (date: Date) => format(date, 'd MMM', { locale: ptBR })

const xTicks = (i: number) => {
  const item = data.value[i]
  if (!item) return ''

  // com muitos dias, mostra só um rótulo a cada N pra não sobrepor
  const step = Math.ceil(data.value.length / 8)
  if (i % step !== 0 && i !== data.value.length - 1) return ''

  return formatDate(item.date)
}

const template = (d: DataRecord) =>
  `${formatDate(d.date)}: ${d.views} ${d.views === 1 ? 'visualização' : 'visualizações'}`
</script>

<template>
  <UCard ref="cardRef" :ui="{ root: 'overflow-visible', body: 'px-0! pt-0! pb-3!' }">
    <template #header>
      <div>
        <p class="text-xs text-muted uppercase mb-1.5">
          Visualizações por dia
        </p>
        <p class="text-3xl text-highlighted font-semibold">
          {{ total }}
        </p>
      </div>
    </template>

    <div v-if="!data.length" class="flex flex-col items-center justify-center gap-3 py-12 text-center">
      <UIcon name="i-lucide-chart-column" class="size-8 text-muted" />
      <p class="text-sm text-muted">
        Nenhuma visualização ainda
      </p>
    </div>

    <VisXYContainer
      v-else
      :data="data"
      :padding="{ top: 40 }"
      :margin="{ left: 24, right: 12 }"
      class="h-80"
      :width="width"
    >
      <VisStackedBar
        :x="x"
        :y="y"
        color="var(--ui-primary)"
        :rounded-corners="4"
        :bar-padding="0.2"
      />

      <VisAxis
        type="x"
        :x="x"
        :tick-format="xTicks"
      />

      <VisAxis
        type="y"
        :tick-format="(value: number) => Number.isInteger(value) ? String(value) : ''"
      />

      <VisCrosshair
        color="var(--ui-primary)"
        :template="template"
      />

      <VisTooltip />
    </VisXYContainer>
  </UCard>
</template>

<style scoped>
.unovis-xy-container {
  --vis-crosshair-line-stroke-color: var(--ui-primary);
  --vis-crosshair-circle-stroke-color: var(--ui-bg);

  --vis-axis-grid-color: var(--ui-border);
  --vis-axis-tick-color: var(--ui-border);
  --vis-axis-tick-label-color: var(--ui-text-dimmed);

  --vis-tooltip-background-color: var(--ui-bg);
  --vis-tooltip-border-color: var(--ui-border);
  --vis-tooltip-text-color: var(--ui-text-highlighted);
}
</style>
