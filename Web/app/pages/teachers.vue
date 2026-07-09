<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface TeacherItem {
  id: number
  name: string
  email: string
  campi: number
  disciplines: number
}

interface GetTeachersOut {
  total: number
  items: TeacherItem[]
}

const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const campiModalOpen = ref(false)
const disciplinesModalOpen = ref(false)
const selectedTeacher = ref<TeacherItem | null>(null)

function openEdit(teacher: TeacherItem) {
  selectedTeacher.value = teacher
  editModalOpen.value = true
}

function openCampi(teacher: TeacherItem) {
  selectedTeacher.value = teacher
  campiModalOpen.value = true
}

function openDisciplines(teacher: TeacherItem) {
  selectedTeacher.value = teacher
  disciplinesModalOpen.value = true
}

const { data, status, refresh } = await useFetch<GetTeachersOut>(`${config.public.backendUrl}/teachers`, {
  credentials: 'include',
  server: false
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
  {
    id: 'actions',
    cell: ({ row }) => h('div', { class: 'flex gap-1' }, [
      h(UTooltip, { text: 'Editar' }, () => h(UButton, {
        icon: 'i-lucide-pencil',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openEdit(row.original) },
      })),
      h(UTooltip, { text: 'Campi' }, () => h(UButton, {
        icon: 'i-lucide-map-pin',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openCampi(row.original) },
      })),
      h(UTooltip, { text: 'Disciplinas' }, () => h(UButton, {
        icon: 'i-lucide-book-open',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openDisciplines(row.original) },
      })),
    ]),
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

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Professor" @click="createModalOpen = true" />
      </div>
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
  <TeachersEditModal v-model:open="editModalOpen" :teacher="selectedTeacher" @updated="refresh()" />
  <TeachersCampiModal v-model:open="campiModalOpen" :teacher="selectedTeacher" @updated="refresh()" />
  <TeachersDisciplinesModal v-model:open="disciplinesModalOpen" :teacher="selectedTeacher" @updated="refresh()" />
</template>
