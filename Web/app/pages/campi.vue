<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

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

const UButton = resolveComponent('UButton')

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
  lazy: true
})

const columns: TableColumn<CampusItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'city',
    header: 'Cidade',
  },
  {
    accessorKey: 'state',
    header: 'Estado',
  },
  {
    accessorKey: 'capacity',
    header: 'Capacidade',
    cell: ({ row }) => row.original.capacity.toLocaleString('pt-BR'),
  },
  {
    accessorKey: 'students',
    header: 'Alunos',
  },
  {
    accessorKey: 'teachers',
    header: 'Professores',
  },
  {
    accessorKey: 'fillRate',
    header: 'Ocupação',
    cell: ({ row }) => `${row.original.fillRate}%`,
  },
  {
    id: 'actions',
    cell: ({ row }) => h(UButton, {
      icon: 'i-lucide-pencil',
      color: 'neutral',
      variant: 'ghost',
      size: 'sm',
      onClick: () => openEdit(row.original),
    }),
  },
]
</script>

<template>
  <UDashboardPanel id="campi">
    <template #header>
      <UDashboardNavbar title="Campi">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Campus" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-map-pin"
            message="Nenhum campus cadastrado"
            button-label="Campus"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <CampiCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CampiEditModal v-model:open="editModalOpen" :campus="selectedCampus" @updated="refresh()" />
</template>
