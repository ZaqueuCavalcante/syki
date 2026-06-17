<script setup lang="ts">
defineProps<{
  collapsed?: boolean
}>()
</script>

<template>
  <div class="flex items-center gap-2 py-1.5" :class="collapsed ? 'justify-center w-full' : 'px-2.5'">
    <EstudIcon class="size-6 shrink-0" />
    <span v-if="!collapsed" class="truncate text-xl font-semibold text-highlighted">
      Estud
    </span>
  </div>
</template>
