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
            <UIcon name="i-lucide-user-pen" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhum professor cadastrado
            </p>
            <UButton icon="i-lucide-plus" label="Professor" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <TeachersCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
