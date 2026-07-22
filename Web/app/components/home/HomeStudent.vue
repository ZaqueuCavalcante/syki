<script setup lang="ts">
import type { GetStudentCourseDetailsOut, StudentCourseDiscipline } from '~/types/students'

const { account } = useUserAccount()
const config = useRuntimeConfig()

const { data, status } = await useFetch<GetStudentCourseDetailsOut>(`${config.public.backendUrl}/students/course`, {
  credentials: 'include',
  server: false,
})

const isLoading = computed(() => status.value === 'pending')
const hasCourse = computed(() => !!data.value)

const disciplines = computed<StudentCourseDiscipline[]>(() => data.value?.disciplines ?? [])

const concludedStatuses = ['Aprovada', 'Dispensada']

const summary = computed(() => {
  const list = disciplines.value
  const concluded = list.filter(d => concludedStatuses.includes(d.status)).length
  const doing = list.filter(d => d.status === 'Cursando').length
  const total = list.length
  return {
    total,
    concluded,
    doing,
    remaining: total - concluded - doing,
    progress: total ? Math.round((concluded / total) * 100) : 0,
  }
})

const stats = computed(() => [
  { label: 'Progresso no curso', value: `${summary.value.progress}%`, icon: 'i-lucide-trending-up' },
  { label: 'Concluídas', value: summary.value.concluded, icon: 'i-lucide-circle-check' },
  { label: 'Cursando', value: summary.value.doing, icon: 'i-lucide-book-open' },
  { label: 'A cursar', value: summary.value.remaining, icon: 'i-lucide-map-pin' },
])

// Disciplinas agrupadas por período (semestre) da grade, em ordem crescente
const periods = computed(() => {
  const map = new Map<number, StudentCourseDiscipline[]>()
  for (const d of disciplines.value) {
    if (!map.has(d.period)) map.set(d.period, [])
    map.get(d.period)!.push(d)
  }
  return [...map.entries()]
    .sort((a, b) => a[0] - b[0])
    .map(([period, items]) => ({
      period,
      items,
      concluded: items.filter(d => concludedStatuses.includes(d.status)).length,
    }))
})

const subtitle = computed(() => {
  if (!data.value) return account.value?.institution
  return data.value.course
})

const meta = computed(() => {
  if (!data.value) return []
  return [
    { icon: 'i-lucide-map-pin', text: data.value.campus },
    { icon: 'i-lucide-calendar', text: data.value.period },
    { icon: 'i-lucide-sun', text: courseSessionLabels[data.value.session] ?? data.value.session },
  ].filter(m => !!m.text)
})
</script>

<template>
  <div class="space-y-6">
    <div>
      <h2 class="text-2xl font-semibold text-highlighted">Bem-vindo, {{ account?.name }}</h2>
      <p class="inline-flex items-center gap-1.5 text-muted text-sm mt-1">
        <UIcon v-if="hasCourse" name="i-lucide-graduation-cap" class="size-4 shrink-0" />
        {{ subtitle }}
      </p>
      <div v-if="meta.length" class="flex flex-wrap items-center gap-x-4 gap-y-1 mt-2">
        <span v-for="m in meta" :key="m.text" class="inline-flex items-center gap-1.5 text-xs text-muted">
          <UIcon :name="m.icon" class="size-3.5 shrink-0" />
          {{ m.text }}
        </span>
      </div>
    </div>

    <!-- Loading -->
    <template v-if="isLoading">
      <div class="grid grid-cols-2 xl:grid-cols-4 gap-3">
        <USkeleton v-for="i in 4" :key="i" class="h-16 w-full" />
      </div>
      <USkeleton class="h-64 w-full" />
    </template>

    <!-- Sem matrícula em curso -->
    <UCard v-else-if="!hasCourse">
      <div class="flex flex-col items-center justify-center text-center py-10 gap-3">
        <div class="p-3 rounded-full bg-primary/10 ring ring-inset ring-primary/25">
          <UIcon name="i-lucide-graduation-cap" class="size-6 text-primary" />
        </div>
        <div>
          <p class="font-medium text-highlighted">Você ainda não está matriculado em um curso</p>
          <p class="text-sm text-muted mt-1">Assim que sua matrícula for feita, sua grade curricular aparecerá aqui.</p>
        </div>
      </div>
    </UCard>

    <template v-else>
      <div class="grid grid-cols-2 xl:grid-cols-4 gap-3">
        <div
          v-for="stat in stats"
          :key="stat.label"
          class="flex items-center gap-3 rounded-lg p-3 ring ring-default bg-elevated/40"
        >
          <div class="flex items-center justify-center p-2 rounded-lg bg-primary/10 ring ring-inset ring-primary/20 shrink-0">
            <UIcon :name="stat.icon" class="size-4 text-primary" />
          </div>
          <div class="min-w-0">
            <p class="text-xl font-semibold text-highlighted leading-none">{{ stat.value }}</p>
            <p class="text-xs text-muted mt-1 truncate">{{ stat.label }}</p>
          </div>
        </div>
      </div>

      <UCard>
        <template #header>
          <div class="flex items-center justify-between gap-4">
            <span class="font-semibold text-highlighted">Grade curricular</span>
            <span class="text-sm text-muted">{{ summary.concluded }} de {{ summary.total }} concluídas</span>
          </div>
        </template>

        <!-- Progresso geral -->
        <div class="mb-6">
          <div class="flex items-center justify-between mb-2">
            <span class="text-sm text-muted">Progresso no curso</span>
            <span class="text-sm font-semibold text-highlighted">{{ summary.progress }}%</span>
          </div>
          <UProgress :model-value="summary.progress" :max="100" />
        </div>

        <!-- Disciplinas agrupadas por período -->
        <div class="space-y-8">
          <section v-for="group in periods" :key="group.period">
            <div class="flex items-center gap-3 mb-3">
              <span class="text-sm font-medium text-highlighted">{{ group.period }}º período</span>
              <USeparator class="flex-1" />
              <span class="text-xs text-muted whitespace-nowrap">{{ group.concluded }}/{{ group.items.length }}</span>
            </div>

            <div class="grid gap-3 sm:grid-cols-2">
              <div
                v-for="disc in group.items"
                :key="disc.id"
                class="flex items-start gap-3 rounded-lg p-3 ring ring-default bg-elevated/40"
              >
                <span class="mt-1.5 size-2.5 shrink-0 rounded-full" :class="studentDisciplineStatusDot[disc.status] ?? 'bg-neutral-300 dark:bg-neutral-700'" />
                <div class="min-w-0 flex-1">
                  <p class="font-medium text-highlighted truncate">{{ disc.name }}</p>
                  <p class="text-xs text-muted mt-0.5">{{ disc.credits }} créditos · {{ disc.workload }}h</p>
                </div>
                <UBadge
                  :color="studentDisciplineStatusColors[disc.status] ?? 'neutral'"
                  variant="subtle"
                  size="sm"
                  class="shrink-0"
                >
                  {{ studentDisciplineStatusLabels[disc.status] ?? disc.status }}
                </UBadge>
              </div>
            </div>
          </section>

          <p v-if="!periods.length" class="text-sm text-muted text-center py-6">
            A grade deste curso ainda não possui disciplinas cadastradas.
          </p>
        </div>
      </UCard>
    </template>
  </div>
</template>
