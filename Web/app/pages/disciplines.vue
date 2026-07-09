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
const UTooltip = resolveComponent('UTooltip')

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
      h(UTooltip, { text: 'Editar' }, () => h(UButton, {
        icon: 'i-lucide-pencil',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openEdit(row.original) },
      })),
      h(UTooltip, { text: 'Cursos' }, () => h(UButton, {
        icon: 'i-lucide-notebook',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openCourses(row.original) },
      })),
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

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Disciplina" @click="createModalOpen = true" />
      </div>
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
