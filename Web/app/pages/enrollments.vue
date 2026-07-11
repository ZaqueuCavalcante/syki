<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface EnrollmentPeriodItem {
  id: number
  name: string
  startAt: string
  endAt: string
}

interface GetEnrollmentPeriodsOut {
  total: number
  items: EnrollmentPeriodItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetEnrollmentPeriodsOut>(`${config.public.backendUrl}/periods/enrollment`, {
  credentials: 'include',
  server: false
})

function formatDate(value: string) {
  const [year, month, day] = value.split('-')
  return `${day}/${month}/${year}`
}

const columns: TableColumn<EnrollmentPeriodItem>[] = [
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
  <UDashboardPanel id="enrollments">
    <template #header>
      <UDashboardNavbar title="Matrículas">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Período de matrícula" @click="() => { createModalOpen = true }" />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-clipboard-list"
            message="Nenhum período de matrícula cadastrado"
            button-label="Período de matrícula"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <EnrollmentsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
