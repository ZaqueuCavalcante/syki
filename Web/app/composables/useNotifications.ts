import { createSharedComposable, useIntervalFn } from '@vueuse/core'

export interface NotificationItem {
  id: number
  notificationType: string
  title: string
  description: string
  createdAt: string
  viewedAt: string | null
  metadata: Record<string, unknown> | null
}

interface NotificationsResponse {
  total: number
  page: number
  pageSize: number
  items: NotificationItem[]
}

const _useNotifications = () => {
  const config = useRuntimeConfig()
  const { isNotificationsSlideoverOpen } = useDashboard()

  const unreadCount = ref(0)
  const notifications = ref<NotificationItem[]>([])
  const onlyUnread = ref(false)
  const loading = ref(false)

  async function fetchUnreadCount() {
    try {
      const data = await $fetch<{ count: number }>(
        `${config.public.backendUrl}/notifications/unread-count`,
        { credentials: 'include' }
      )
      unreadCount.value = data.count
    } catch { /* ignore */ }
  }

  async function fetchNotifications() {
    loading.value = true
    try {
      const data = await $fetch<NotificationsResponse>(
        `${config.public.backendUrl}/notifications`,
        {
          credentials: 'include',
          query: { page: 1, pageSize: 50, unreadOnly: onlyUnread.value },
        }
      )
      notifications.value = data.items
    } catch { /* ignore */ }
    finally { loading.value = false }
  }

  async function markAsViewed(id?: number) {
    await $fetch(`${config.public.backendUrl}/notifications/mark-as-viewed`, {
      method: 'PUT',
      credentials: 'include',
      body: id ? { notificationId: id } : { markAll: true },
    })
    if (id) {
      const item = notifications.value.find(n => n.id === id)
      if (item) item.viewedAt = new Date().toISOString()
    } else {
      notifications.value.forEach(n => { n.viewedAt = n.viewedAt ?? new Date().toISOString() })
    }
    await fetchUnreadCount()
  }

  watch(isNotificationsSlideoverOpen, (open) => {
    if (open) fetchNotifications()
  })

  watch(onlyUnread, () => {
    if (isNotificationsSlideoverOpen.value) fetchNotifications()
  })

  useIntervalFn(fetchUnreadCount, 60_000)
  fetchUnreadCount()

  return {
    unreadCount,
    notifications,
    onlyUnread,
    loading,
    fetchNotifications,
    markAsViewed,
  }
}

export const useNotifications = createSharedComposable(_useNotifications)
