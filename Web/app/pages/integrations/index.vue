<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface WebhookSubscriptionItem {
  id: number
  name: string
  url: string
  isActive: boolean
  events: string[]
  createdAt: string
}

interface GetWebhookSubscriptionsOut {
  total: number
  items: WebhookSubscriptionItem[]
}

const UButton = resolveComponent('UButton')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const selectedSubscription = ref<WebhookSubscriptionItem | null>(null)

function openEdit(item: WebhookSubscriptionItem) {
  selectedSubscription.value = item
  editModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetWebhookSubscriptionsOut>(
  `${config.public.backendUrl}/webhooks/subscriptions`,
  { credentials: 'include', server: false }
)

const columns: TableColumn<WebhookSubscriptionItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'url',
    header: 'URL',
  },
  {
    accessorKey: 'events',
    header: 'Eventos',
    cell: ({ row }) => row.original.events.length,
  },
  {
    accessorKey: 'isActive',
    header: 'Ativo',
    cell: ({ row }) => row.original.isActive ? 'Sim' : 'Não',
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
  <div>
    <div class="flex justify-end mb-4">
      <UButton icon="i-lucide-plus" label="Webhook" @click="createModalOpen = true" />
    </div>

    <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
      <template #empty>
        <TableEmptyState
          :loading="status === 'pending'"
          icon="i-lucide-webhook"
          message="Nenhum webhook cadastrado"
          button-label="Webhook"
          @create="createModalOpen = true"
        />
      </template>
    </DataTable>
  </div>

  <IntegrationsWebhooksCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <IntegrationsWebhooksEditModal v-model:open="editModalOpen" :subscription="selectedSubscription" @updated="refresh()" />
</template>
