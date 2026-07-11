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

// ── MOCK (remover) — preview de agenda de aluno com dados fake ──
const isStudent = computed(() => account.value?.userType === 'Student')
const mockAgenda: GetAgendaOut = {
  days: [
    {
      day: 'Monday',
      disciplines: [
        { classId: 9, name: 'Banco de Dados', start: 'H07_00', end: 'H08_30' },
        { classId: 1, name: 'Cálculo I', start: 'H09_15', end: 'H10_45' },
        { classId: 2, name: 'Algoritmos', start: 'H13_30', end: 'H14_45' },
      ],
    },
    {
      day: 'Tuesday',
      disciplines: [
        { classId: 3, name: 'Física I', start: 'H07_30', end: 'H09_00' },
        { classId: 10, name: 'Estruturas de Dados', start: 'H10_00', end: 'H11_15' },
        { classId: 11, name: 'Banco de Dados', start: 'H14_00', end: 'H15_30' },
      ],
    },
    {
      day: 'Wednesday',
      disciplines: [
        { classId: 5, name: 'Banco de Dados', start: 'H07_15', end: 'H08_45' },
        { classId: 4, name: 'Cálculo I', start: 'H09_30', end: 'H11_00' },
        { classId: 12, name: 'Estruturas de Dados', start: 'H13_00', end: 'H14_30' },
      ],
    },
    {
      day: 'Thursday',
      disciplines: [
        { classId: 7, name: 'Estruturas de Dados', start: 'H07_00', end: 'H08_15' },
        { classId: 6, name: 'Física I', start: 'H10_30', end: 'H12_00' },
        { classId: 13, name: 'Banco de Dados', start: 'H15_00', end: 'H16_30' },
      ],
    },
    {
      day: 'Friday',
      disciplines: [
        { classId: 15, name: 'Banco de Dados', start: 'H08_00', end: 'H09_30' },
        { classId: 8, name: 'Algoritmos', start: 'H10_15', end: 'H11_45' },
        { classId: 14, name: 'Estruturas de Dados', start: 'H13_15', end: 'H14_45' },
      ],
    },
  ],
}
// ── FIM MOCK ───────────────────────────────────────────────────
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
      <!-- MOCK (remover) — preview no lugar dos dados reais do aluno -->
      <div v-else-if="isStudent">
        <p class="mb-4 text-sm font-medium text-muted">
          Preview (mock) — remover depois
        </p>
        <AgendaWeek :days="mockAgenda.days" />
      </div>
      <!-- FIM MOCK -->

      <AgendaWeek v-else :days="data?.days ?? []" />
    </template>
  </UDashboardPanel>
</template>
