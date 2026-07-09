<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

const UBadge = resolveComponent('UBadge')

interface ClassItem {
  id: number
  discipline: string
  teacher: string
  period: string
  vacancies: number
  status: string
}

interface GetClassesOut {
  total: number
  items: ClassItem[]
}

const statusLabels: Record<string, string> = {
  OnPreEnrollment: 'Pré-matrícula',
  OnEnrollment: 'Matrícula',
  AwaitingStart: 'Aguardando início',
  Started: 'Iniciada',
  Finalized: 'Finalizada',
}

const statusColors: Record<string, 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'> = {
  OnPreEnrollment: 'neutral',
  OnEnrollment: 'info',
  AwaitingStart: 'warning',
  Started: 'primary',
  Finalized: 'success',
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetClassesOut>(`${config.public.backendUrl}/classes`, {
  credentials: 'include',
  server: false,
})

const columns: TableColumn<ClassItem>[] = [
  {
    accessorKey: 'discipline',
    header: 'Disciplina',
  },
  {
    accessorKey: 'teacher',
    header: 'Professor',
  },
  {
    accessorKey: 'period',
    header: 'Período',
  },
  {
    accessorKey: 'vacancies',
    header: 'Vagas',
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => {
      const s = row.original.status
      return h(UBadge, {
        label: statusLabels[s] ?? s,
        color: statusColors[s] ?? 'neutral',
        variant: 'subtle',
      })
    },
  },
]
</script>

<template>
  <UDashboardPanel id="classes">
    <template #header>
      <UDashboardNavbar title="Turmas">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Turma" @click="() => { createModalOpen = true }" />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-door-open"
            message="Nenhuma turma cadastrada"
            button-label="Turma"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <ClassesCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
