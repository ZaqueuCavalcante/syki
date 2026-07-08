<script setup lang="ts">
import { formatTimeAgo } from '@vueuse/core'

interface NotificationItem {
  id: number
  title: string
  description: string
  createdAt: string
}

interface GetInstitutionNotificationsOut {
  total: number
  page: number
  pageSize: number
  items: NotificationItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

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
            class="rounded-xl border border-default bg-elevated flex flex-col overflow-hidden hover:shadow-md transition-shadow duration-200"
          >
            <div class="px-4 pt-3 pb-3 flex items-start justify-between gap-2">
              <p class="flex-1 min-w-0 font-bold text-base text-highlighted truncate">
                {{ notification.title }}
              </p>
              <time
                :datetime="notification.createdAt"
                class="shrink-0 text-xs text-muted whitespace-nowrap"
              >
                {{ formatTimeAgo(new Date(notification.createdAt)) }}
              </time>
            </div>

            <div class="border-t border-default mx-4" />

            <p class="px-4 py-3 text-sm text-muted line-clamp-3">
              {{ notification.description }}
            </p>
          </div>
        </div>
      </div>
    </template>
  </UDashboardPanel>

  <NotificationsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
