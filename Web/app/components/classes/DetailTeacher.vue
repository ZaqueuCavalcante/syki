<script setup lang="ts">
import type { NavigationMenuItem, TableColumn } from '@nuxt/ui'
import type { InstitutionConfig } from '~/types/configs'
import type { ClassLessonItem, ClassStudentItem, GetTeacherClassActivitiesOut, GetTeacherClassLessonsOut, GetTeacherClassOut, GetTeacherClassStudentsOut } from '~/types/classes'

const UAvatar = resolveComponent('UAvatar')
const UBadge = resolveComponent('UBadge')

const props = defineProps<{ classId: string }>()

const config = useRuntimeConfig()

const breadcrumb = [
  { label: 'Início', to: '/home', icon: 'i-lucide-house' },
  { label: 'Detalhes da turma' },
]

const activeTab = ref('students')

const tabs = computed(() => [[
  { label: 'Alunos', icon: 'i-lucide-users', active: activeTab.value === 'students', onSelect: () => { activeTab.value = 'students' } },
  { label: 'Aulas', icon: 'i-lucide-calendar-days', active: activeTab.value === 'lessons', onSelect: () => { activeTab.value = 'lessons' } },
  { label: 'Atividades', icon: 'i-lucide-clipboard-list', active: activeTab.value === 'activities', onSelect: () => { activeTab.value = 'activities' } },
]] satisfies NavigationMenuItem[][])

// Dados gerais da turma + horários — carregados ao entrar na página.
const { data, status, error } = await useFetch<GetTeacherClassOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

// Cada tab tem endpoint próprio, chamado toda vez que a tab é acessada.
// Alunos é a primeira tab, então carrega ao entrar na página.
const { data: studentsData, status: studentsStatus, refresh: refreshStudents } = await useFetch<GetTeacherClassStudentsOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/students`,
  { credentials: 'include', server: false },
)
const students = computed(() => studentsData.value?.students ?? [])

const { data: institutionConfig } = await useFetch<InstitutionConfig>(
  `${config.public.backendUrl}/institutions/config`,
  { credentials: 'include', server: false },
)

const noteLimit = computed(() => institutionConfig.value?.noteLimit ?? 7)
const frequencyLimit = computed(() => institutionConfig.value?.frequencyLimit ?? 70)

// Colunas estáveis (criadas uma vez). As cores de Nota/Frequência leem os
// limites reativos dentro da própria célula — que roda no render do UTable —
// então recolorem quando o config da instituição chega, sem recriar o array.
const studentColumns: TableColumn<ClassStudentItem>[] = [
  {
    accessorKey: 'name',
    header: 'Aluno',
    cell: ({ row }) => h('div', { class: 'flex items-center gap-2.5' }, [
      h(UAvatar, { alt: row.original.name, size: '2xs' }),
      h('span', { class: 'font-medium text-highlighted' }, row.original.name),
    ]),
  },
  {
    accessorKey: 'averageGrade',
    header: 'Nota média',
    cell: ({ row }) => {
      const grade = row.original.averageGrade
      if (grade == null) return h('span', { class: 'text-muted' }, '—')
      const color = grade < noteLimit.value ? 'text-error' : 'text-success'
      return h('span', { class: `font-medium ${color}` }, grade.toFixed(1).replace('.', ','))
    },
  },
  {
    accessorKey: 'averageAttendance',
    header: 'Frequência média',
    cell: ({ row }) => {
      const attendance = row.original.averageAttendance
      if (attendance == null) return h('span', { class: 'text-muted' }, '—')
      const color = attendance < frequencyLimit.value ? 'text-error' : 'text-success'
      return h('span', { class: `font-medium ${color}` }, `${Math.round(attendance)}%`)
    },
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h(UBadge, {
      label: studentClassStatusLabels[row.original.status] ?? row.original.status,
      color: studentClassStatusColors[row.original.status] ?? 'neutral',
      variant: 'subtle',
    }),
  },
]

const { data: lessonsData, status: lessonsStatus, refresh: refreshLessons } = await useFetch<GetTeacherClassLessonsOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/lessons`,
  { credentials: 'include', server: false, immediate: false },
)
const lessons = computed(() => lessonsData.value?.lessons ?? [])

const { data: activitiesData, status: activitiesStatus, refresh: refreshActivities } = await useFetch<GetTeacherClassActivitiesOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/activities`,
  { credentials: 'include', server: false, immediate: false },
)
const activities = computed(() => activitiesData.value?.activities ?? [])

watch(activeTab, (tab) => {
  if (tab === 'students') refreshStudents()
  else if (tab === 'lessons') refreshLessons()
  else if (tab === 'activities') refreshActivities()
})

// Alunos matriculados usados na chamada — vêm dos dados gerais da turma,
// para a chamada funcionar independente da tab de Alunos ter sido aberta.
const enrolledStudents = computed(() =>
  (data.value?.students ?? []).filter(s => s.status === 'Matriculado'),
)

const createActivityModalOpen = ref(false)

const attendanceModalOpen = ref(false)
const selectedLesson = ref<ClassLessonItem | null>(null)

function openAttendance(lesson: ClassLessonItem) {
  selectedLesson.value = lesson
  attendanceModalOpen.value = true
}
</script>

<template>
  <UDashboardPanel id="class-details">
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
          Turma não encontrada
        </p>
        <UButton icon="i-lucide-arrow-left" label="Voltar" to="/home" />
      </div>

      <div v-else class="flex flex-col gap-6 py-2">
        <div class="grid grid-cols-1 items-start gap-x-4 gap-y-1 sm:grid-cols-[1fr_auto]">
          <h1 class="order-1 text-2xl font-semibold tracking-tight text-highlighted sm:col-start-1 sm:row-start-1">
            {{ data.discipline }}
          </h1>
          <div class="order-2 flex flex-wrap items-center gap-x-6 gap-y-1 text-sm text-muted sm:col-start-1 sm:row-start-2">
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-calendar" class="size-4" />
              {{ data.period }}
            </span>
            <span class="flex items-center gap-1.5">
              <UIcon name="i-lucide-clock" class="size-4" />
              {{ data.workload }}h
            </span>
          </div>
        </div>

        <!-- Horários -->
        <section class="flex flex-col gap-3">
          <div v-if="data.schedules.length" class="flex flex-wrap gap-2">
            <div
              v-for="(s, i) in data.schedules"
              :key="i"
              class="flex flex-col gap-0.5 rounded-lg border border-default bg-elevated/40 px-3 py-2"
            >
              <span
                class="text-sm font-medium"
                :class="s.teacher ? 'text-highlighted' : 'text-muted'"
              >
                {{ s.teacher ?? 'Sem professor' }}
              </span>
              <span class="flex items-center gap-1 text-xs text-muted">
                <UIcon name="i-lucide-clock" class="size-3.5" />
                {{ formatClassSchedule(s) }}
              </span>
              <span
                class="flex items-center gap-1 text-xs"
                :class="s.classroom ? 'text-muted' : 'text-dimmed'"
              >
                <UIcon name="i-lucide-door-open" class="size-3.5" />
                {{ s.classroom ?? 'Sem sala' }}
              </span>
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-clock" class="size-4" />
            Nenhum horário cadastrado
          </div>
        </section>

        <UNavigationMenu :items="tabs" highlight class="-mx-1" />

        <!-- Alunos -->
        <section v-if="activeTab === 'students'" class="flex flex-col gap-3">
          <div v-if="studentsStatus === 'pending'" class="flex justify-center py-8">
            <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
          </div>
          <DataTable v-else :data="students" :columns="studentColumns">
            <template #empty>
              <div class="flex items-center justify-center gap-2 py-6 text-sm text-muted">
                <UIcon name="i-lucide-users" class="size-4" />
                Nenhum aluno matriculado
              </div>
            </template>
          </DataTable>
        </section>

        <!-- Aulas -->
        <section v-else-if="activeTab === 'lessons'" class="flex flex-col gap-3">
          <div v-if="lessonsStatus === 'pending'" class="flex justify-center py-8">
            <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
          </div>
          <template v-else>
            <div v-if="lessons.length" class="flex flex-col divide-y divide-default">
              <div
                v-for="lesson in lessons"
                :key="lesson.id"
                class="flex flex-col gap-2 py-3 sm:flex-row sm:items-center sm:justify-between"
              >
                <div class="flex flex-col gap-1">
                  <span class="text-sm text-highlighted">Aula {{ lesson.number }}</span>
                  <span class="text-xs text-muted">{{ formatClassLesson(lesson) }}</span>
                </div>

                <div class="flex items-center gap-2">
                  <UBadge
                    v-if="lesson.status === 'Finalized'"
                    :label="`${lesson.presentStudents.length} / ${enrolledStudents.length} presentes`"
                    color="neutral"
                    variant="subtle"
                    icon="i-lucide-user-check"
                  />
                  <UBadge
                    :label="classLessonStatusLabels[lesson.status] ?? lesson.status"
                    :color="classLessonStatusColors[lesson.status] ?? 'neutral'"
                    variant="subtle"
                  />
                  <UButton
                    :label="lesson.status === 'Finalized' ? 'Editar chamada' : 'Fazer chamada'"
                    icon="i-lucide-clipboard-check"
                    color="neutral"
                    variant="subtle"
                    size="sm"
                    @click="() => { openAttendance(lesson) }"
                  />
                </div>
              </div>
            </div>
            <div v-else class="flex items-center gap-2 text-sm text-muted">
              <UIcon name="i-lucide-calendar-days" class="size-4" />
              Nenhuma aula cadastrada
            </div>
          </template>
        </section>

        <!-- Atividades -->
        <section v-else-if="activeTab === 'activities'" class="flex flex-col gap-3">
          <div v-if="activitiesStatus === 'pending'" class="flex justify-center py-8">
            <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
          </div>
          <template v-else>
            <div class="flex items-center justify-end gap-2">
              <UButton
                icon="i-lucide-plus"
                label="Atividade"
                size="sm"
                @click="() => { createActivityModalOpen = true }"
              />
            </div>

            <div v-if="activities.length" class="flex flex-col divide-y divide-default">
              <NuxtLink
                v-for="activity in activities"
                :key="activity.id"
                :to="`/classes/${props.classId}/activities/${activity.id}`"
                class="flex flex-col gap-2 py-3 sm:flex-row sm:items-center sm:justify-between hover:bg-elevated/50"
              >
                <div class="flex flex-col gap-1">
                  <span class="text-sm text-highlighted">{{ activity.title }}</span>
                  <span class="text-xs text-muted">
                    {{ classActivityTypeLabels[activity.type] ?? activity.type }}
                    · {{ activity.note }}
                    · Peso {{ activity.weight }}
                    · Entrega até {{ formatClassActivityDueDate(activity.dueDate, activity.dueHour) }}
                  </span>
                </div>

                <div class="flex items-center gap-2">
                  <UBadge
                    :label="`${activity.deliveredWorks} / ${activity.totalWorks} entregas`"
                    color="neutral"
                    variant="subtle"
                    icon="i-lucide-file-check"
                  />
                  <UBadge
                    :label="classActivityStatusLabels[activity.status] ?? activity.status"
                    :color="classActivityStatusColors[activity.status] ?? 'neutral'"
                    variant="subtle"
                  />
                </div>
              </NuxtLink>
            </div>
            <div v-else class="flex flex-col items-center gap-3 py-6">
              <UIcon name="i-lucide-clipboard-list" class="size-10 text-muted" />
              <p class="text-sm text-muted">
                Nenhuma atividade cadastrada
              </p>
            </div>
          </template>
        </section>

        <ClassesCreateActivityModal
          v-model:open="createActivityModalOpen"
          :class-id="data.id"
          @created="refreshActivities()"
        />

        <ClassesLessonAttendanceModal
          v-model:open="attendanceModalOpen"
          :lesson="selectedLesson"
          :students="enrolledStudents"
          @saved="refreshLessons()"
        />
      </div>
    </template>
  </UDashboardPanel>
</template>
