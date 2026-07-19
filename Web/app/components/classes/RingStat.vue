<script setup lang="ts">
const props = defineProps<{
  // Preenchimento do anel, de 0 a 100
  percent: number
  // Texto no centro do anel (ex: "87%", "7,8")
  centerText: string
  // Linha principal, à direita do anel
  title: string
  // Linha secundária, em muted
  subtitle: string
  // Classe de cor (ex: "text-primary", "text-warning")
  colorClass: string
}>()

// dasharray em unidades reais da circunferência (r = 15.5), evitando depender
// do atributo pathLength (que o Vue não normaliza em SVG).
const CIRCUMFERENCE = 2 * Math.PI * 15.5
const dash = computed(() => {
  const filled = (CIRCUMFERENCE * Math.min(Math.max(props.percent, 0), 100)) / 100
  return `${filled} ${CIRCUMFERENCE}`
})
</script>

<template>
  <div class="flex items-center gap-3">
    <div class="relative size-12 shrink-0">
      <svg viewBox="0 0 36 36" class="size-12 -rotate-90">
        <circle
          cx="18"
          cy="18"
          r="15.5"
          fill="none"
          stroke-width="3.5"
          class="stroke-current text-muted opacity-15"
        />
        <circle
          cx="18"
          cy="18"
          r="15.5"
          fill="none"
          stroke-width="3.5"
          stroke-linecap="round"
          :stroke-dasharray="dash"
          class="stroke-current transition-all duration-500"
          :class="colorClass"
        />
      </svg>
      <span
        class="absolute inset-0 flex items-center justify-center text-[0.7rem] font-semibold"
        :class="colorClass"
      >
        {{ centerText }}
      </span>
    </div>
    <div class="flex flex-col leading-tight">
      <span class="text-sm font-medium text-highlighted">{{ title }}</span>
      <span class="text-xs text-muted">{{ subtitle }}</span>
    </div>
  </div>
</template>
