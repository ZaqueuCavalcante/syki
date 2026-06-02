<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface StudentItem {
  id: string
  name: string
  email: string
  enrollmentCode: string
  courseOffering: string
}

interface GetStudentsOut {
  total: number
  items: StudentItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetStudentsOut>(`${config.public.backendUrl}/students`, {
  credentials: 'include',
  lazy: true
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
    accessorKey: 'courseOffering',
    header: 'Oferta de Curso',
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
</template>
