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
  lazy: true
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

        <template #right>
          <UButton icon="i-lucide-plus" label="Período" @click="createModalOpen = true" />
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
          <div v-if="status !== 'pending'" class="flex flex-col items-center gap-4 py-12">
            <UIcon name="i-lucide-calendar" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhum período cadastrado
            </p>
            <UButton icon="i-lucide-plus" label="Período" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <PeriodsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
