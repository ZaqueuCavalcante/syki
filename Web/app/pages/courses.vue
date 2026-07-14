<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface CourseItem {
  id: number
  name: string
  type: string
  typeValue: string
  disciplines: number
}

interface GetCoursesOut {
  total: number
  page: number
  pageSize: number
  items: CourseItem[]
}

const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const disciplinesModalOpen = ref(false)
const selectedCourse = ref<CourseItem | null>(null)

function openEdit(course: CourseItem) {
  selectedCourse.value = course
  editModalOpen.value = true
}

function openDisciplines(course: CourseItem) {
  selectedCourse.value = course
  disciplinesModalOpen.value = true
}

const route = useRoute()
const router = useRouter()

const pageSize = 10
const page = ref(Number(route.query.page) || 1)

// Sync page to URL
watch(page, () => {
  const query: Record<string, string> = {}
  if (page.value > 1) query.page = String(page.value)
  router.replace({ query })
}, { flush: 'post' })

const { data, status, refresh } = await useFetch<GetCoursesOut>(`${config.public.backendUrl}/courses`, {
  credentials: 'include',
  server: false,
  query: { page, pageSize }
})

const columns: TableColumn<CourseItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'type',
    header: 'Tipo',
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
  <UDashboardPanel id="courses">
    <template #header>
      <UDashboardNavbar title="Cursos">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Curso" @click="() => { createModalOpen = true }" />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-notebook"
            message="Nenhum curso cadastrado"
            button-label="Curso"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>

      <div v-if="(data?.total ?? 0) > 0" class="flex items-center justify-between gap-2 mt-4">
        <UBadge color="neutral" variant="subtle" class="h-8 px-3">
          {{ data?.total }} {{ data?.total === 1 ? 'curso encontrado' : 'cursos encontrados' }}
        </UBadge>

        <UPagination
          v-if="(data?.total ?? 0) > pageSize"
          v-model:page="page"
          :items-per-page="pageSize"
          :total="data?.total ?? 0"
        />
      </div>
    </template>
  </UDashboardPanel>

  <CoursesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <CoursesEditModal v-model:open="editModalOpen" :course="selectedCourse" @updated="refresh()" />
  <CoursesDisciplinesModal v-model:open="disciplinesModalOpen" :course="selectedCourse" @updated="refresh()" />
</template>
