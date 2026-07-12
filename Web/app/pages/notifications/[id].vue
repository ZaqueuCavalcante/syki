<script setup lang="ts">
interface ViewsByDayItem {
  day: string
  views: number
}

interface GetInstitutionNotificationOut {
  id: number
  title: string
  description: string
  createdAt: string
  recipients: number
  viewed: number
  viewRate: number
  viewsByDay: ViewsByDayItem[]
}

const route = useRoute()
const config = useRuntimeConfig()
const notificationId = route.params.id

const { data, status, error } = await useFetch<GetInstitutionNotificationOut>(
  `${config.public.backendUrl}/notifications/institution/${notificationId}`,
  { credentials: 'include', server: false },
)

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
</script>

<template>
  <UDashboardPanel id="notification-details">
    <template #header>
      <UDashboardNavbar :title="data?.title ?? 'Notificação'">
        <template #leading>
          <UButton
            icon="i-lucide-arrow-left"
            color="neutral"
            variant="ghost"
            to="/notifications"
          />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-12">
        <AppSpinner class="size-6 text-muted" />
      </div>

      <div v-else-if="error || !data" class="flex flex-col items-center gap-4 py-12">
        <UIcon name="i-lucide-triangle-alert" class="size-16 text-muted" />
        <p class="text-muted text-sm">
          Notificação não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/notifications" />
      </div>

      <div v-else class="flex flex-col gap-6 py-4">
        <UPageCard title="Dados da notificação">
          <dl class="grid grid-cols-1 gap-x-8 gap-y-4 sm:grid-cols-2">
            <div class="flex flex-col gap-1 sm:col-span-2">
              <dt class="text-xs text-muted">
                Descrição
              </dt>
              <dd class="text-sm text-highlighted">
                {{ data.description }}
              </dd>
            </div>

            <div class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                Criada em
              </dt>
              <dd class="text-sm text-highlighted">
                {{ formatDateTime(data.createdAt) }}
              </dd>
            </div>

            <div class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                Destinatários
              </dt>
              <dd class="text-sm text-highlighted">
                {{ data.recipients }}
              </dd>
            </div>
          </dl>

          <div class="mt-6">
            <div class="h-1.5 w-full rounded-full bg-accented overflow-hidden">
              <div
                class="h-full rounded-full"
                :style="{ width: `${data.viewRate}%`, backgroundColor: viewRateColor(data.viewRate) }"
              />
            </div>
            <div class="flex items-center justify-between mt-1">
              <p class="text-xs text-muted">
                {{ data.viewed }} de {{ data.recipients }} visualizaram
              </p>
              <UBadge
                :color="viewRateBadgeColor(data.viewRate)"
                variant="subtle"
                size="sm"
                :label="`${data.viewRate}%`"
              />
            </div>
          </div>
        </UPageCard>

        <NotificationsViewsChart :views-by-day="data.viewsByDay" />
      </div>
    </template>
  </UDashboardPanel>
</template>
