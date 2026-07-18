<script setup lang="ts">
type Step = {
  label: string
  icon: string
  virtual?: boolean
}

const props = withDefaults(defineProps<{
  steps?: Step[]
}>(), {
  steps: () => [
    { label: 'Pré-matrícula', icon: 'i-lucide-file-plus' },
    { label: 'Matrícula', icon: 'i-lucide-clipboard-list' },
    { label: 'Aguardando início', icon: 'i-lucide-hourglass', virtual: true },
    { label: 'Iniciada', icon: 'i-lucide-circle-play' },
    { label: 'Finalizada', icon: 'i-lucide-circle-check' }
  ]
})
</script>

<template>
  <div class="not-prose my-6 flex flex-col items-stretch gap-2 sm:flex-row sm:items-center sm:justify-center">
    <template v-for="(step, i) in props.steps" :key="step.label">
      <UCard
        :ui="{ body: 'p-3 sm:p-3' }"
        class="flex-1"
        :class="step.virtual ? 'ring-0 border border-dashed border-accented' : 'ring-primary/50'"
      >
        <div class="flex items-center gap-2 sm:flex-col sm:text-center">
          <span
            class="flex size-8 shrink-0 items-center justify-center rounded-lg"
            :class="step.virtual ? 'bg-elevated' : 'bg-primary/10'"
          >
            <UIcon
              :name="step.icon"
              class="size-4"
              :class="step.virtual ? 'text-muted' : 'text-primary'"
            />
          </span>
          <span class="text-sm font-medium text-highlighted">{{ step.label }}</span>
        </div>
      </UCard>

      <div
        v-if="i < props.steps.length - 1"
        class="flex shrink-0 items-center justify-center text-muted"
      >
        <UIcon name="i-lucide-chevron-down" class="size-4 sm:hidden" />
        <UIcon name="i-lucide-chevron-right" class="hidden size-4 sm:block" />
      </div>
    </template>
  </div>
</template>
