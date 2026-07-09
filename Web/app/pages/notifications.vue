<script setup lang="ts">
interface NotificationItem {
  id: number
  title: string
  description: string
  createdAt: string
  recipients: number
  viewed: number
  viewRate: number
}

interface GetInstitutionNotificationsOut {
  total: number
  page: number
  pageSize: number
  items: NotificationItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

function formatDateTime(value: string) {
  return new Date(value).toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function viewRateColor(rate: number): string {
  if (rate < 50) return 'var(--ui-error)'
  if (rate < 80) return 'var(--ui-warning)'
  return 'var(--ui-success)'
}

function viewRateBadgeColor(rate: number): 'error' | 'warning' | 'success' {
  if (rate < 50) return 'error'
  if (rate < 80) return 'warning'
  return 'success'
}

const { data, status, refresh } = await useFetch<GetInstitutionNotificationsOut>(
  `${config.public.backendUrl}/notifications/institution`,
  {
    credentials: 'include',
    server: false,
  },
)
</script>

<template>
  <UDashboardPanel id="notifications">
    <template #header>
      <UDashboardNavbar title="Notificações">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex items-center justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
      </div>

      <div
        v-else-if="!data?.items?.length"
        class="flex flex-col items-center justify-center gap-3 py-12 text-center"
      >
        <UIcon name="i-lucide-bell-off" class="size-8 text-muted" />
        <p class="text-sm text-muted">Nenhuma notificação cadastrada</p>
        <UButton icon="i-lucide-plus" label="Notificação" @click="createModalOpen = true" />
      </div>

      <div v-else class="space-y-4">
        <div class="flex justify-end">
          <UButton icon="i-lucide-plus" label="Notificação" class="shrink-0" @click="createModalOpen = true" />
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          <div
            v-for="notification in data.items"
            :key="notification.id"
            class="group relative rounded-xl border border-default bg-elevated/50 p-4 flex flex-col gap-3 hover:border-primary/50 hover:bg-elevated transition-colors duration-200"
          >
            <div class="flex items-start justify-between gap-2">
              <p class="flex-1 min-w-0 font-semibold text-sm text-highlighted truncate">
                {{ notification.title }}
              </p>
              <time
                :datetime="notification.createdAt"
                class="shrink-0 text-xs text-muted whitespace-nowrap"
              >
                {{ formatDateTime(notification.createdAt) }}
              </time>
            </div>

            <p class="text-sm text-muted line-clamp-3">
              {{ notification.description }}
            </p>

            <div class="mt-auto pt-1">
              <div class="h-1.5 w-full rounded-full bg-accented overflow-hidden">
                <div
                  class="h-full rounded-full"
                  :style="{ width: `${notification.viewRate}%`, backgroundColor: viewRateColor(notification.viewRate) }"
                />
              </div>
              <div class="flex items-center justify-between mt-1">
                <p class="text-xs text-muted">
                  {{ notification.viewed }} de {{ notification.recipients }} visualizaram
                </p>
                <UBadge :color="viewRateBadgeColor(notification.viewRate)" variant="subtle" size="sm" :label="`${notification.viewRate}%`" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
  </UDashboardPanel>

  <NotificationsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
