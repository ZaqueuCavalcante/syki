<script setup lang="ts">
import type { GetTeacherClassActivityOut } from '~/types/classes'

const props = defineProps<{ classId: string, activityId: string }>()

const config = useRuntimeConfig()

const { data, status, error } = await useFetch<GetTeacherClassActivityOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/activities/${props.activityId}`,
  { credentials: 'include', server: false },
)

const works = computed(() => {
  if (!data.value) return []
  const weight = data.value.weight
  return data.value.works.map(work => ({
    ...work,
    ponderedValue: work.value * weight / 100,
  }))
})

const details = computed(() => {
  if (!data.value) return []
  return [
    { label: 'Status', status: true },
    { label: 'Tipo', value: classActivityTypeLabels[data.value.type] ?? data.value.type },
    { label: 'Nota', value: data.value.note },
    { label: 'Peso', value: `${data.value.weight}%` },
    { label: 'Entrega até', value: formatClassActivityDueDate(data.value.dueDate, data.value.dueHour) },
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

        <UPageCard :title="`Entregas (${data.deliveredWorks} / ${data.totalWorks})`">
          <div v-if="works.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="work in works"
              :key="work.studentId"
              class="flex flex-col gap-2 py-3 sm:flex-row sm:items-center sm:justify-between"
            >
              <div class="flex flex-col gap-1">
                <span class="text-sm text-highlighted">{{ work.student }}</span>
                <ULink
                  v-if="work.link"
                  :to="work.link"
                  target="_blank"
                  class="text-xs text-muted hover:text-highlighted"
                >
                  {{ work.link }}
                </ULink>
                <span v-else class="text-xs text-muted">
                  Nenhuma entrega
                </span>
              </div>

              <div class="flex items-center gap-2">
                <UBadge
                  v-if="work.status === 'Finalized'"
                  :label="`Nota ${work.value} · ${work.ponderedValue} na ${data.note}`"
                  color="neutral"
                  variant="subtle"
                  icon="i-lucide-award"
                />
                <UBadge
                  :label="classActivityWorkStatusLabels[work.status] ?? work.status"
                  :color="classActivityWorkStatusColors[work.status] ?? 'neutral'"
                  variant="subtle"
                />
              </div>
            </div>
          </div>
          <div v-else class="flex flex-col items-center gap-3 py-6">
            <UIcon name="i-lucide-file-check" class="size-10 text-muted" />
            <p class="text-sm text-muted">
              Nenhuma entrega até o momento
            </p>
          </div>
        </UPageCard>
      </div>
    </template>
  </UDashboardPanel>
</template>
