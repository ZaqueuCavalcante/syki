<script setup lang="ts">
const props = withDefaults(defineProps<{
  loading: boolean
  icon: string
  message: string
  buttonLabel?: string
  filtered?: boolean
  notFoundIcon?: string
  notFoundMessage?: string
  clearFiltersLabel?: string
}>(), {
  notFoundIcon: 'i-lucide-search-x',
  clearFiltersLabel: 'Remover filtros',
})

const emit = defineEmits<{
  create: []
  clearFilters: []
}>()
</script>

<template>
  <div v-if="!props.loading" class="flex flex-col items-center gap-4 py-12">
    <UIcon :name="props.filtered ? props.notFoundIcon : props.icon" class="size-16 text-muted" />
    <p class="text-muted text-sm">
      {{ props.filtered ? props.notFoundMessage : props.message }}
    </p>
    <UButton
      v-if="props.filtered"
      icon="i-lucide-filter-x"
      color="neutral"
      variant="subtle"
      :label="props.clearFiltersLabel"
      @click="() => { emit('clearFilters') }"
    />
    <UButton
      v-else-if="props.buttonLabel"
      icon="i-lucide-plus"
      :label="props.buttonLabel"
      @click="() => { emit('create') }"
    />
  </div>
</template>
