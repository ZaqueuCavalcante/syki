<script setup lang="ts">
import { formatTimeAgo } from '@vueuse/core'

const { isNotificationsSlideoverOpen } = useDashboard()
const { notifications, onlyUnread, loading, markAsViewed } = useNotifications()

const unreadNotifications = computed(() => notifications.value.filter(n => !n.viewedAt))

async function markOne(id: number) {
  await markAsViewed(id)
}

async function markAll() {
  await markAsViewed()
}
</script>

<template>
  <USlideover
    v-model:open="isNotificationsSlideoverOpen"
    title="Notificações"
  >
    <template #header>
      <div class="flex flex-col gap-2 w-full">
        <div class="flex items-center justify-between w-full gap-3">
          <p class="text-base font-semibold text-highlighted">Notificações</p>
          <div class="flex items-center gap-2">
            <UButton
              v-if="unreadNotifications.length > 0"
              variant="ghost"
              color="neutral"
              size="xs"
              icon="i-lucide-check-check"
              @click="markAll"
            >
              Marcar todas
            </UButton>
            <UButton
              variant="ghost"
              color="neutral"
              size="md"
              icon="i-lucide-x"
              @click="() => { isNotificationsSlideoverOpen = false }"
            />
          </div>
        </div>
        <USwitch
          v-model="onlyUnread"
          size="xs"
          label="Não lidas"
        />
      </div>
    </template>

    <template #body>
      <div v-if="loading" class="flex items-center justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
      </div>

      <div
        v-else-if="notifications.length === 0"
        class="flex flex-col items-center justify-center gap-2 py-12 text-center"
      >
        <UIcon name="i-lucide-bell-off" class="size-8 text-muted" />
        <p class="text-sm text-muted">
          {{ onlyUnread ? 'Nenhuma notificação não lida' : 'Nenhuma notificação' }}
        </p>
      </div>

      <div v-else class="flex flex-col gap-1 -mx-3">
        <div
          v-for="notification in notifications"
          :key="notification.id"
          class="px-3 py-3 rounded-md hover:bg-elevated/50 flex items-start gap-3 transition-colors"
          :class="!notification.viewedAt ? 'bg-elevated/30' : ''"
        >
          <div class="flex-1 min-w-0">
            <div class="flex items-start justify-between gap-2">
              <div class="flex items-center gap-1.5 min-w-0">
                <span
                  v-if="!notification.viewedAt"
                  class="shrink-0 size-1.5 rounded-full bg-primary mt-1"
                />
                <p
                  class="text-sm font-medium text-highlighted truncate"
                  :class="notification.viewedAt ? 'pl-3' : ''"
                >
                  {{ notification.title }}
                </p>
              </div>
              <time
                :datetime="notification.createdAt"
                class="shrink-0 text-xs text-muted whitespace-nowrap"
              >
                {{ formatTimeAgo(new Date(notification.createdAt)) }}
              </time>
            </div>
            <p class="text-sm text-dimmed mt-0.5 pl-3 line-clamp-2">
              {{ notification.description }}
            </p>
          </div>

          <UButton
            v-if="!notification.viewedAt"
            icon="i-lucide-check"
            variant="ghost"
            color="neutral"
            size="xs"
            class="shrink-0 mt-0.5"
            title="Marcar como lida"
            @click="markOne(notification.id)"
          />
        </div>
      </div>
    </template>
  </USlideover>
</template>
