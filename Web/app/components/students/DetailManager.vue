<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import type { InstitutionConfig } from '~/types/configs'
import type { GetStudentDetailsOut, StudentClassItem } from '~/types/students'

const UBadge = resolveComponent('UBadge')
const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const props = defineProps<{ studentId: string }>()

const breadcrumb = [
  { label: 'Alunos', to: '/students', icon: 'i-lucide-graduation-cap' },
  { label: 'Detalhes do aluno' },
]

const config = useRuntimeConfig()

const { data, status, error } = await useFetch<GetStudentDetailsOut>(
  `${config.public.backendUrl}/students/${props.studentId}/details`,
  { credentials: 'include', server: false },
)

const { data: institutionConfig } = await useFetch<InstitutionConfig>(
  `${config.public.backendUrl}/institutions/config`,
  { credentials: 'include', server: false },
)

const noteLimit = computed(() => institutionConfig.value?.noteLimit ?? 7)
const frequencyLimit = computed(() => institutionConfig.value?.frequencyLimit ?? 70)

const yieldCoefficient = computed(() => data.value?.yieldCoefficient ?? 0)
const averageGrade = computed(() => data.value?.averageGrade ?? 0)
const averageAttendance = computed(() => data.value?.averageAttendance ?? 0)

const yieldPercent = computed(() => Math.round((yieldCoefficient.value / 10) * 100))
const yieldLabel = computed(() => yieldCoefficient.value.toFixed(1).replace('.', ','))
const yieldRingClass = computed(() =>
  yieldCoefficient.value < noteLimit.value ? 'text-error' : 'text-success',
)

const gradePercent = computed(() => Math.round((averageGrade.value / 10) * 100))
const gradeLabel = computed(() => averageGrade.value.toFixed(1).replace('.', ','))
const gradeRingClass = computed(() =>
  averageGrade.value < noteLimit.value ? 'text-error' : 'text-success',
)

const attendanceRingClass = computed(() =>
  averageAttendance.value < frequencyLimit.value ? 'text-error' : 'text-success',
)

const activeClasses = computed(() =>
  data.value?.classes.filter(c => c.status !== 'Finalized').length ?? 0,
)

// Colunas estáveis (criadas uma vez). As cores de Nota/Frequência leem os
// limites reativos dentro da própria célula — que roda no render do UTable —
// então recolorem quando o config da instituição chega, sem recriar o array.
const classColumns: TableColumn<StudentClassItem>[] = [
  {
    accessorKey: 'discipline',
    header: 'Disciplina',
    cell: ({ row }) => h('span', { class: 'font-medium text-highlighted' }, row.original.discipline),
  },
  {
    accessorKey: 'period',
    header: 'Período',
  },
  {
    accessorKey: 'averageGrade',
    header: 'Nota média',
    cell: ({ row }) => {
      const grade = row.original.averageGrade
      if (grade == null) return h('span', { class: 'text-muted' }, '—')
      const color = grade < noteLimit.value ? 'text-error' : 'text-success'
      return h('span', { class: `font-medium ${color}` }, grade.toFixed(1).replace('.', ','))
    },
  },
  {
    accessorKey: 'averageAttendance',
    header: 'Frequência média',
    cell: ({ row }) => {
      const attendance = row.original.averageAttendance
      if (attendance == null) return h('span', { class: 'text-muted' }, '—')
      const color = attendance < frequencyLimit.value ? 'text-error' : 'text-success'
      return h('span', { class: `font-medium ${color}` }, `${Math.round(attendance)}%`)
    },
  },
  {
    accessorKey: 'myStatus',
    header: 'Situação',
    cell: ({ row }) => h(UBadge, {
      label: studentClassStatusLabels[row.original.myStatus] ?? row.original.myStatus,
      color: studentClassStatusColors[row.original.myStatus] ?? 'neutral',
      variant: 'subtle',
    }),
  },
  {
    accessorKey: 'status',
    header: 'Turma',
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
      to: `/classes/${row.original.id}`,
      'aria-label': 'Ver turma',
    }))),
  },
]
</script>

<template>
  <UDashboardPanel id="student-details">
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
          Aluno não encontrado
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/students" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="flex flex-col gap-5">
          <div class="flex items-start gap-4">
            <UAvatar :alt="data.name" size="xl" />
            <div class="flex flex-col gap-1">
              <div class="flex flex-wrap items-center gap-2">
                <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
                  {{ data.name }}
                </h1>
                <UBadge
                  :label="studentStatusLabels[data.status] ?? data.status"
                  :color="studentStatusColors[data.status] ?? 'neutral'"
                  variant="subtle"
                />
              </div>
              <div class="flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted">
                <span class="flex items-center gap-1.5">
                  <UIcon name="i-lucide-mail" class="size-4" />
                  {{ data.email }}
                </span>
                <span class="flex items-center gap-1.5">
                  <UIcon name="i-lucide-hash" class="size-4" />
                  {{ data.enrollmentCode }}
                </span>
              </div>
            </div>
          </div>

          <div class="grid grid-cols-1 gap-4 sm:grid-cols-3">
            <ClassesRingStat
              :percent="yieldPercent"
              :center-text="yieldLabel"
              title="Coeficiente"
              subtitle="de rendimento"
              :color-class="yieldRingClass"
            />
            <ClassesRingStat
              :percent="gradePercent"
              :center-text="gradeLabel"
              title="Nota média"
              subtitle="nas turmas atuais"
              :color-class="gradeRingClass"
            />
            <ClassesRingStat
              :percent="averageAttendance"
              :center-text="`${Math.round(averageAttendance)}%`"
              title="Frequência média"
              subtitle="nas turmas atuais"
              :color-class="attendanceRingClass"
            />
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Curso
          </h2>

          <div
            v-if="data.course"
            class="flex flex-col gap-2 rounded-lg border border-default bg-elevated/40 px-4 py-3"
          >
            <span class="font-medium text-highlighted">{{ data.course.course }}</span>
            <div class="flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted">
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-map-pin" class="size-4" />
                {{ data.course.campus }}
              </span>
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-calendar" class="size-4" />
                {{ data.course.period }}
              </span>
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-sun" class="size-4" />
                {{ courseSessionLabels[data.course.session] ?? data.course.session }}
              </span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-library" class="size-4" />
            Aluno ainda não matriculado em uma oferta de curso
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-2">
            <h2 class="font-semibold text-highlighted">
              Turmas
            </h2>
            <UBadge
              v-if="data.classes.length"
              :label="`${activeClasses} em andamento`"
              color="neutral"
              variant="subtle"
            />
          </div>

          <DataTable :data="data.classes" :columns="classColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-door-open" class="size-4" />
                Aluno não está matriculado em nenhuma turma
              </div>
            </template>
          </DataTable>
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
