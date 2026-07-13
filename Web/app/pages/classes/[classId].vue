<script setup lang="ts">
interface ScheduleItem {
  day: string
  startAt: string
  endAt: string
}

interface ClassStudentItem {
  id: number
  name: string
  status: string
}

interface GetClassOut {
  id: number
  discipline: string
  teacher: string
  period: string
  vacancies: number
  workload: number
  status: string
  schedules: ScheduleItem[]
  students: ClassStudentItem[]
}

const route = useRoute()
const config = useRuntimeConfig()
const classId = route.params.classId

const { data, status, error, refresh } = await useFetch<GetClassOut>(
  `${config.public.backendUrl}/classes/${classId}`,
  { credentials: 'include', server: false },
)

const assignStudentModalOpen = ref(false)

const statusLabels: Record<string, string> = {
  OnPreEnrollment: 'Pré-matrícula',
  OnEnrollment: 'Matrícula',
  AwaitingStart: 'Aguardando início',
  Started: 'Iniciada',
  Finalized: 'Finalizada',
}

const statusColors: Record<string, 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'> = {
  OnPreEnrollment: 'neutral',
  OnEnrollment: 'info',
  AwaitingStart: 'warning',
  Started: 'primary',
  Finalized: 'success',
}

const studentStatusLabels: Record<string, string> = {
  Pendente: 'Pendente',
  Matriculado: 'Matriculado',
  Aprovado: 'Aprovado',
  Dispensado: 'Dispensado',
  ReprovadoPorNota: 'Reprovado por nota',
  ReprovadoPorFalta: 'Reprovado por falta',
}

const studentStatusColors: Record<string, 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'> = {
  Pendente: 'neutral',
  Matriculado: 'info',
  Aprovado: 'success',
  Dispensado: 'warning',
  ReprovadoPorNota: 'error',
  ReprovadoPorFalta: 'error',
}

const dayLabels: Record<string, string> = {
  Sunday: 'Domingo',
  Monday: 'Segunda',
  Tuesday: 'Terça',
  Wednesday: 'Quarta',
  Thursday: 'Quinta',
  Friday: 'Sexta',
  Saturday: 'Sábado',
}

function formatHour(value: string) {
  return value.replace(/^H/, '').replace('_', ':')
}

function formatSchedule(s: ScheduleItem) {
  return `${dayLabels[s.day] ?? s.day} · ${formatHour(s.startAt)} – ${formatHour(s.endAt)}`
}

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Disciplina', value: data.value.discipline },
    { label: 'Status', status: true },
    { label: 'Professor', value: data.value.teacher || '—' },
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
        <UPageCard title="Dados da turma">
          <dl class="grid grid-cols-1 gap-x-8 gap-y-4 sm:grid-cols-2">
            <div v-for="item in details" :key="item.label" class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                {{ item.label }}
              </dt>
              <dd v-if="item.status">
                <UBadge
                  :label="statusLabels[data.status] ?? data.status"
                  :color="statusColors[data.status] ?? 'neutral'"
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
              :label="formatSchedule(s)"
              color="neutral"
              variant="subtle"
              icon="i-lucide-clock"
            />
          </div>
          <p v-else class="text-sm text-muted">
            Nenhum horário cadastrado
          </p>
        </UPageCard>

        <UPageCard :title="`Alunos matriculados (${data.students.length})`">
          <div v-if="data.students.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="student in data.students"
              :key="student.id"
              class="flex items-center justify-between py-3"
            >
              <span class="text-sm text-highlighted">{{ student.name }}</span>
              <UBadge
                :label="studentStatusLabels[student.status] ?? student.status"
                :color="studentStatusColors[student.status] ?? 'neutral'"
                variant="subtle"
              />
            </div>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-users" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhum aluno matriculado
            </p>
            <UButton
              icon="i-lucide-plus"
              label="Aluno"
              @click="() => { assignStudentModalOpen = true }"
            />
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
