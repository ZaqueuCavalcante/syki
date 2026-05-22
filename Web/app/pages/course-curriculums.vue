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

const { data, status, refresh } = await useFetch<GetCourseCurriculumsOut>(`${config.public.backendUrl}/courses/course-curriculums`, {
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
            <UIcon name="i-lucide-layout-list" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhuma grade curricular cadastrada
            </p>
            <UButton icon="i-lucide-plus" label="Grade" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>
</template>
