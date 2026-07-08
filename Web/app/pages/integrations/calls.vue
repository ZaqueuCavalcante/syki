<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface WebhookCallItem {
  id: number
  eventType: string
  status: string
  attemptsCount: number
  createdAt: string
}

interface GetWebhookCallsOut {
  total: number
  page: number
  pageSize: number
  items: WebhookCallItem[]
}

const UBadge = resolveComponent('UBadge')

const config = useRuntimeConfig()

const page = ref(1)
const pageSize = 20

const { data, status } = await useFetch<GetWebhookCallsOut>(
  `${config.public.backendUrl}/webhooks/calls`,
  {
    credentials: 'include',
    server: false,
    query: { page, pageSize },
  },
)

const eventLabels: Record<string, string> = {
  StudentCreated: 'Aluno criado',
  ClassActivityCreated: 'Atividade publicada',
}

const statusLabels: Record<string, string> = {
  Pending: 'Pendente',
  Processing: 'Processando',
  Success: 'Sucesso',
  Error: 'Erro',
}

const statusColors: Record<string, 'neutral' | 'info' | 'success' | 'error'> = {
  Pending: 'neutral',
  Processing: 'info',
  Success: 'success',
  Error: 'error',
}

function formatDateTime(value: string) {
  return new Date(value).toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

const columns: TableColumn<WebhookCallItem>[] = [
  {
    accessorKey: 'id',
    header: 'Id',
  },
  {
    accessorKey: 'eventType',
    header: 'Evento',
    cell: ({ row }) => eventLabels[row.original.eventType] ?? row.original.eventType,
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h(
      UBadge,
      { variant: 'subtle', color: statusColors[row.original.status] ?? 'neutral' },
      () => statusLabels[row.original.status] ?? row.original.status,
    ),
  },
  {
    accessorKey: 'attemptsCount',
    header: 'Tentativas',
  },
  {
    accessorKey: 'createdAt',
    header: 'Criada em',
    cell: ({ row }) => formatDateTime(row.original.createdAt),
  },
]
</script>

<template>
  <div>
    <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
      <template #empty>
        <div class="flex flex-col items-center justify-center gap-3 py-12 text-center">
          <UIcon name="i-lucide-radio" class="size-8 text-muted" />
          <p class="text-sm text-muted">Nenhuma chamada de webhook realizada</p>
        </div>
      </template>
    </DataTable>

    <div v-if="(data?.total ?? 0) > pageSize" class="flex justify-end mt-4">
      <UPagination
        v-model:page="page"
        :items-per-page="pageSize"
        :total="data?.total ?? 0"
      />
    </div>
  </div>
</template>
