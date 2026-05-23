<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface CourseItem {
  id: number
  name: string
  type: string
  typeValue: string
}

interface GetCoursesOut {
  total: number
  items: CourseItem[]
}

const UButton = resolveComponent('UButton')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const selectedCourse = ref<CourseItem | null>(null)

function openEdit(course: CourseItem) {
  selectedCourse.value = course
  editModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetCoursesOut>(`${config.public.backendUrl}/courses`, {
  credentials: 'include',
  lazy: true
})

const columns: TableColumn<CourseItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'type',
    header: 'Tipo',
  },
  {
    id: 'actions',
    cell: ({ row }) => h(UButton, {
      icon: 'i-lucide-pencil',
      color: 'neutral',
      variant: 'ghost',
      size: 'sm',
      onClick: () => openEdit(row.original),
    }),
  },
]
</script>

<template>
  <UDashboardPanel id="courses">
    <template #header>
      <UDashboardNavbar title="Cursos">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Curso" @click="createModalOpen = true" />
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
            <UIcon name="i-lucide-graduation-cap" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhum curso cadastrado
            </p>
            <UButton icon="i-lucide-plus" label="Curso" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <CoursesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CoursesEditModal v-model:open="editModalOpen" :course="selectedCourse" @updated="refresh()" />
</template>
