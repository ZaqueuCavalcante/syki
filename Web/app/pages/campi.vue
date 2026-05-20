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
      <UTable
        :data="data?.items"
        :columns="columns"
        :loading="status === 'pending'"
        :ui="{
          base: 'table-fixed border-separate border-spacing-0',
          thead: '[&>tr]:bg-elevated/50 [&>tr]:after:content-none',
          tbody: '[&>tr]:last:[&>td]:border-b-0',
          th: 'py-2 first:rounded-l-lg last:rounded-r-lg border-y border-default first:border-l last:border-r',
          td: 'border-b border-default',
        }"
      >
        <template #empty>
          <div class="flex flex-col items-center gap-4 py-12">
            <UIcon name="i-lucide-map-pin-off" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhum campus cadastrado
            </p>
            <UButton icon="i-lucide-plus" label="Campus" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <CampiCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CampiEditModal v-model:open="editModalOpen" :campus="selectedCampus" @updated="refresh()" />
</template>
