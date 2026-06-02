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
  items: CourseOfferingItem[]
}

const sessionLabels: Record<string, string> = {
  Morning: 'Matutino',
  Afternoon: 'Vespertino',
  Evening: 'Noturno',
}

const config = useRuntimeConfig()
const createModalOpen = ref(false)

const { data, status, refresh } = await useFetch<GetCourseOfferingsOut>(`${config.public.backendUrl}/course-offerings`, {
  credentials: 'include',
  lazy: true
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

        <template #right>
          <UButton icon="i-lucide-plus" label="Oferta" @click="createModalOpen = true" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
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
    </template>
  </UDashboardPanel>

  <CourseOfferingsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
