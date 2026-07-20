<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import type { InstitutionConfig } from '~/types/configs'
import type { ClassStatusTransition, ClassStudentItem, GetClassOut } from '~/types/classes'

const UAvatar = resolveComponent('UAvatar')
const UBadge = resolveComponent('UBadge')

const props = defineProps<{ classId: string }>()

const breadcrumb = [
  { label: 'Turmas', to: '/classes', icon: 'i-lucide-door-open' },
  { label: 'Detalhes da turma' },
]

const toast = useToast()
const { can } = usePolicy()
const config = useRuntimeConfig()

const { data, status, error, refresh } = await useFetch<GetClassOut>(
  `${config.public.backendUrl}/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

const { data: institutionConfig } = await useFetch<InstitutionConfig>(
  `${config.public.backendUrl}/institutions/config`,
  { credentials: 'include', server: false },
)

const assignStudentModalOpen = ref(false)
const teachersModalOpen = ref(false)
const schedulesModalOpen = ref(false)

const canStart = can('StartClass')
const canRelease = can('ReleaseClassForEnrollment')
const canUpdateTeachers = can('UpdateClassTeachers')
const canUpdateSchedules = can('UpdateClassSchedules')

const showEditSchedules = computed(() =>
  canUpdateSchedules.value && data.value?.status !== 'Started' && data.value?.status !== 'Finalized',
)

const actionLoading = ref(false)
const changeStatusModalOpen = ref(false)
const pendingTransition = ref<ClassStatusTransition | null>(null)

const showReleaseButton = computed(() => canRelease.value && data.value?.status === 'OnPreEnrollment')
const showStartButton = computed(() =>
  canStart.value && (data.value?.status === 'OnEnrollment' || data.value?.status === 'AwaitingStart'),
)

const releaseTransition = computed<ClassStatusTransition>(() => ({
  path: 'release-for-enrollment',
  title: 'Liberar turma para matrícula',
  actionLabel: 'Liberar para matrícula',
  actionIcon: 'i-lucide-door-open',
  successTitle: 'Turma liberada para matrícula',
  errorTitle: 'Erro ao liberar a turma para matrícula.',
  fromStatus: data.value?.status ?? 'OnPreEnrollment',
  toStatus: 'OnEnrollment',
  implications: [
    { icon: 'i-lucide-clipboard-list', class: 'text-info', text: 'A turma fica aberta para matrícula de alunos, dentro do período de matrícula.' },
    { icon: 'i-lucide-info', class: 'text-muted', text: 'Ainda é possível ajustar professores e horários enquanto as matrículas acontecem.' },
  ],
}))

const startTransition = computed<ClassStatusTransition>(() => ({
  path: 'start',
  title: 'Iniciar turma',
  actionLabel: 'Iniciar turma',
  actionIcon: 'i-lucide-play',
  successTitle: 'Turma iniciada com sucesso',
  errorTitle: 'Erro ao iniciar a turma.',
  fromStatus: data.value?.status ?? 'OnEnrollment',
  toStatus: 'Started',
  implications: [
    { icon: 'i-lucide-circle-play', class: 'text-primary', text: 'A turma entra em andamento e passa a acontecer normalmente.' },
    { icon: 'i-lucide-triangle-alert', class: 'text-warning', text: 'Esta ação não pode ser desfeita — não é possível retroceder o status a partir daqui.' },
  ],
}))

function askChangeStatus(transition: ClassStatusTransition) {
  pendingTransition.value = transition
  changeStatusModalOpen.value = true
}

async function confirmChangeStatus() {
  const transition = pendingTransition.value
  if (!transition) return

  actionLoading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classes/${props.classId}/${transition.path}`, {
      method: 'PUT',
      credentials: 'include',
    })
    toast.add({ title: transition.successTitle, color: 'success' })
    changeStatusModalOpen.value = false
    await refresh()
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? transition.errorTitle
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    actionLoading.value = false
  }
}

const enrolled = computed(() => data.value?.students.length ?? 0)
const vacancies = computed(() => data.value?.vacancies ?? 0)
const occupancyPercent = computed(() =>
  vacancies.value > 0 ? Math.round((enrolled.value / vacancies.value) * 100) : 0,
)
const availableVacancies = computed(() => Math.max(vacancies.value - enrolled.value, 0))

const occupancyRingClass = computed(() => {
  if (occupancyPercent.value < 50) return 'text-error'
  if (occupancyPercent.value <= 70) return 'text-warning'
  return 'text-success'
})

// Mock: ainda não há endpoint para nota média e frequência média da turma.
const averageGrade = ref(7.8)
const averageAttendance = ref(87)

const noteLimit = computed(() => institutionConfig.value?.noteLimit ?? 7)
const frequencyLimit = computed(() => institutionConfig.value?.frequencyLimit ?? 70)

const gradePercent = computed(() => Math.round((averageGrade.value / 10) * 100))
const gradeLabel = computed(() => averageGrade.value.toFixed(1).replace('.', ','))
const gradeRingClass = computed(() =>
  averageGrade.value < noteLimit.value ? 'text-error' : 'text-success',
)

const attendanceRingClass = computed(() =>
  averageAttendance.value < frequencyLimit.value ? 'text-error' : 'text-success',
)

// Colunas estáveis (criadas uma vez). As cores de Nota/Frequência leem os
// limites reativos dentro da própria célula — que roda no render do UTable —
// então recolorem quando o config da instituição chega, sem recriar o array.
const studentColumns: TableColumn<ClassStudentItem>[] = [
  {
    accessorKey: 'name',
    header: 'Aluno',
    cell: ({ row }) => h('div', { class: 'flex items-center gap-2.5' }, [
      h(UAvatar, { alt: row.original.name, size: '2xs' }),
      h('span', { class: 'font-medium text-highlighted' }, row.original.name),
    ]),
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
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h(UBadge, {
      label: studentClassStatusLabels[row.original.status] ?? row.original.status,
      color: studentClassStatusColors[row.original.status] ?? 'neutral',
      variant: 'subtle',
    }),
  },
]
</script>

<template>
  <UDashboardPanel id="class-details">
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
          Turma não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/classes" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="flex flex-col gap-5">
          <div class="grid grid-cols-1 items-start gap-x-4 gap-y-1 sm:grid-cols-[1fr_auto]">
            <h1 class="order-1 text-2xl font-semibold tracking-tight text-highlighted sm:col-start-1 sm:row-start-1">
              {{ data.discipline }}
            </h1>
            <div class="order-2 flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted sm:col-start-1 sm:row-start-2">
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-calendar" class="size-4" />
                {{ data.period }}
              </span>
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-clock" class="size-4" />
                {{ data.workload }}h
              </span>
            </div>
            <div
              v-if="showReleaseButton || showStartButton"
              class="order-3 flex flex-wrap gap-2 sm:col-start-2 sm:row-start-1 sm:justify-self-end"
            >
              <UButton
                v-if="showReleaseButton"
                icon="i-lucide-door-open"
                label="Liberar para matrícula"
                size="sm"
                @click="() => { askChangeStatus(releaseTransition) }"
              />
              <UButton
                v-if="showStartButton"
                icon="i-lucide-play"
                label="Iniciar turma"
                size="sm"
                @click="() => { askChangeStatus(startTransition) }"
              />
            </div>
          </div>

          <ClassesStatusTimeline :status="data.status" />
        </div>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-1.5">
            <h2 class="font-semibold text-highlighted">
              Professores
            </h2>
            <UTooltip v-if="canUpdateTeachers" text="Editar">
              <UButton
                icon="i-lucide-pencil"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); teachersModalOpen = true }"
              />
            </UTooltip>
          </div>

          <div v-if="data.teachers.length" class="flex flex-wrap gap-2">
            <div
              v-for="teacher in data.teachers"
              :key="teacher.id"
              class="flex items-center gap-2 rounded-full border border-default bg-elevated/40 py-1 pl-1 pr-3"
            >
              <UAvatar :alt="teacher.name" size="2xs" />
              <span class="text-sm text-highlighted">{{ teacher.name }}</span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-user-x" class="size-4" />
            Nenhum professor definido
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-1.5">
            <h2 class="font-semibold text-highlighted">
              Horários
            </h2>
            <UTooltip v-if="showEditSchedules" text="Editar">
              <UButton
                icon="i-lucide-pencil"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); schedulesModalOpen = true }"
              />
            </UTooltip>
          </div>

          <div v-if="data.schedules.length" class="flex flex-wrap gap-2">
            <div
              v-for="(s, i) in data.schedules"
              :key="i"
              class="flex flex-col gap-0.5 rounded-lg border border-default bg-elevated/40 px-3 py-2"
            >
              <span
                class="text-sm font-medium"
                :class="s.teacher ? 'text-highlighted' : 'text-muted'"
              >
                {{ s.teacher ?? 'Sem professor' }}
              </span>
              <span class="flex items-center gap-1 text-xs text-muted">
                <UIcon name="i-lucide-clock" class="size-3.5" />
                {{ formatClassSchedule(s) }}
              </span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-clock" class="size-4" />
            Nenhum horário cadastrado
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-1.5">
            <h2 class="font-semibold text-highlighted">
              Alunos
            </h2>
            <UTooltip text="Matricular aluno">
              <UButton
                icon="i-lucide-plus"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); assignStudentModalOpen = true }"
              />
            </UTooltip>
          </div>

          <div class="mb-2 grid grid-cols-1 gap-4 sm:grid-cols-3">
            <ClassesRingStat
              :percent="occupancyPercent"
              :center-text="`${occupancyPercent}%`"
              :title="`${enrolled} / ${vacancies} vagas`"
              :subtitle="`${availableVacancies} disponíveis`"
              :color-class="occupancyRingClass"
            />
            <ClassesRingStat
              :percent="gradePercent"
              :center-text="gradeLabel"
              title="Nota média"
              subtitle="da turma"
              :color-class="gradeRingClass"
            />
            <ClassesRingStat
              :percent="averageAttendance"
              :center-text="`${averageAttendance}%`"
              title="Frequência média"
              subtitle="da turma"
              :color-class="attendanceRingClass"
            />
          </div>

          <DataTable :data="data.students" :columns="studentColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-users" class="size-4" />
                Nenhum aluno matriculado
              </div>
            </template>
          </DataTable>
        </section>

        <ClassesAssignStudentModal
          v-model:open="assignStudentModalOpen"
          :class-id="data.id"
          :enrolled-ids="data.students.map(s => s.id)"
          @assigned="refresh()"
        />

        <ClassesTeachersModal
          v-model:open="teachersModalOpen"
          :class-id="data.id"
          :discipline-id="data.disciplineId"
          :teachers="data.teachers"
          @assigned="refresh()"
        />

        <ClassesSchedulesModal
          v-model:open="schedulesModalOpen"
          :class-id="data.id"
          :schedules="data.schedules"
          :teachers="data.teachers"
          @saved="refresh()"
        />

        <ClassesChangeStatusModal
          v-model:open="changeStatusModalOpen"
          :transition="pendingTransition"
          :loading="actionLoading"
          @confirm="confirmChangeStatus()"
        />
      </div>
    </template>
  </UDashboardPanel>
</template>
