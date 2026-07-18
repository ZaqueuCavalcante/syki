<script setup lang="ts">
import type { ClassStatusTransition, GetClassOut } from '~/types/classes'

const props = defineProps<{ classId: string }>()

const toast = useToast()
const { can } = usePolicy()
const config = useRuntimeConfig()

const { data, status, error, refresh } = await useFetch<GetClassOut>(
  `${config.public.backendUrl}/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

const assignStudentModalOpen = ref(false)
const teachersModalOpen = ref(false)
const schedulesModalOpen = ref(false)

const canStart = can('StartClass')
const canRelease = can('ReleaseClassForEnrollment')
const canAssignTeachers = can('AssignTeachersToClass')
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

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Disciplina', value: data.value.discipline },
    { label: 'Status', status: true },
    { label: 'Período', value: data.value.period },
    { label: 'Vagas', value: `${data.value.students.length} / ${data.value.vacancies}` },
    { label: 'Carga horária', value: `${data.value.workload}h` },
  ]
})
</script>

<template>
  <UDashboardPanel id="class-details">
    <template #header>
      <UDashboardNavbar :title="data?.discipline ?? 'Turma'">
        <template #leading>
          <UButton
            icon="i-lucide-arrow-left"
            color="neutral"
            variant="ghost"
            to="/classes"
          />
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

      <div v-else class="flex flex-col gap-6 py-4">
        <UPageCard :ui="{ wrapper: 'w-full', body: 'w-full' }">
          <template #title>
            <div class="flex w-full items-center justify-between gap-2">
              <span>Ciclo de vida</span>
              <div v-if="showReleaseButton || showStartButton" class="flex gap-2">
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
          </template>
          <ClassesStatusTimeline :status="data.status" />
        </UPageCard>

        <UPageCard title="Dados da turma">
          <dl class="grid grid-cols-1 gap-x-8 gap-y-4 sm:grid-cols-2">
            <div v-for="item in details" :key="item.label" class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                {{ item.label }}
              </dt>
              <dd v-if="item.status">
                <UBadge
                  :label="classStatusLabels[data.status] ?? data.status"
                  :color="classStatusColors[data.status] ?? 'neutral'"
                  variant="subtle"
                />
              </dd>
              <dd v-else class="text-sm text-highlighted">
                {{ item.value }}
              </dd>
            </div>
          </dl>
        </UPageCard>

        <UPageCard :ui="{ wrapper: 'w-full', body: 'w-full' }">
          <template #title>
            <div class="flex w-full items-center justify-between gap-2">
              <span>Professores ({{ data.teachers.length }})</span>
              <UButton
                v-if="canAssignTeachers"
                icon="i-lucide-user-pen"
                label="Definir"
                size="sm"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); teachersModalOpen = true }"
              />
            </div>
          </template>

          <div v-if="data.teachers.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="teacher in data.teachers"
              :key="teacher.id"
              class="flex items-center py-3"
            >
              <span class="text-sm text-highlighted">{{ teacher.name }}</span>
            </div>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-user-x" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhum professor definido
            </p>
          </div>
        </UPageCard>

        <UPageCard :ui="{ wrapper: 'w-full', body: 'w-full' }">
          <template #title>
            <div class="flex w-full items-center justify-between gap-2">
              <span>Horários</span>
              <UButton
                v-if="showEditSchedules"
                icon="i-lucide-clock"
                label="Editar"
                size="sm"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); schedulesModalOpen = true }"
              />
            </div>
          </template>

          <div v-if="data.schedules.length" class="flex flex-wrap gap-2">
            <UBadge
              v-for="(s, i) in data.schedules"
              :key="i"
              :label="formatClassSchedule(s)"
              color="neutral"
              variant="subtle"
              icon="i-lucide-clock"
            />
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-clock" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhum horário cadastrado
            </p>
          </div>
        </UPageCard>

        <UPageCard :ui="{ wrapper: 'w-full', body: 'w-full' }">
          <template #title>
            <div class="flex w-full items-center justify-between gap-2">
              <span>Alunos matriculados ({{ data.students.length }})</span>
              <UButton
                icon="i-lucide-plus"
                label="Aluno"
                size="sm"
                @click="() => { assignStudentModalOpen = true }"
              />
            </div>
          </template>

          <div v-if="data.students.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="student in data.students"
              :key="student.id"
              class="flex items-center justify-between py-3"
            >
              <span class="text-sm text-highlighted">{{ student.name }}</span>
              <UBadge
                :label="studentClassStatusLabels[student.status] ?? student.status"
                :color="studentClassStatusColors[student.status] ?? 'neutral'"
                variant="subtle"
              />
            </div>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-users" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhum aluno matriculado
            </p>
          </div>
        </UPageCard>

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
