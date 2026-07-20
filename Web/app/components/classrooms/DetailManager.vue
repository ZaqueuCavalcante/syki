<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import type { ClassroomScheduleItem, GetClassroomOut } from '~/types/classrooms'

const UBadge = resolveComponent('UBadge')
const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const props = defineProps<{ classroomId: string }>()

const breadcrumb = [
  { label: 'Salas', to: '/classrooms', icon: 'i-lucide-school' },
  { label: 'Detalhes da sala' },
]

const config = useRuntimeConfig()

const { data, status, error } = await useFetch<GetClassroomOut>(
  `${config.public.backendUrl}/classrooms/${props.classroomId}`,
  { credentials: 'include', server: false },
)

const capacity = computed(() => data.value?.capacity ?? 0)
const peakStudents = computed(() => data.value?.peakStudents ?? 0)
const weeklyHours = computed(() => data.value?.weeklyHours ?? 0)
const classesCount = computed(() => data.value?.classesCount ?? 0)

const occupancyPercent = computed(() =>
  capacity.value > 0 ? Math.round((peakStudents.value / capacity.value) * 100) : 0,
)
const occupancyRingClass = computed(() => {
  if (occupancyPercent.value > 100) return 'text-error'
  if (occupancyPercent.value > 85) return 'text-warning'
  return 'text-success'
})

// Uso semanal medido contra uma semana útil nominal de 40h.
const WEEKLY_HOURS_REFERENCE = 40
const usagePercent = computed(() => Math.round((weeklyHours.value / WEEKLY_HOURS_REFERENCE) * 100))
const usageLabel = computed(() => `${weeklyHours.value.toFixed(1).replace('.', ',')}h`)
const usageRingClass = computed(() => (usagePercent.value > 100 ? 'text-warning' : 'text-primary'))

const classesRingClass = computed(() => (classesCount.value > 0 ? 'text-info' : 'text-muted'))

// Uma turma pode ocupar a sala em vários horários — a tabela lista turmas, não horários.
const allocatedClasses = computed(() => {
  const byClass = new Map<number, ClassroomScheduleItem>()
  for (const schedule of data.value?.schedules ?? []) {
    if (!byClass.has(schedule.classId)) byClass.set(schedule.classId, schedule)
  }
  return [...byClass.values()]
})

const classColumns: TableColumn<ClassroomScheduleItem>[] = [
  {
    accessorKey: 'discipline',
    header: 'Disciplina',
    cell: ({ row }) => h('span', { class: 'font-medium text-highlighted' }, row.original.discipline || 'Turma'),
  },
  {
    accessorKey: 'period',
    header: 'Período',
    cell: ({ row }) => row.original.period || '—',
  },
  {
    accessorKey: 'teachers',
    header: 'Professores',
    cell: ({ row }) => {
      const teachers = row.original.teachers
      if (!teachers.length) return h('span', { class: 'text-muted' }, '—')
      return teachers.join(', ')
    },
  },
  {
    accessorKey: 'students',
    header: 'Alunos',
    cell: ({ row }) => {
      const students = row.original.students
      const color = capacity.value > 0 && students > capacity.value ? 'text-error' : 'text-highlighted'
      return h('span', { class: `font-medium ${color}` }, `${students} / ${capacity.value}`)
    },
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h(UBadge, {
      label: classStatusLabels[row.original.status] ?? row.original.status,
      color: classStatusColors[row.original.status] ?? 'neutral',
      variant: 'subtle',
    }),
  },
  {
    id: 'actions',
    header: '',
    cell: ({ row }) => h('div', { class: 'flex justify-end' }, h(UTooltip, { text: 'Ver turma' }, () => h(UButton, {
      icon: 'i-lucide-arrow-right',
      color: 'neutral',
      variant: 'ghost',
      to: `/classes/${row.original.classId}`,
      'aria-label': 'Ver turma',
    }))),
  },
]
</script>

<template>
  <UDashboardPanel id="classroom-details">
    <template #header>
      <UDashboardNavbar>
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
          Sala não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/classrooms" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="flex flex-col gap-5">
          <div class="flex flex-col gap-1">
            <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
              {{ data.name }}
            </h1>
            <div class="flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted">
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-map-pin" class="size-4" />
                {{ data.campus || '—' }}
              </span>
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-users" class="size-4" />
                {{ data.capacity.toLocaleString('pt-BR') }} lugares
              </span>
            </div>
          </div>

          <div class="grid grid-cols-1 gap-4 sm:grid-cols-3">
            <ClassesRingStat
              :percent="occupancyPercent"
              :center-text="`${occupancyPercent}%`"
              :title="`${peakStudents} / ${capacity} lugares`"
              subtitle="turma mais cheia"
              :color-class="occupancyRingClass"
            />
            <ClassesRingStat
              :percent="usagePercent"
              :center-text="usageLabel"
              title="Uso semanal"
              :subtitle="`de ${WEEKLY_HOURS_REFERENCE}h úteis`"
              :color-class="usageRingClass"
            />
            <ClassesRingStat
              :percent="classesCount > 0 ? 100 : 0"
              :center-text="String(classesCount)"
              :title="classesCount === 1 ? 'Turma alocada' : 'Turmas alocadas'"
              subtitle="nesta sala"
              :color-class="classesRingClass"
            />
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Agenda
          </h2>

          <div v-if="data.schedules.length" class="flex flex-wrap gap-2">
            <NuxtLink
              v-for="(s, i) in data.schedules"
              :key="i"
              :to="`/classes/${s.classId}`"
              class="flex flex-col gap-0.5 rounded-lg border border-default bg-elevated/40 px-3 py-2 transition-colors hover:bg-elevated"
            >
              <span class="text-sm font-medium text-highlighted">
                {{ s.discipline || 'Turma' }}
              </span>
              <span class="flex items-center gap-1 text-xs text-muted">
                <UIcon name="i-lucide-clock" class="size-3.5" />
                {{ formatClassSchedule(s) }}
              </span>
            </NuxtLink>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-calendar-x" class="size-4" />
            Nenhuma turma alocada nesta sala
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Turmas
          </h2>

          <DataTable :data="allocatedClasses" :columns="classColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-door-open" class="size-4" />
                Nenhuma turma alocada nesta sala
              </div>
            </template>
          </DataTable>
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
