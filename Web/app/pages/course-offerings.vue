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
            <UIcon name="i-lucide-library" class="size-16 text-muted" />
            <p class="text-muted text-sm">
              Nenhuma oferta de curso cadastrada
            </p>
            <UButton icon="i-lucide-plus" label="Oferta" @click="createModalOpen = true" />
          </div>
        </template>
      </UTable>
    </template>
  </UDashboardPanel>

  <CourseOfferingsCreateModal v-model:open="createModalOpen" @created="refresh()" />
</template>
