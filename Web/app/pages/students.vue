<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface StudentItem {
  id: number
  name: string
  email: string
  enrollmentCode: string
  activeEnrollments: number
  currentCourseOfferingId: number | null
}

interface GetStudentsOut {
  total: number
  items: StudentItem[]
}

const UButton = resolveComponent('UButton')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const courseOfferingModalOpen = ref(false)
const selectedStudent = ref<StudentItem | null>(null)

function openCourseOffering(student: StudentItem) {
  selectedStudent.value = student
  courseOfferingModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetStudentsOut>(`${config.public.backendUrl}/students`, {
  credentials: 'include',
  server: false
})

const columns: TableColumn<StudentItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'email',
    header: 'Email',
  },
  {
    accessorKey: 'enrollmentCode',
    header: 'Matrícula',
  },
  {
    accessorKey: 'course',
    header: 'Curso',
  },
  {
    id: 'actions',
    cell: ({ row }) => h(UButton, {
      icon: 'i-lucide-library',
      color: 'neutral',
      variant: 'ghost',
      size: 'sm',
      onClick: () => openCourseOffering(row.original),
    }),
  },
]
</script>

<template>
  <UDashboardPanel id="students">
    <template #header>
      <UDashboardNavbar title="Alunos">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Aluno" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-user-round"
            message="Nenhum aluno cadastrado"
            button-label="Aluno"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <StudentsCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <StudentsCourseOfferingModal v-model:open="courseOfferingModalOpen" :student="selectedStudent" @enrolled="refresh()" />
</template>
