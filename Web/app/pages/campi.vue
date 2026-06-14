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
      <div v-if="status === 'pending'" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 p-4">
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

      <div v-else class="p-4 space-y-4">
        <div class="flex justify-end">
          <UButton icon="i-lucide-plus" label="Campus" @click="createModalOpen = true" />
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          <div
            v-for="campus in data.items"
            :key="campus.id"
            class="rounded-xl border border-default bg-elevated p-4 flex flex-col gap-4"
          >
            <div class="flex items-start justify-between gap-2">
              <div>
                <p class="font-semibold text-base text-highlighted">{{ campus.name }}</p>
                <p class="text-sm text-muted">{{ campus.city }} - {{ campus.state }}</p>
              </div>
              <UButton
                icon="i-lucide-pencil"
                color="neutral"
                variant="ghost"
                size="sm"
                @click="openEdit(campus)"
              />
            </div>

            <div class="flex flex-col gap-2 text-sm">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-user-pen" class="size-4 text-muted shrink-0" />
                <span>{{ campus.teachers }} Professores</span>
              </div>
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-graduation-cap" class="size-4 text-muted shrink-0" />
                <span>{{ campus.students }}/{{ campus.capacity.toLocaleString('pt-BR') }} Alunos</span>
              </div>
            </div>

            <div class="flex flex-col gap-1">
              <div class="h-1.5 w-full rounded-full bg-accented overflow-hidden">
                <div
                  class="h-full rounded-full"
                  :style="{
                    width: `${campus.fillRate}%`,
                    backgroundColor: fillRateColor(campus.fillRate),
                  }"
                />
              </div>
              <p class="text-xs text-muted text-right">{{ campus.fillRate }}% de ocupação</p>
            </div>
          </div>
        </div>
      </div>
    </template>
  </UDashboardPanel>

  <CampiCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CampiEditModal v-model:open="editModalOpen" :campus="selectedCampus" @updated="refresh()" />
</template>
