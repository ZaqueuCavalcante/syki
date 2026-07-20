<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'
import type { GetTeacherDetailsOut, TeacherClassItem } from '~/types/teachers'

const UBadge = resolveComponent('UBadge')
const UButton = resolveComponent('UButton')
const UTooltip = resolveComponent('UTooltip')

const props = defineProps<{ teacherId: string }>()

const breadcrumb = [
  { label: 'Professores', to: '/teachers', icon: 'i-lucide-user-pen' },
  { label: 'Detalhes do professor' },
]

const config = useRuntimeConfig()

const { data, status, error, refresh } = await useFetch<GetTeacherDetailsOut>(
  `${config.public.backendUrl}/teachers/${props.teacherId}/details`,
  { credentials: 'include', server: false },
)

const editModalOpen = ref(false)
const campiModalOpen = ref(false)
const disciplinesModalOpen = ref(false)

// Os modais de edição esperam o mesmo formato usado na listagem de professores
const teacherRef = computed(() => data.value
  ? { id: data.value.id, name: data.value.name, email: data.value.email }
  : null,
)

const classes = computed(() => data.value?.classes ?? [])

const totalVacancies = computed(() => classes.value.reduce((sum, c) => sum + c.vacancies, 0))
const totalStudents = computed(() => classes.value.reduce((sum, c) => sum + c.students, 0))
const occupancyPercent = computed(() =>
  totalVacancies.value > 0 ? Math.round((totalStudents.value / totalVacancies.value) * 100) : 0,
)
const occupancyRingClass = computed(() => {
  if (occupancyPercent.value < 50) return 'text-error'
  if (occupancyPercent.value <= 70) return 'text-warning'
  return 'text-success'
})

const ongoingClasses = computed(() => classes.value.filter(c => c.status === 'Started').length)
const ongoingPercent = computed(() =>
  classes.value.length > 0 ? Math.round((ongoingClasses.value / classes.value.length) * 100) : 0,
)

// Turmas em que o professor ainda não tem nenhum horário atribuído
const classesWithSchedules = computed(() => classes.value.filter(c => c.schedules.length > 0).length)
const schedulesPercent = computed(() =>
  classes.value.length > 0 ? Math.round((classesWithSchedules.value / classes.value.length) * 100) : 0,
)
const schedulesRingClass = computed(() =>
  schedulesPercent.value === 100 ? 'text-success' : 'text-warning',
)

const classColumns: TableColumn<TeacherClassItem>[] = [
  {
    accessorKey: 'discipline',
    header: 'Disciplina',
    cell: ({ row }) => h('span', { class: 'font-medium text-highlighted' }, row.original.discipline),
  },
  {
    accessorKey: 'period',
    header: 'Período',
  },
  {
    accessorKey: 'schedules',
    header: 'Horários',
    cell: ({ row }) => {
      const schedules = row.original.schedules
      if (!schedules.length) return h('span', { class: 'text-muted' }, 'Sem horário')
      return h('div', { class: 'flex flex-col gap-0.5' }, schedules.map(s =>
        h('span', { class: 'text-xs text-muted' }, formatClassSchedule(s)),
      ))
    },
  },
  {
    accessorKey: 'students',
    header: 'Alunos',
    cell: ({ row }) => `${row.original.students} / ${row.original.vacancies}`,
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h(UBadge, {
      label: classStatusLabels[row.original.status] ?? row.original.status,
      color: classStatusColors[row.original.status] ?? 'neutral',
      variant: 'subtle',
    }),
  },
  {
    id: 'actions',
    header: '',
    cell: ({ row }) => h('div', { class: 'flex justify-end' }, h(UTooltip, { text: 'Ver turma' }, () => h(UButton, {
      icon: 'i-lucide-arrow-right',
      color: 'neutral',
      variant: 'ghost',
      to: `/classes/${row.original.id}`,
      'aria-label': 'Ver turma',
    }))),
  },
]
</script>

<template>
  <UDashboardPanel id="teacher-details">
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
          Professor não encontrado
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/teachers" />
      </div>

      <div v-else class="flex flex-col gap-10 py-2">
        <div class="flex items-start gap-4">
          <UAvatar :alt="data.name" size="xl" />
          <div class="flex flex-col gap-1">
            <div class="flex items-center gap-1.5">
              <h1 class="text-2xl font-semibold tracking-tight text-highlighted">
                {{ data.name }}
              </h1>
              <UTooltip text="Editar">
                <UButton
                  icon="i-lucide-pencil"
                  color="neutral"
                  variant="ghost"
                  size="xs"
                  @click="(e) => { (e.currentTarget as HTMLElement).blur(); editModalOpen = true }"
                />
              </UTooltip>
            </div>
            <span class="flex items-center gap-1.5 text-sm text-muted">
              <UIcon name="i-lucide-mail" class="size-4" />
              {{ data.email }}
            </span>
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-1.5">
            <h2 class="font-semibold text-highlighted">
              Campus
            </h2>
            <UTooltip text="Editar">
              <UButton
                icon="i-lucide-pencil"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); campiModalOpen = true }"
              />
            </UTooltip>
          </div>

          <div v-if="data.campi.length" class="flex flex-wrap gap-2">
            <div
              v-for="campus in data.campi"
              :key="campus.id"
              class="flex items-center gap-1.5 rounded-full border border-default bg-elevated/40 px-3 py-1"
            >
              <UIcon name="i-lucide-map-pin" class="size-3.5 text-muted" />
              <span class="text-sm text-highlighted">{{ campus.name }}</span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-map-pin-off" class="size-4" />
            Nenhum campus vinculado
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <div class="flex items-center gap-1.5">
            <h2 class="font-semibold text-highlighted">
              Disciplinas
            </h2>
            <UTooltip text="Editar">
              <UButton
                icon="i-lucide-pencil"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="(e) => { (e.currentTarget as HTMLElement).blur(); disciplinesModalOpen = true }"
              />
            </UTooltip>
          </div>

          <div v-if="data.disciplines.length" class="flex flex-wrap gap-2">
            <div
              v-for="discipline in data.disciplines"
              :key="discipline.id"
              class="flex items-center gap-1.5 rounded-full border border-default bg-elevated/40 px-3 py-1"
            >
              <UIcon name="i-lucide-book-open" class="size-3.5 text-muted" />
              <span class="text-sm text-highlighted">{{ discipline.name }}</span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-book-dashed" class="size-4" />
            Nenhuma disciplina vinculada
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Turmas
          </h2>

          <div v-if="classes.length" class="mb-2 grid grid-cols-1 gap-4 sm:grid-cols-3">
            <ClassesRingStat
              :percent="occupancyPercent"
              :center-text="`${occupancyPercent}%`"
              :title="`${totalStudents} / ${totalVacancies} vagas`"
              subtitle="ocupação das turmas"
              :color-class="occupancyRingClass"
            />
            <ClassesRingStat
              :percent="ongoingPercent"
              :center-text="`${ongoingClasses}`"
              :title="`${ongoingClasses} de ${classes.length} turmas`"
              subtitle="em andamento"
              color-class="text-primary"
            />
            <ClassesRingStat
              :percent="schedulesPercent"
              :center-text="`${schedulesPercent}%`"
              :title="`${classesWithSchedules} de ${classes.length} turmas`"
              subtitle="com horário definido"
              :color-class="schedulesRingClass"
            />
          </div>

          <DataTable :data="classes" :columns="classColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-door-closed" class="size-4" />
                Nenhuma turma atribuída
              </div>
            </template>
          </DataTable>
        </section>

        <TeachersEditModal
          v-model:open="editModalOpen"
          :teacher="teacherRef"
          @updated="refresh()"
        />

        <TeachersCampiModal
          v-model:open="campiModalOpen"
          :teacher="teacherRef"
          @updated="refresh()"
        />

        <TeachersDisciplinesModal
          v-model:open="disciplinesModalOpen"
          :teacher="teacherRef"
          @updated="refresh()"
        />
      </div>
    </template>
  </UDashboardPanel>
</template>
