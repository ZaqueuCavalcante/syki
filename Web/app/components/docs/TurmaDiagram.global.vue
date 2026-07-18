<script setup lang="ts">
type Growth = {
  label: string
  icon: string
  badge?: string
  optional?: boolean
}

const props = withDefaults(defineProps<{
  discipline?: string
  period?: string
  growth?: Growth[]
}>(), {
  discipline: 'Banco de Dados',
  period: '2025.1',
  growth: () => [
    { label: 'Professores', icon: 'i-lucide-user-pen', badge: 'No máximo 2' },
    { label: 'Alunos', icon: 'i-lucide-graduation-cap' },
    { label: 'Aulas', icon: 'i-lucide-notebook-pen' },
    { label: 'Horários', icon: 'i-lucide-clock' },
    { label: 'Atividades', icon: 'i-lucide-clipboard-list' },
    { label: 'Salas', icon: 'i-lucide-school', optional: true }
  ]
})
</script>

<template>
  <div class="not-prose my-6 flex flex-col items-center">
    <UCard
      :ui="{ body: 'p-4 sm:p-4' }"
      class="w-full max-w-xs ring-primary/50"
    >
      <div class="flex items-center gap-3">
        <span class="flex size-9 shrink-0 items-center justify-center rounded-lg bg-primary/10">
          <UIcon name="i-lucide-door-open" class="size-5 text-primary" />
        </span>
        <div class="flex flex-col">
          <span class="text-sm font-medium text-highlighted">Turma</span>
          <span class="text-xs text-muted">Nasce simples</span>
        </div>
      </div>

      <div class="mt-3 flex flex-wrap items-center gap-1.5">
        <UBadge
          :label="props.discipline"
          icon="i-lucide-book-open"
          color="primary"
          variant="subtle"
          size="sm"
        />
        <UBadge
          :label="props.period"
          icon="i-lucide-calendar"
          color="primary"
          variant="subtle"
          size="sm"
        />
      </div>
    </UCard>

    <div class="h-6 w-px bg-accented" />

    <span class="mb-4 text-xs font-medium text-muted">
      Depois, cresce com&hellip;
    </span>

    <div class="grid w-full grid-cols-2 gap-4 sm:grid-cols-3">
      <UCard
        v-for="item in props.growth"
        :key="item.label"
        :ui="{ body: 'p-4 sm:p-4' }"
      >
        <div class="flex flex-col gap-3">
          <div class="flex items-center gap-2">
            <UIcon :name="item.icon" class="size-4 shrink-0 text-muted" />
            <span class="truncate text-sm font-medium text-highlighted">{{ item.label }}</span>
          </div>

          <div v-if="item.badge || item.optional" class="flex flex-wrap items-center gap-1.5">
            <UBadge
              v-if="item.badge"
              :label="item.badge"
              color="primary"
              variant="subtle"
              size="sm"
            />
            <UBadge
              v-if="item.optional"
              label="Opcional"
              color="neutral"
              variant="subtle"
              size="sm"
            />
          </div>
        </div>
      </UCard>
    </div>
  </div>
</template>
