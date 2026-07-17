<script setup lang="ts">
interface ClassroomSchedule {
  classId: number
  discipline: string
  teachers: string[]
  day: string // 'Monday' | 'Tuesday' | ...
  startAt: string // ex: "H07_00"
  endAt: string // ex: "H10_00"
}

interface GetClassroomOut {
  id: number
  name: string
  campusId: number
  campus: string
  capacity: number
  schedules: ClassroomSchedule[]
}

const route = useRoute()
const config = useRuntimeConfig()

const classroomId = route.params.id as string

const { data, status, error } = await useFetch<GetClassroomOut>(
  `${config.public.backendUrl}/classrooms/${classroomId}`,
  { credentials: 'include', server: false },
)

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Nome', value: data.value.name },
    { label: 'Campus', value: data.value.campus || '—' },
    { label: 'Capacidade', value: `${data.value.capacity.toLocaleString('pt-BR')} alunos` },
  ]
})
</script>

<template>
  <UDashboardPanel id="classroom-details">
    <template #header>
      <UDashboardNavbar :title="data?.name ?? 'Sala'">
        <template #leading>
          <UButton
            icon="i-lucide-arrow-left"
            color="neutral"
            variant="ghost"
            to="/classrooms"
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
          Sala não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/classrooms" />
      </div>

      <div v-else class="flex flex-col gap-6 py-4">
        <UPageCard title="Dados da sala">
          <dl class="grid grid-cols-1 gap-x-8 gap-y-4 sm:grid-cols-3">
            <div v-for="item in details" :key="item.label" class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                {{ item.label }}
              </dt>
              <dd class="text-sm text-highlighted">
                {{ item.value }}
              </dd>
            </div>
          </dl>
        </UPageCard>

        <UPageCard :title="`Agenda (${data.schedules.length})`">
          <div v-if="data.schedules.length" class="flex flex-col divide-y divide-default">
            <NuxtLink
              v-for="(s, i) in data.schedules"
              :key="i"
              :to="`/classes/${s.classId}`"
              class="flex items-center justify-between gap-4 py-3 hover:bg-elevated/50 -mx-2 px-2 rounded-md transition-colors"
            >
              <div class="flex flex-col min-w-0">
                <span class="text-sm font-medium text-highlighted truncate">{{ s.discipline || 'Turma' }}</span>
                <span v-if="s.teachers.length" class="text-xs text-muted truncate">{{ s.teachers.join(', ') }}</span>
              </div>
              <UBadge
                :label="formatClassSchedule(s)"
                color="neutral"
                variant="subtle"
                icon="i-lucide-clock"
                class="shrink-0"
              />
            </NuxtLink>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-calendar-x" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhuma turma alocada nesta sala
            </p>
          </div>
        </UPageCard>
      </div>
    </template>
  </UDashboardPanel>
</template>
