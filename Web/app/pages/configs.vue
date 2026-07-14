<script setup lang="ts">
import type { InstitutionConfig } from '~/types/configs'

const config = useRuntimeConfig()

const editModalOpen = ref(false)

const { data, status, refresh } = await useFetch<InstitutionConfig>(`${config.public.backendUrl}/institutions/config`, {
  credentials: 'include',
  server: false,
})

function formatNumber(value: number): string {
  return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}
</script>

<template>
  <UDashboardPanel id="configs">
    <template #header>
      <UDashboardNavbar title="Configurações">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="w-full lg:max-w-2xl mx-auto min-w-0">
        <USkeleton v-if="status === 'pending'" class="h-56 rounded-xl" />

        <template v-else-if="data">
          <UPageCard
            title="Critérios de aprovação"
            description="Usados nas disciplinas da sua instituição."
            variant="naked"
            orientation="horizontal"
            class="mb-4"
          >
            <UButton
              icon="i-lucide-pencil"
              label="Editar"
              color="neutral"
              variant="subtle"
              class="w-fit lg:ms-auto"
              @click="() => { editModalOpen = true }"
            />
          </UPageCard>

          <div class="rounded-xl border border-default bg-elevated overflow-hidden">
            <div class="grid grid-cols-1 sm:grid-cols-2 py-4">
              <div class="flex flex-col items-center px-4 sm:border-r border-default">
                <span class="text-3xl font-bold text-highlighted leading-none">{{ formatNumber(data.noteLimit) }}</span>
                <span class="text-xs text-muted mt-1.5">Nota mínima</span>
                <span class="text-xs text-dimmed mt-1 text-center">Abaixo disso o aluno é reprovado por nota.</span>
              </div>
              <div class="flex flex-col items-center px-4 mt-4 sm:mt-0">
                <div class="flex items-baseline gap-0.5">
                  <span class="text-3xl font-bold text-highlighted leading-none">{{ formatNumber(data.frequencyLimit) }}</span>
                  <span class="text-base font-bold text-highlighted">%</span>
                </div>
                <span class="text-xs text-muted mt-1.5">Frequência mínima</span>
                <span class="text-xs text-dimmed mt-1 text-center">Abaixo disso o aluno é reprovado por falta.</span>
              </div>
            </div>
          </div>
        </template>
      </div>
    </template>
  </UDashboardPanel>

  <ConfigsEditModal v-model:open="editModalOpen" :config="data ?? null" @updated="refresh()" />
</template>
