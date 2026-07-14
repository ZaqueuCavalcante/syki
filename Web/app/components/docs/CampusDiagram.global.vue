<script setup lang="ts">
type Campus = {
  name: string
  city: string
  state: string
  capacity: number
}

const props = withDefaults(defineProps<{
  institution?: string
  campi?: Campus[]
}>(), {
  institution: 'Instituição',
  campi: () => [
    { name: 'Campus Central', city: 'Recife', state: 'PE', capacity: 1200 },
    { name: 'Campus Norte', city: 'Olinda', state: 'PE', capacity: 800 },
    { name: 'Campus Sul', city: 'Jaboatão', state: 'PE', capacity: 450 }
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
          <UIcon name="i-lucide-building-2" class="size-5 text-primary" />
        </span>
        <div class="flex flex-col">
          <span class="text-sm font-medium text-highlighted">{{ props.institution }}</span>
          <span class="text-xs text-muted">UFPE</span>
        </div>
      </div>
    </UCard>

    <div class="h-6 w-px bg-accented" />

    <div class="hidden w-full grid-cols-3 gap-4 sm:grid">
      <div
        v-for="(campus, i) in props.campi"
        :key="campus.name"
        class="relative h-6"
      >
        <div
          v-if="props.campi.length > 1"
          class="absolute top-0 h-px bg-accented"
          :class="[
            i === 0 ? 'left-1/2 -right-2' : '',
            i === props.campi.length - 1 ? '-left-2 right-1/2' : '',
            i > 0 && i < props.campi.length - 1 ? '-left-2 -right-2' : ''
          ]"
        />
        <div class="absolute inset-y-0 left-1/2 w-px -translate-x-1/2 bg-accented" />
      </div>
    </div>

    <div class="grid w-full grid-cols-1 gap-4 sm:grid-cols-3">
      <UCard
        v-for="campus in props.campi"
        :key="campus.name"
        :ui="{ body: 'p-4 sm:p-4' }"
      >
        <div class="flex flex-col gap-3">
          <div class="flex items-center gap-2">
            <UIcon name="i-lucide-map-pin" class="size-4 shrink-0 text-muted" />
            <span class="truncate text-sm font-medium text-highlighted">{{ campus.name }}</span>
          </div>

          <div class="flex flex-wrap items-center gap-1.5">
            <UBadge
              :label="`${campus.city} · ${campus.state}`"
              color="neutral"
              variant="subtle"
              size="sm"
            />
            <UBadge
              :label="`${campus.capacity} vagas`"
              color="neutral"
              variant="subtle"
              size="sm"
              icon="i-lucide-users"
            />
          </div>
        </div>
      </UCard>
    </div>
  </div>
</template>
