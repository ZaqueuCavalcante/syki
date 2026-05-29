<script setup lang="ts">
const { account } = useUserAccount()

const turmas = [
  { disciplina: 'Cálculo I', curso: 'Engenharia de Software', alunos: 38, horario: 'Seg/Qua 08h–10h' },
  { disciplina: 'Álgebra Linear', curso: 'Ciência da Computação', alunos: 31, horario: 'Ter/Qui 10h–12h' },
  { disciplina: 'Estruturas de Dados', curso: 'Sistemas de Informação', alunos: 42, horario: 'Sex 14h–18h' },
]

const stats = [
  { label: 'Turmas ativas', value: '3', icon: 'i-lucide-layout-list' },
  { label: 'Total de alunos', value: '111', icon: 'i-lucide-users' },
  { label: 'Disciplinas', value: '3', icon: 'i-lucide-book-open' },
  { label: 'Aulas esta semana', value: '6', icon: 'i-lucide-calendar-check' },
]
</script>

<template>
  <div class="p-6 space-y-6">
    <div>
      <h2 class="text-2xl font-semibold text-highlighted">Bem-vindo, {{ account?.name }}</h2>
      <p class="text-muted text-sm mt-1">{{ account?.institution }}</p>
    </div>

    <UPageGrid class="lg:grid-cols-4">
      <UPageCard
        v-for="stat in stats"
        :key="stat.label"
        :icon="stat.icon"
        :title="stat.label"
        spotlight
        :ui="{ container: 'gap-y-1.5', wrapper: 'items-start', leading: 'p-2.5 rounded-full bg-primary/10 ring ring-inset ring-primary/25' }"
      >
        <span class="text-3xl font-bold text-highlighted">{{ stat.value }}</span>
      </UPageCard>
    </UPageGrid>

    <UCard>
      <template #header>
        <span class="font-semibold text-highlighted">Minhas turmas</span>
      </template>
      <div class="divide-y divide-default">
        <div v-for="turma in turmas" :key="turma.disciplina" class="py-3 flex items-center justify-between">
          <div>
            <p class="font-medium text-highlighted">{{ turma.disciplina }}</p>
            <p class="text-sm text-muted">{{ turma.curso }}</p>
            <p class="text-xs text-muted mt-0.5">{{ turma.horario }}</p>
          </div>
          <UBadge color="primary" variant="subtle">{{ turma.alunos }} alunos</UBadge>
        </div>
      </div>
    </UCard>
  </div>
</template>
