<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import type { GetParentDetailsOut, ParentStudentItem } from '~/types/parents'

const UAvatar = resolveComponent('UAvatar')
const UBadge = resolveComponent('UBadge')
const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const props = defineProps<{ parentId: string }>()

const breadcrumb = [
  { label: 'Responsáveis', to: '/parents', icon: 'i-lucide-users' },
  { label: 'Detalhes do responsável' },
]

const config = useRuntimeConfig()

const { data, status, error } = await useFetch<GetParentDetailsOut>(
  `${config.public.backendUrl}/parents/${props.parentId}/details`,
  { credentials: 'include', server: false },
)

const activeLinks = computed(() =>
  data.value?.students.filter(s => s.linkStatus === 'Active' && !s.revokedByStudent).length ?? 0,
)

const studentColumns: TableColumn<ParentStudentItem>[] = [
  {
    accessorKey: 'name',
    header: 'Aluno',
    cell: ({ row }) => h('div', { class: 'flex items-center gap-2.5' }, [
      h(UAvatar, { alt: row.original.name, size: '2xs' }),
      h('div', { class: 'flex flex-col' }, [
        h('span', { class: 'font-medium text-highlighted' }, row.original.name),
        h('span', { class: 'text-xs text-muted' }, row.original.enrollmentCode),
      ]),
    ]),
  },
  {
    accessorKey: 'relationship',
    header: 'Parentesco',
    cell: ({ row }) => relationshipLabel(row.original.relationship),
  },
  {
    accessorKey: 'course',
    header: 'Curso',
    cell: ({ row }) => {
      const course = row.original.course
      if (!course) return h('span', { class: 'text-muted' }, '—')
      return h('div', { class: 'flex flex-col' }, [
        h('span', { class: 'text-highlighted' }, course),
        h('span', { class: 'text-xs text-muted' }, [row.original.campus, row.original.period].filter(Boolean).join(' · ')),
      ])
    },
  },
  {
    accessorKey: 'status',
    header: 'Situação',
    cell: ({ row }) => h(UBadge, {
      label: studentStatusLabels[row.original.status] ?? row.original.status,
      color: studentStatusColors[row.original.status] ?? 'neutral',
      variant: 'subtle',
    }),
  },
  {
    accessorKey: 'linkStatus',
    header: 'Vínculo',
    cell: ({ row }) => {
      if (row.original.revokedByStudent) {
        return h(UTooltip, { text: 'O aluno revogou o acesso do responsável aos seus dados' }, () => h(UBadge, {
          label: 'Revogado pelo aluno',
          color: 'error',
          variant: 'subtle',
        }))
      }
      return h(UBadge, {
        label: parentStudentStatusLabels[row.original.linkStatus] ?? row.original.linkStatus,
        color: parentStudentStatusColors[row.original.linkStatus] ?? 'neutral',
        variant: 'subtle',
      })
    },
  },
  {
    id: 'actions',
    header: '',
    cell: ({ row }) => h('div', { class: 'flex justify-end' }, h(UTooltip, { text: 'Ver aluno' }, () => h(UButton, {
      icon: 'i-lucide-arrow-right',
      color: 'neutral',
      variant: 'ghost',
      to: `/students/${row.original.id}`,
      'aria-label': 'Ver aluno',
    }))),
  },
]
</script>

<template>
  <UDashboardPanel id="parent-details">
    <template #header>
      <UDashboardNavbar>
        <template #title>
          <UBreadcrumb :items="breadcrumb" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div v-if="status === 'pending'" class="flex justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-8 animate-spin text-muted" />
      </div>

      <div v-else-if="error || !data" class="flex flex-col items-center gap-4 py-12">
        <UIcon name="i-lucide-triangle-alert" class="size-16 text-muted" />
        <p class="text-muted text-sm">
          Responsável não encontrado
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/parents" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="flex items-start gap-4">
          <UAvatar :alt="data.name" size="xl" />
          <div class="flex flex-col gap-1">
            <div class="flex flex-wrap items-center gap-2">
              <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
                {{ data.name }}
              </h1>
              <UBadge
                :label="`${data.students.length} ${data.students.length === 1 ? 'aluno' : 'alunos'}`"
                color="neutral"
                variant="subtle"
              />
            </div>
            <div class="flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted">
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-mail" class="size-4" />
                {{ data.email }}
              </span>
              <span v-if="data.phoneNumber" class="flex items-center gap-1.5">
                <UIcon name="i-lucide-phone" class="size-4" />
                {{ data.phoneNumber }}
              </span>
              <span class="flex items-center gap-1.5">
                <UIcon name="i-lucide-calendar" class="size-4" />
                Desde {{ new Date(data.createdAt).toLocaleDateString('pt-BR') }}
              </span>
            </div>
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-2">
            <h2 class="font-semibold text-highlighted">
              Alunos vinculados
            </h2>
            <UBadge
              v-if="data.students.length"
              :label="`${activeLinks} ${activeLinks === 1 ? 'vínculo ativo' : 'vínculos ativos'}`"
              color="neutral"
              variant="subtle"
            />
          </div>

          <DataTable :data="data.students" :columns="studentColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-graduation-cap" class="size-4" />
                Nenhum aluno vinculado a este responsável
              </div>
            </template>
          </DataTable>
        </section>
      </div>
    </template>
  </UDashboardPanel>
</template>
