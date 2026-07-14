<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

interface CourseOfferingItem {
  id: number
  campus: string
  course: string
  courseCurriculum: string
  period: string
  session: string
}

interface GetCourseOfferingsOut {
  total: number
  page: number
  pageSize: number
  items: CourseOfferingItem[]
}

const sessionLabels: Record<string, string> = {
  Morning: 'Matutino',
  Afternoon: 'Vespertino',
  Evening: 'Noturno',
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

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

const { data, status, refresh } = await useFetch<GetCourseOfferingsOut>(`${config.public.backendUrl}/course-offerings`, {
  credentials: 'include',
  server: false,
  query: { page, pageSize }
})

const columns: TableColumn<CourseOfferingItem>[] = [
  {
    accessorKey: 'campus',
    header: 'Campus',
  },
  {
    accessorKey: 'course',
    header: 'Curso',
  },
  {
    accessorKey: 'courseCurriculum',
    header: 'Grade',
  },
  {
    accessorKey: 'period',
    header: 'Período',
  },
  {
    accessorKey: 'session',
    header: 'Turno',
    cell: ({ row }) => sessionLabels[row.original.session] ?? row.original.session,
  },
]
</script>

<template>
  <UDashboardPanel id="course-offerings">
    <template #header>
      <UDashboardNavbar title="Ofertas de Curso">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="data?.items?.length" class="flex justify-end pt-4">
        <UButton icon="i-lucide-plus" label="Oferta" @click="() => { createModalOpen = true }" />
      </div>
      <DataTable :data="data?.items ?? []" :columns="columns" :loading="status === 'pending'">
        <template #empty>
          <TableEmptyState
            :loading="status === 'pending'"
            icon="i-lucide-library"
            message="Nenhuma oferta de curso cadastrada"
            button-label="Oferta"
            @create="createModalOpen = true"
          />
        </template>
      </DataTable>

      <div v-if="(data?.total ?? 0) > 0" class="flex items-center justify-between gap-2 mt-4">
        <UBadge color="neutral" variant="subtle" class="h-8 px-3">
          {{ data?.total }} {{ data?.total === 1 ? 'oferta encontrada' : 'ofertas encontradas' }}
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

  <CourseOfferingsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
