<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface DisciplineItem {
  id: number
  name: string
  code: string
  period: number
  credits: number
  workload: number
  courses: number
  teachers: number
}

interface GetDisciplinesOut {
  total: number
  items: DisciplineItem[]
}

const UButton = resolveComponent('UButton')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const coursesModalOpen = ref(false)
const selectedDiscipline = ref<DisciplineItem | null>(null)

function openEdit(discipline: DisciplineItem) {
  selectedDiscipline.value = discipline
  editModalOpen.value = true
}

function openCourses(discipline: DisciplineItem) {
  selectedDiscipline.value = discipline
  coursesModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetDisciplinesOut>(`${config.public.backendUrl}/disciplines`, {
  credentials: 'include',
  server: false
})

const columns: TableColumn<DisciplineItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'code',
    header: 'Código',
  },
  {
    accessorKey: 'courses',
    header: 'Cursos',
  },
  {
    accessorKey: 'teachers',
    header: 'Professores',
  },
  {
    id: 'actions',
    cell: ({ row }) => h('div', { class: 'flex gap-1' }, [
        h(UButton, {
          icon: 'i-lucide-pencil',
          color: 'neutral',
          variant: 'ghost',
          size: 'sm',
          onClick: () => openEdit(row.original),
        }),
      h(UButton, {
        icon: 'i-lucide-graduation-cap',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: () => openCourses(row.original),
      }),
    ]),
  },
]
</script>

<template>
  <UDashboardPanel id="disciplines">
    <template #header>
      <UDashboardNavbar title="Disciplinas">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Disciplina" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-book-open"
            message="Nenhuma disciplina cadastrada"
            button-label="Disciplina"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <DisciplinesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <DisciplinesEditModal v-model:open="editModalOpen" :discipline="selectedDiscipline" @updated="refresh()" />
  <DisciplinesCoursesModal v-model:open="coursesModalOpen" :discipline="selectedDiscipline" @updated="refresh()" />
</template>
