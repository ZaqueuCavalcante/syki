<script setup lang="ts">
interface CampusItem {
  id: number
  name: string
  state: string
  city: string
  capacity: number
  students: number
  teachers: number
  fillRate: number
}

interface GetCampiOut {
  total: number
  items: CampusItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const selectedCampus = ref<CampusItem | null>(null)

function openEdit(campus: CampusItem) {
  selectedCampus.value = campus
  editModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetCampiOut>(`${config.public.backendUrl}/campi`, {
  credentials: 'include',
  server: false
})

function fillRateColor(rate: number): string {
  if (rate < 50) return 'var(--ui-error)'
  if (rate < 80) return 'var(--ui-warning)'
  return 'var(--ui-success)'
}

function fillRateBadgeColor(rate: number): 'error' | 'warning' | 'success' {
  if (rate < 50) return 'error'
  if (rate < 80) return 'warning'
  return 'success'
}
</script>

<template>
  <UDashboardPanel id="campi">
    <template #header>
      <UDashboardNavbar title="Campi">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        <USkeleton v-for="i in 3" :key="i" class="h-44 rounded-xl" />
      </div>

      <TableEmptyState
        v-else-if="!data?.items?.length"
        :loading="false"
        icon="i-lucide-map-pin"
        message="Nenhum campus cadastrado"
        button-label="Campus"
        @create="createModalOpen = true"
      />

      <div v-else class="space-y-4">
        <div class="flex items-start justify-between gap-4">
          <div>
            <p class="text-sm text-muted mt-0.5">Visualize e gerencie os campus da sua instituição.</p>
          </div>
          <UButton icon="i-lucide-plus" label="Campus" class="shrink-0" @click="() => { createModalOpen = true }" />
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          <div
            v-for="campus in data.items"
            :key="campus.id"
            class="rounded-xl border border-default bg-elevated flex flex-col overflow-hidden hover:shadow-md transition-shadow duration-200"
          >
            <!-- Header -->
            <div class="px-4 pt-3 pb-3 flex items-start justify-between gap-2">
              <div class="flex-1 min-w-0">
                <p class="font-bold text-base text-highlighted truncate">{{ campus.name }}</p>
                <div class="flex items-center gap-1 mt-0.5">
                  <UIcon name="i-lucide-map-pin" class="size-3.5 text-muted shrink-0" />
                  <p class="text-sm text-muted">{{ campus.city }} · {{ campus.state }}</p>
                </div>
              </div>
              <div class="flex items-center gap-1.5 shrink-0">
                <UTooltip text="Editar">
                  <UButton icon="i-lucide-pencil" color="neutral" variant="ghost" size="xs" @click="($event.currentTarget as HTMLElement).blur(); openEdit(campus)" />
                </UTooltip>
              </div>
            </div>

            <!-- Divisor -->
            <div class="border-t border-default mx-4" />

            <!-- Stats -->
            <div class="grid grid-cols-2 py-3">
              <div class="flex flex-col px-4 border-r border-default">
                <span class="text-2xl font-bold text-highlighted leading-none">{{ campus.teachers }}</span>
                <span class="text-xs text-muted mt-1">Professores</span>
              </div>
              <div class="flex flex-col px-4">
                <div class="flex items-baseline gap-1">
                  <span class="text-2xl font-bold text-highlighted leading-none">{{ campus.students }}</span>
                  <span class="text-xs text-muted">/{{ campus.capacity.toLocaleString('pt-BR') }}</span>
                </div>
                <span class="text-xs text-muted mt-1">Alunos</span>
              </div>
            </div>

            <!-- Barra de ocupação -->
            <div class="px-4 pb-4">
              <div class="h-1.5 w-full rounded-full bg-accented overflow-hidden">
                <div
                  class="h-full rounded-full"
                  :style="{ width: `${campus.fillRate}%`, backgroundColor: fillRateColor(campus.fillRate) }"
                />
              </div>
              <div class="flex items-center justify-between mt-1">
                <p class="text-xs text-muted">Taxa de ocupação</p>
                <UBadge :color="fillRateBadgeColor(campus.fillRate)" variant="subtle" size="sm" :label="`${campus.fillRate}%`" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
  </UDashboardPanel>

  <CampiCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CampiEditModal v-model:open="editModalOpen" :campus="selectedCampus" @updated="refresh()" />
</template>
