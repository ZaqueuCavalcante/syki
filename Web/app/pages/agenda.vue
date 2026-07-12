<script setup lang="ts">
import type { GetAgendaOut } from '~/types/agenda'

const config = useRuntimeConfig()
const { account } = useUserAccount()

// Professor consome a agenda do professor; aluno, a agenda do aluno.
const endpoint = computed(() =>
  account.value?.userType === 'Teacher'
    ? `${config.public.backendUrl}/teachers/agenda`
    : `${config.public.backendUrl}/students/agenda`,
)

const { data, status } = await useFetch<GetAgendaOut>(endpoint, {
  credentials: 'include',
  server: false,
})
</script>

<template>
  <UDashboardPanel id="agenda">
    <template #header>
      <UDashboardNavbar title="Agenda">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-16">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>
      <AgendaWeek v-else :days="data?.days ?? []" />
    </template>
  </UDashboardPanel>
</template>
