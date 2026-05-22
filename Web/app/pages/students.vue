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
            <UIcon name="i-lucide-user-round" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhum aluno cadastrado
            </p>
            <UButton icon="i-lucide-plus" label="Aluno" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <StudentsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
