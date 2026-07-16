<script setup lang="ts">
import type { GetAgendaOut } from '~/types/agenda'

const config = useRuntimeConfig()
const { account } = useUserAccount()
const { selectedChild, selectedChildId, loading: childrenLoading } = useParentChildren()

// Professor consome a agenda do professor; aluno, a do aluno; responsável, a do filho selecionado.
// Pro responsável a URL só existe depois que os filhos carregam e um é selecionado.
const url = computed(() => {
  const base = config.public.backendUrl
  const userType = account.value?.userType

  if (userType === 'Teacher') return `${base}/teachers/agenda`
  if (userType === 'Parent') return selectedChildId.value ? `${base}/parents/students/${selectedChildId.value}/agenda` : null

  return `${base}/students/agenda`
})

const { data, status } = await useAsyncData<GetAgendaOut | null>(
  'agenda',
  async () => {
    if (!url.value) return null
    return await $fetch<GetAgendaOut>(url.value, { credentials: 'include' })
  },
  { server: false, watch: [url] }
)

const title = computed(() =>
  account.value?.userType === 'Parent' && selectedChild.value
    ? `Agenda — ${selectedChild.value.name}`
    : 'Agenda',
)
</script>

<template>
  <UDashboardPanel id="agenda">
    <template #header>
      <UDashboardNavbar :title="title">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending' || childrenLoading" class="flex justify-center py-16">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>
      <div v-else-if="account?.userType === 'Parent' && !selectedChildId" class="flex flex-col items-center gap-3 pt-16 text-center">
        <UIcon name="i-lucide-users-round" class="size-10 text-muted" />
        <p class="text-muted">
          Nenhum aluno vinculado à sua conta. Fale com a secretaria da instituição.
        </p>
      </div>
      <AgendaWeek v-else :days="data?.days ?? []" />
    </template>
  </UDashboardPanel>
</template>
