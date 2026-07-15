<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface ClassroomItem {
  id: number
  name: string
  campusId: number
  campus: string
  capacity: number
}

const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const selectedClassroom = ref<ClassroomItem | null>(null)

function openEdit(classroom: ClassroomItem) {
  selectedClassroom.value = classroom
  editModalOpen.value = true
}

const { data, status, refresh } = await useFetch<ClassroomItem[]>(`${config.public.backendUrl}/classrooms`, {
  credentials: 'include',
  server: false,
})

const columns: TableColumn<ClassroomItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'campus',
    header: 'Campus',
  },
  {
    accessorKey: 'capacity',
    header: 'Capacidade',
  },
  {
    id: 'actions',
    cell: ({ row }) => h('div', { class: 'flex justify-end gap-1' }, [
      h(UTooltip, { text: 'Editar' }, () => h(UButton, {
        icon: 'i-lucide-pencil',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openEdit(row.original) },
      })),
      h(UTooltip, { text: 'Ver detalhes' }, () => h(UButton, {
        icon: 'i-lucide-arrow-right',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        to: `/classrooms/${row.original.id}`,
        'aria-label': 'Ver detalhes',
      })),
    ]),
  },
]
</script>

<template>
  <UDashboardPanel id="classrooms">
    <template #header>
      <UDashboardNavbar title="Salas">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="flex items-center justify-end gap-2 pt-4">
        <UButton
          v-if="data?.length"
          icon="i-lucide-plus"
          label="Sala"
          @click="() => { createModalOpen = true }"
        />
      </div>
      <DataTable :data="data ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-school"
            message="Nenhuma sala cadastrada"
            button-label="Sala"
            @create="() => { createModalOpen = true }"
          />
        </template>
      </DataTable>

      <div v-if="(data?.length ?? 0) > 0" class="flex items-center justify-between gap-2 mt-4">
        <UBadge color="neutral" variant="subtle" class="h-8 px-3">
          {{ data?.length }} {{ data?.length === 1 ? 'sala encontrada' : 'salas encontradas' }}
        </UBadge>
      </div>
    </template>
  </UDashboardPanel>

  <ClassroomsCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <ClassroomsEditModal v-model:open="editModalOpen" :classroom="selectedClassroom" @updated="refresh()" />
</template>
