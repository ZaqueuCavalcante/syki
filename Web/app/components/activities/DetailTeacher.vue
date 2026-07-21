<script setup lang="ts">
import type { GetTeacherClassActivityOut } from '~/types/classes'

const props = defineProps<{ classId: string, activityId: string }>()

const config = useRuntimeConfig()

const breadcrumb = computed(() => [
  { label: 'Início', to: '/home', icon: 'i-lucide-house' },
  { label: 'Detalhes da turma', to: `/classes/${props.classId}` },
  { label: 'Atividade' },
])

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
</script>

<template>
  <UDashboardPanel id="activity-details">
    <template #header>
      <UDashboardNavbar>
        <template #title>
          <UBreadcrumb :items="breadcrumb" />
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

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="grid grid-cols-1 items-start gap-x-4 gap-y-1 sm:grid-cols-[1fr_auto]">
          <h1 class="order-1 text-2xl font-semibold tracking-tight text-highlighted sm:col-start-1 sm:row-start-1">
            {{ data.title }}
          </h1>
          <div class="order-2 flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted sm:col-start-1 sm:row-start-2">
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-tag" class="size-4" />
              {{ classActivityTypeLabels[data.type] ?? data.type }}
            </span>
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-hash" class="size-4" />
              {{ data.note }}
            </span>
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-scale" class="size-4" />
              Peso {{ data.weight }}%
            </span>
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-calendar-clock" class="size-4" />
              Entrega até {{ formatClassActivityDueDate(data.dueDate, data.dueHour) }}
            </span>
            <UBadge
              :label="classActivityStatusLabels[data.status] ?? data.status"
              :color="classActivityStatusColors[data.status] ?? 'neutral'"
              variant="subtle"
            />
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Descrição
          </h2>
          <p v-if="data.description" class="text-sm text-highlighted whitespace-pre-line">
            {{ data.description }}
          </p>
          <p v-else class="text-sm text-muted">
            Nenhuma descrição cadastrada
          </p>
        </section>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Entregas ({{ data.deliveredWorks }} / {{ data.totalWorks }})
          </h2>

          <div v-if="works.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="work in works"
              :key="work.studentId"
              class="flex flex-col gap-2 py-3 sm:flex-row sm:items-center sm:justify-between"
            >
              <div class="flex items-center gap-2.5">
                <UAvatar :alt="work.student" size="2xs" />
                <div class="flex flex-col gap-0.5">
                  <span class="text-sm font-medium text-highlighted">{{ work.student }}</span>
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
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
