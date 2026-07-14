<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import { useDebounceFn } from '@vueuse/core'

interface DisciplineItem {
  id: number
  name: string
  code: string
  period: number
  credits: number
  workload: number
  courses: number
  teachers: number
}

interface GetDisciplinesOut {
  total: number
  page: number
  pageSize: number
  items: DisciplineItem[]
}

const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)
const coursesModalOpen = ref(false)
const selectedDiscipline = ref<DisciplineItem | null>(null)

function openEdit(discipline: DisciplineItem) {
  selectedDiscipline.value = discipline
  editModalOpen.value = true
}

function openCourses(discipline: DisciplineItem) {
  selectedDiscipline.value = discipline
  coursesModalOpen.value = true
}

const route = useRoute()
const router = useRouter()

const filter = ref((route.query.filter as string) || '')

// The filter actually applied to the fetch. Typing updates it debounced;
// clearing updates it immediately so the reload feels instant.
const appliedFilter = ref(filter.value)
const applyFilter = useDebounceFn((value: string) => { appliedFilter.value = value }, 300)
watch(filter, (value) => { applyFilter(value) })

const pageSize = 10
const page = ref(Number(route.query.page) || 1)

// Sync filter and page to URL
watch([filter, page], () => {
  const query: Record<string, string> = {}
  if (filter.value) query.filter = filter.value
  if (page.value > 1) query.page = String(page.value)
  router.replace({ query })
}, { flush: 'post' })

// A new search starts over from the first page
watch(appliedFilter, () => { page.value = 1 })

// Reflects the filters actually applied to the data being shown
const hasFilters = computed(() => appliedFilter.value.length > 0)

function clearFilters() {
  filter.value = ''
  appliedFilter.value = ''
}

const { data, status, refresh } = await useFetch<GetDisciplinesOut>(`${config.public.backendUrl}/disciplines`, {
  credentials: 'include',
  server: false,
  query: { filter: appliedFilter, page, pageSize }
})

const columns: TableColumn<DisciplineItem>[] = [
  {
    accessorKey: 'name',
    header: 'Nome',
  },
  {
    accessorKey: 'code',
    header: 'Código',
  },
  {
    accessorKey: 'courses',
    header: 'Cursos',
  },
  {
    accessorKey: 'teachers',
    header: 'Professores',
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
      h(UTooltip, { text: 'Cursos' }, () => h(UButton, {
        icon: 'i-lucide-notebook',
        color: 'neutral',
        variant: 'ghost',
        size: 'sm',
        onClick: (e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); openCourses(row.original) },
      })),
    ]),
  },
]
</script>

<template>
  <UDashboardPanel id="disciplines">
    <template #header>
      <UDashboardNavbar title="Disciplinas">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="flex items-center justify-between gap-2 pt-4">
        <UInput
          v-model="filter"
          class="max-w-sm"
          icon="i-lucide-search"
          placeholder="Buscar por nome ou código..."
          :loading="status === 'pending'"
        >
          <template v-if="filter" #trailing>
            <UButton
              icon="i-lucide-x"
              color="neutral"
              variant="link"
              size="sm"
              aria-label="Remover filtro"
              @click="clearFilters"
            />
          </template>
        </UInput>
        <UButton
          v-if="data?.items?.length || filter"
          icon="i-lucide-plus"
          label="Disciplina"
          @click="() => { createModalOpen = true }"
        />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-book-open"
            message="Nenhuma disciplina cadastrada"
            button-label="Disciplina"
            :filtered="hasFilters"
            not-found-message="Nenhuma disciplina encontrada com os filtros aplicados"
            @create="() => { createModalOpen = true }"
            @clear-filters="clearFilters"
          />
        </template>
      </DataTable>

      <div v-if="(data?.total ?? 0) > 0" class="flex items-center justify-between gap-2 mt-4">
        <UBadge color="neutral" variant="subtle" class="h-8 px-3">
          {{ data?.total }} {{ data?.total === 1 ? 'disciplina encontrada' : 'disciplinas encontradas' }}
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

  <DisciplinesCreateModal v-model:open="createModalOpen" @created="refresh()" />
  <DisciplinesEditModal v-model:open="editModalOpen" :discipline="selectedDiscipline" @updated="refresh()" />
  <DisciplinesCoursesModal v-model:open="coursesModalOpen" :discipline="selectedDiscipline" @updated="refresh()" />
</template>
