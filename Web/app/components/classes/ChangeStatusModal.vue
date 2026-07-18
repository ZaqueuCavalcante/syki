<script setup lang="ts">
import type { ClassStatusTransition } from '~/types/classes'

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{
  transition: ClassStatusTransition | null
  loading?: boolean
}>()
const emit = defineEmits<{ confirm: [] }>()

const isMobile = useIsMobile()
</script>

<template>
  <UModal
    v-model:open="open"
    :title="transition?.title ?? 'Mudar status da turma'"
    description="Revise a mudança antes de confirmar. O status só será alterado após a confirmação."
    :fullscreen="isMobile"
  >
    <template #body>
      <div v-if="transition" class="space-y-5">
        <div class="flex items-center justify-center gap-4">
          <div class="flex flex-col items-center gap-1.5">
            <UBadge
              :label="classStatusLabels[transition.fromStatus] ?? transition.fromStatus"
              :color="classStatusColors[transition.fromStatus] ?? 'neutral'"
              variant="subtle"
            />
            <span class="text-xs text-muted">Status atual</span>
          </div>
          <UIcon name="i-lucide-arrow-right" class="size-5 text-muted shrink-0" />
          <div class="flex flex-col items-center gap-1.5">
            <UBadge
              :label="classStatusLabels[transition.toStatus] ?? transition.toStatus"
              :color="classStatusColors[transition.toStatus] ?? 'neutral'"
              variant="subtle"
            />
            <span class="text-xs text-muted">Novo status</span>
          </div>
        </div>

        <div class="rounded-lg border border-default p-3 space-y-2.5">
          <p class="text-xs font-semibold text-highlighted">
            O que muda ao confirmar
          </p>
          <ul class="space-y-2">
            <li
              v-for="(imp, i) in transition.implications"
              :key="i"
              class="flex items-start gap-2"
            >
              <UIcon :name="imp.icon" :class="imp.class" class="size-4 shrink-0 mt-0.5" />
              <span class="text-xs text-muted">{{ imp.text }}</span>
            </li>
          </ul>
        </div>
      </div>

      <div class="flex justify-end gap-2 pt-4">
        <UButton
          label="Cancelar"
          color="neutral"
          variant="subtle"
          :disabled="loading"
          @click="() => { open = false }"
        />
        <UButton
          :label="transition?.actionLabel ?? 'Confirmar'"
          :icon="transition?.actionIcon"
          :loading="loading"
          @click="() => { emit('confirm') }"
        />
      </div>
    </template>
  </UModal>
</template>
