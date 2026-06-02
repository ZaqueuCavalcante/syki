<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface CourseCurriculumItem {
  id: number
  name: string
  course: string
  disciplines: number
}

interface GetCourseCurriculumsOut {
  total: number
  items: CourseCurriculumItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetCourseCurriculumsOut>(`${config.public.backendUrl}/course-curriculums`, {
  credentials: 'include',
  lazy: true
})

const columns: TableColumn<CourseCurriculumItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'course',
    header: 'Curso',
  },
  {
    accessorKey: 'disciplines',
    header: 'Disciplinas',
  },
]
</script>

<template>
  <UDashboardPanel id="course-curriculums">
    <template #header>
      <UDashboardNavbar title="Grades Curriculares">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Grade" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-layout-list"
            message="Nenhuma grade curricular cadastrada"
            button-label="Grade"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <CourseCurriculumsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
