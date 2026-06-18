<script setup lang="ts">
interface GetHomeStatsOut {
  enrolledStudents: number
  activeTeachers: number
  offeredCourses: number
  registeredDisciplines: number
}

const { account } = useUserAccount()
const config = useRuntimeConfig()

const { data: stats } = await useFetch<GetHomeStatsOut>(`${config.public.backendUrl}/home/stats`, {
  credentials: 'include',
  server: false,
})

const cards = computed(() => [
  { label: 'Alunos matriculados',     value: stats.value?.enrolledStudents,      icon: 'i-lucide-graduation-cap' },
  { label: 'Professores ativos',      value: stats.value?.activeTeachers,        icon: 'i-lucide-user-pen'       },
  { label: 'Cursos ofertados',        value: stats.value?.offeredCourses,        icon: 'i-lucide-notebook'       },
  { label: 'Disciplinas cadastradas', value: stats.value?.registeredDisciplines, icon: 'i-lucide-library'        },
])
</script>

<template>
  <div class="p-6 space-y-6">
    <div>
      <h2 class="text-2xl font-semibold text-highlighted">Bem-vindo, {{ account?.name }}</h2>
      <p class="text-muted text-sm mt-1">{{ account?.institution }}</p>
    </div>

    <UPageGrid class="lg:grid-cols-4">
      <UPageCard
        v-for="card in cards"
        :key="card.label"
        :icon="card.icon"
        :title="card.label"
        spotlight
        :ui="{ container: 'gap-y-1.5', wrapper: 'items-start', leading: 'p-2.5 rounded-full bg-primary/10 ring ring-inset ring-primary/25' }"
      >
        <span class="text-3xl font-bold text-highlighted">{{ card.value ?? '-' }}</span>
      </UPageCard>
    </UPageGrid>

    <UCard>
      <template #header>
        <span class="font-semibold text-highlighted">Visão geral da instituição</span>
      </template>
      <div class="grid grid-cols-2 gap-4 text-sm">
        <div class="space-y-1">
          <p class="text-muted">Período letivo atual</p>
          <p class="font-medium text-highlighted">2025.1</p>
        </div>
        <div class="space-y-1">
          <p class="text-muted">Taxa de aprovação</p>
          <p class="font-medium text-highlighted">91%</p>
        </div>
        <div class="space-y-1">
          <p class="text-muted">Turmas em andamento</p>
          <p class="font-medium text-highlighted">58</p>
        </div>
        <div class="space-y-1">
          <p class="text-muted">Evasão no semestre</p>
          <p class="font-medium text-highlighted">3,2%</p>
        </div>
      </div>
    </UCard>
  </div>
</template>
