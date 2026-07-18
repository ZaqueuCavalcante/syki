<script setup lang="ts">
import type { GetStudentClassActivitiesOut, GetStudentClassOut } from '~/types/classes'

const props = defineProps<{ classId: string }>()

const config = useRuntimeConfig()

const { data, status, error } = await useFetch<GetStudentClassOut>(
  `${config.public.backendUrl}/students/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

const { data: activitiesData } = await useFetch<GetStudentClassActivitiesOut>(
  `${config.public.backendUrl}/students/classes/${props.classId}/activities`,
  { credentials: 'include', server: false },
)

const activities = computed(() => activitiesData.value?.activities ?? [])

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Disciplina', value: data.value.discipline },
    { label: 'Status da turma', status: true },
    { label: 'Professores', value: data.value.teachers.join(', ') || '—' },
    { label: 'Período', value: data.value.period },
    { label: 'Carga horária', value: `${data.value.workload}h` },
    { label: 'Minha matrícula', myStatus: true },
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
            to="/agenda"
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
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/agenda" />
      </div>

      <div v-else class="flex flex-col gap-6 py-4">
        <UPageCard title="Ciclo de vida" :ui="{ wrapper: 'w-full', body: 'w-full' }">
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
              <dd v-else-if="item.myStatus">
                <UBadge
                  :label="studentClassStatusLabels[data.myStatus] ?? data.myStatus"
                  :color="studentClassStatusColors[data.myStatus] ?? 'neutral'"
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

        <UPageCard :title="`Atividades (${activities.length})`">
          <div v-if="activities.length" class="flex flex-col divide-y divide-default">
            <NuxtLink
              v-for="activity in activities"
              :key="activity.id"
              :to="`/classes/${props.classId}/activities/${activity.id}`"
              class="flex flex-col gap-2 py-3 sm:flex-row sm:items-center sm:justify-between hover:bg-elevated/50"
            >
              <div class="flex flex-col gap-1">
                <span class="text-sm text-highlighted">{{ activity.title }}</span>
                <span class="text-xs text-muted">
                  {{ classActivityTypeLabels[activity.type] ?? activity.type }}
                  · {{ activity.note }}
                  · Peso {{ activity.weight }}
                  · Entrega até {{ formatClassActivityDueDate(activity.dueDate, activity.dueHour) }}
                </span>
              </div>

              <div class="flex items-center gap-2">
                <UBadge
                  v-if="activity.workStatus === 'Finalized'"
                  :label="`Nota ${activity.value} · ${activity.ponderedValue} na ${activity.note}`"
                  color="neutral"
                  variant="subtle"
                  icon="i-lucide-award"
                />
                <UBadge
                  :label="classActivityWorkStatusLabels[activity.workStatus] ?? activity.workStatus"
                  :color="classActivityWorkStatusColors[activity.workStatus] ?? 'neutral'"
                  variant="subtle"
                />
              </div>
            </NuxtLink>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-clipboard-list" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhuma atividade cadastrada
            </p>
          </div>
        </UPageCard>
      </div>
    </template>
  </UDashboardPanel>
</template>
