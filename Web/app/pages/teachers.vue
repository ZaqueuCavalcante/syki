<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface TeacherItem {
  id: string
  name: string
  email: string
  campi: number
  disciplines: number
}

interface GetTeachersOut {
  total: number
  items: TeacherItem[]
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetTeachersOut>(`${config.public.backendUrl}/teachers`, {
  credentials: 'include',
  lazy: true
})

const columns: TableColumn<TeacherItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'email',
    header: 'Email',
  },
  {
    accessorKey: 'campi',
    header: 'Campi',
  },
  {
    accessorKey: 'disciplines',
    header: 'Disciplinas',
  },
]
</script>

<template>
  <UDashboardPanel id="teachers">
    <template #header>
      <UDashboardNavbar title="Professores">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton icon="i-lucide-plus" label="Professor" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-user-pen"
            message="Nenhum professor cadastrado"
            button-label="Professor"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>
    </template>
  </UDashboardPanel>

  <TeachersCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
