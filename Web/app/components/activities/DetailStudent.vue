<script setup lang="ts">
import type { GetStudentClassActivityOut } from '~/types/classes'

const props = defineProps<{ classId: string, activityId: string }>()

const config = useRuntimeConfig()

const { data, status, error, refresh } = await useFetch<GetStudentClassActivityOut>(
  `${config.public.backendUrl}/students/classes/${props.classId}/activities/${props.activityId}`,
  { credentials: 'include', server: false },
)

const createWorkModalOpen = ref(false)

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Status da atividade', status: true },
    { label: 'Tipo', value: classActivityTypeLabels[data.value.type] ?? data.value.type },
    { label: 'Nota', value: data.value.note },
    { label: 'Peso', value: `${data.value.weight}%` },
    { label: 'Entrega até', value: formatClassActivityDueDate(data.value.dueDate, data.value.dueHour) },
    { label: 'Minha entrega', workStatus: true },
  ]
})
</script>

<template>
  <UDashboardPanel id="activity-details">
    <template #header>
      <UDashboardNavbar :title="data?.title ?? 'Atividade'">
        <template #leading>
          <UButton
            icon="i-lucide-arrow-left"
            color="neutral"
            variant="ghost"
            :to="`/classes/${props.classId}`"
          />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>

      <div v-else-if="error || !data" class="flex flex-col items-center gap-4 py-12">
        <UIcon name="i-lucide-triangle-alert" class="size-16 text-muted" />
        <p class="text-muted text-sm">
          Atividade não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" :to="`/classes/${props.classId}`" />
      </div>

      <div v-else class="flex flex-col gap-6 py-4">
        <UPageCard title="Dados da atividade">
          <dl class="grid grid-cols-1 gap-x-8 gap-y-4 sm:grid-cols-2">
            <div v-for="item in details" :key="item.label" class="flex flex-col gap-1">
              <dt class="text-xs text-muted">
                {{ item.label }}
              </dt>
              <dd v-if="item.status">
                <UBadge
                  :label="classActivityStatusLabels[data.status] ?? data.status"
                  :color="classActivityStatusColors[data.status] ?? 'neutral'"
                  variant="subtle"
                />
              </dd>
              <dd v-else-if="item.workStatus">
                <UBadge
                  :label="classActivityWorkStatusLabels[data.workStatus] ?? data.workStatus"
                  :color="classActivityWorkStatusColors[data.workStatus] ?? 'neutral'"
                  variant="subtle"
                />
              </dd>
              <dd v-else class="text-sm text-highlighted">
                {{ item.value }}
              </dd>
            </div>
          </dl>
        </UPageCard>

        <UPageCard title="Descrição">
          <p v-if="data.description" class="text-sm text-highlighted whitespace-pre-line">
            {{ data.description }}
          </p>
          <p v-else class="text-sm text-muted">
            Nenhuma descrição cadastrada
          </p>
        </UPageCard>

        <UPageCard :ui="{ wrapper: 'w-full', body: 'w-full' }">
          <template #title>
            <div class="flex w-full items-center justify-between gap-2">
              <span>Minha entrega</span>
              <UButton
                v-if="!data.workLink"
                icon="i-lucide-plus"
                label="Entrega"
                size="sm"
                @click="() => { createWorkModalOpen = true }"
              />
            </div>
          </template>

          <div class="flex flex-col gap-4">
            <div v-if="data.workLink" class="flex flex-col gap-1">
              <span class="text-xs text-muted">Link entregue</span>
              <ULink
                :to="data.workLink"
                target="_blank"
                class="text-sm text-highlighted hover:text-primary"
              >
                {{ data.workLink }}
              </ULink>
            </div>
            <p v-else class="text-sm text-muted">
              Você ainda não entregou esta atividade
            </p>

            <div v-if="data.workStatus === 'Finalized'" class="flex items-center gap-2">
              <UBadge
                :label="`Nota ${data.value} · ${data.ponderedValue} na ${data.note}`"
                color="neutral"
                variant="subtle"
                icon="i-lucide-award"
              />
            </div>
          </div>
        </UPageCard>

        <ActivitiesCreateWorkModal
          v-model:open="createWorkModalOpen"
          :activity-id="props.activityId"
          @created="() => { refresh() }"
        />
      </div>
    </template>
  </UDashboardPanel>
</template>
