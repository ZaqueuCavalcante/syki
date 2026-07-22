<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

const UBadge = resolveComponent('UBadge')
const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

interface ClassItem {
  id: number
  discipline: string
  teachers: string[]
  period: string
  vacancies: number
  status: string
}

interface GetClassesOut {
  total: number
  page: number
  pageSize: number
  items: ClassItem[]
}

const statusLabels: Record<string, string> = {
  OnPreEnrollment: 'Pré-matrícula',
  OnEnrollment: 'Matrícula',
  OnReview: 'Revisão',
  Started: 'Iniciada',
  Finalized: 'Finalizada',
}

const statusColors: Record<string, 'neutral' | 'primary' | 'success' | 'warning' | 'error' | 'info'> = {
  OnPreEnrollment: 'neutral',
  OnEnrollment: 'info',
  OnReview: 'warning',
  Started: 'primary',
  Finalized: 'success',
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const route = useRoute()
const router = useRouter()

const pageSize = 10
const page = ref(Number(route.query.page) || 1)

// Sync page to URL
watch(page, () => {
  const query: Record<string, string> = {}
  if (page.value > 1) query.page = String(page.value)
  router.replace({ query })
}, { flush: 'post' })

const { data, status, refresh } = await useFetch<GetClassesOut>(`${config.public.backendUrl}/classes`, {
  credentials: 'include',
  server: false,
  query: { page, pageSize },
})

const columns: TableColumn<ClassItem>[] = [
  {
    accessorKey: 'discipline',
    header: 'Disciplina',
  },
  {
    accessorKey: 'teachers',
    header: 'Professores',
    cell: ({ row }) => row.original.teachers.join(', ') || '—',
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
  {
    id: 'actions',
    header: '',
    cell: ({ row }) => h('div', { class: 'flex justify-end' }, h(UTooltip, { text: 'Ver detalhes' }, () => h(UButton, {
      icon: 'i-lucide-arrow-right',
      color: 'neutral',
      variant: 'ghost',
      to: `/classes/${row.original.id}`,
      'aria-label': 'Ver detalhes',
    }))),
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

      <div v-if="(data?.total ?? 0) > 0" class="flex items-center justify-between gap-2 mt-4">
        <UBadge color="neutral" variant="subtle" class="h-8 px-3">
          {{ data?.total }} {{ data?.total === 1 ? 'turma encontrada' : 'turmas encontradas' }}
        </UBadge>

        <UPagination
          v-if="(data?.total ?? 0) > pageSize"
          v-model:page="page"
          :items-per-page="pageSize"
          :total="data?.total ?? 0"
        />
      </div>
    </template>
  </UDashboardPanel>

  <ClassesCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
