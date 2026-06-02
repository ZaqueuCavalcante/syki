<script setup lang="ts">
const props = defineProps<{
  loading: boolean
  icon: string
  message: string
  buttonLabel?: string
}>()

const emit = defineEmits<{
  create: []
}>()
</script>

<template>
  <div v-if="!props.loading" class="flex flex-col items-center gap-4 py-12">
    <UIcon :name="icon" class="size-16 text-muted" />
    <p class="text-muted text-sm">
      {{ message }}
    </p>
    <UButton
      v-if="buttonLabel"
      icon="i-lucide-plus"
      :label="buttonLabel"
      @click="emit('create')"
    />
  </div>
</template>
