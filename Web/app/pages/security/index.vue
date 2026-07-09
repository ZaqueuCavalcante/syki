<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface RoleItem {
  id: number
  name: string
  description: string
  permissions: number
}

interface GetRolesOut {
  total: number
  items: RoleItem[]
}

const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const selectedRoleId = ref<number | null>(null)

function openEdit(role: RoleItem) {
  selectedRoleId.value = role.id
  editModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetRolesOut>(`${config.public.backendUrl}/identity/roles`, {
  credentials: 'include',
  server: false
})

const columns: TableColumn<RoleItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'description',
    header: 'Descrição',
  },
  {
    accessorKey: 'permissions',
    header: 'Permissões',
  },
  {
    id: 'actions',
    cell: ({ row }) => h(UTooltip, { text: 'Editar' }, () => h(UButton, {
      icon: 'i-lucide-pencil',
      color: 'neutral',
      variant: 'ghost',
      size: 'sm',
      onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openEdit(row.original) },
    })),
  },
]
</script>

<template>
  <div>
    <div class="flex justify-end mb-4">
      <UButton icon="i-lucide-plus" label="Perfil" @click="createModalOpen = true" />
    </div>

    <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
      <template #empty>
        <TableEmptyState
          :loading="status === 'pending'"
          icon="i-lucide-user-cog"
          message="Nenhum perfil cadastrado"
          button-label="Perfil"
          @create="createModalOpen = true"
        />
      </template>
    </DataTable>
  </div>

  <SecurityRolesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <SecurityRolesEditModal v-model:open="editModalOpen" :role-id="selectedRoleId" @updated="refresh()" />
</template>
