<script setup lang="ts">
const { account } = useUserAccount()

const disciplinas = [
  { nome: 'Cálculo I', professor: 'Dr. Ricardo Alves', cargaHoraria: '80h', nota: 8.5, status: 'Em andamento' },
  { nome: 'Programação Orientada a Objetos', professor: 'Profa. Carla Mendes', cargaHoraria: '60h', nota: 9.0, status: 'Em andamento' },
  { nome: 'Banco de Dados', professor: 'Prof. André Costa', cargaHoraria: '60h', nota: 7.8, status: 'Em andamento' },
  { nome: 'Engenharia de Software', professor: 'Profa. Júlia Ferreira', cargaHoraria: '80h', nota: null, status: 'Aguardando' },
]

const stats = [
  { label: 'Semestre atual', value: '3º', icon: 'i-lucide-flag' },
  { label: 'Disciplinas', value: '4', icon: 'i-lucide-book-open' },
  { label: 'Média geral', value: '8,4', icon: 'i-lucide-chart-line' },
  { label: 'Créditos cursados', value: '92', icon: 'i-lucide-check-circle' },
]
</script>

<template>
  <div class="space-y-6">
    <div>
      <h2 class="text-2xl font-semibold text-highlighted">Bem-vindo, {{ account?.name }}</h2>
      <p class="text-muted text-sm mt-1">{{ account?.course ?? account?.institution }}</p>
    </div>

    <UPageGrid class="lg:grid-cols-2 xl:grid-cols-4">
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
        <span class="font-semibold text-highlighted">Minhas disciplinas</span>
      </template>
      <div class="divide-y divide-default">
        <div v-for="disc in disciplinas" :key="disc.nome" class="py-3 flex items-center justify-between">
          <div>
            <p class="font-medium text-highlighted">{{ disc.nome }}</p>
            <p class="text-sm text-muted">{{ disc.professor }} · {{ disc.cargaHoraria }}</p>
          </div>
          <div class="flex items-center gap-2">
            <span v-if="disc.nota !== null" class="text-sm font-semibold text-highlighted">{{ disc.nota }}</span>
            <UBadge :color="disc.status === 'Em andamento' ? 'primary' : 'neutral'" variant="subtle">
              {{ disc.status }}
            </UBadge>
          </div>
        </div>
      </div>
    </UCard>
  </div>
</template>
