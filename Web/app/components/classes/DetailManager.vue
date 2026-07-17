<script setup lang="ts">
import type { GetClassOut } from '~/types/classes'

const props = defineProps<{ classId: string }>()

const toast = useToast()
const { can } = usePolicy()
const config = useRuntimeConfig()

const { data, status, error, refresh } = await useFetch<GetClassOut>(
  `${config.public.backendUrl}/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

const assignStudentModalOpen = ref(false)

const canStart = can('StartClass')
const canRelease = can('ReleaseClassForEnrollment')

const actionLoading = ref(false)

const showReleaseButton = computed(() => canRelease.value && data.value?.status === 'OnPreEnrollment')
const showStartButton = computed(() =>
  canStart.value && (data.value?.status === 'OnEnrollment' || data.value?.status === 'AwaitingStart'),
)

async function runAction(path: string, successTitle: string, errorTitle: string) {
  actionLoading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classes/${props.classId}/${path}`, {
      method: 'PUT',
      credentials: 'include',
    })
    toast.add({ title: successTitle, color: 'success' })
    await refresh()
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? errorTitle
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    actionLoading.value = false
  }
}

async function release() {
  await runAction('release-for-enrollment', 'Turma liberada para matrícula', 'Erro ao liberar a turma para matrícula.')
}

async function start() {
  await runAction('start', 'Turma iniciada com sucesso', 'Erro ao iniciar a turma.')
}

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Disciplina', value: data.value.discipline },
    { label: 'Status', status: true },
    { label: 'Professores', value: data.value.teachers.join(', ') || '—' },
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
        <div v-if="showReleaseButton || showStartButton" class="flex justify-end gap-2">
          <UButton
            v-if="showReleaseButton"
            icon="i-lucide-door-open"
            label="Liberar para matrícula"
            :loading="actionLoading"
            @click="() => { release() }"
          />
          <UButton
            v-if="showStartButton"
            icon="i-lucide-play"
            label="Iniciar turma"
            :loading="actionLoading"
            @click="() => { start() }"
          />
        </div>

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

        <UPageCard title="Horários">
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
          <p v-else class="text-sm text-muted">
            Nenhum horário cadastrado
          </p>
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
      </div>
    </template>
  </UDashboardPanel>
</template>
