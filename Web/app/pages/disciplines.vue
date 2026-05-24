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
  lazy: true
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
      <UTable
        :data="data?.items"
        :columns="columns"
        :loading="status === 'pending'"
        :ui="{
          base: 'table-fixed border-separate border-spacing-0',
          thead: '[&>tr]:bg-elevated/50 [&>tr]:after:content-none',
          tbody: '[&>tr]:last:[&>td]:border-b-0',
          th: 'py-2 first:rounded-l-lg last:rounded-r-lg border-y border-default first:border-l last:border-r',
          td: 'border-b border-default',
        }"
      >
        <template #empty>
          <div v-if="status !== 'pending'" class="flex flex-col items-center gap-4 py-12">
            <UIcon name="i-lucide-book-open" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhuma disciplina cadastrada
            </p>
            <UButton icon="i-lucide-plus" label="Disciplina" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <DisciplinesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <DisciplinesEditModal v-model:open="editModalOpen" :discipline="selectedDiscipline" @updated="refresh()" />
  <DisciplinesCoursesModal v-model:open="coursesModalOpen" :discipline="selectedDiscipline" />
</template>
