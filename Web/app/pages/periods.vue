<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface PeriodItem {
  id: number
  name: string
  startAt: string
  endAt: string
}

interface GetPeriodsOut {
  total: number
  items: PeriodItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetPeriodsOut>(`${config.public.backendUrl}/periods/academic`, {
  credentials: 'include',
  server: false
})

function formatDate(value: string) {
  const [year, month, day] = value.split('-')
  return `${day}/${month}/${year}`
}

const columns: TableColumn<PeriodItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'startAt',
    header: 'Início',
    cell: ({ row }) => formatDate(row.original.startAt),
  },
  {
    accessorKey: 'endAt',
    header: 'Término',
    cell: ({ row }) => formatDate(row.original.endAt),
  },
]
</script>

<template>
  <UDashboardPanel id="periods">
    <template #header>
      <UDashboardNavbar title="Períodos">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Período" @click="createModalOpen = true" />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-calendar"
            message="Nenhum período cadastrado"
            button-label="Período"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <PeriodsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
