<script setup lang="ts">
import type { ClassLessonItem, GetTeacherClassActivitiesOut, GetTeacherClassLessonsOut, GetTeacherClassOut } from '~/types/classes'

const props = defineProps<{ classId: string }>()

const config = useRuntimeConfig()

const breadcrumb = [
  { label: 'Início', to: '/home', icon: 'i-lucide-house' },
  { label: 'Detalhes da turma' },
]

const { data, status, error } = await useFetch<GetTeacherClassOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}`,
  { credentials: 'include', server: false },
)

const { data: activitiesData, refresh: refreshActivities } = await useFetch<GetTeacherClassActivitiesOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/activities`,
  { credentials: 'include', server: false },
)

const activities = computed(() => activitiesData.value?.activities ?? [])

const { data: lessonsData, refresh: refreshLessons } = await useFetch<GetTeacherClassLessonsOut>(
  `${config.public.backendUrl}/teachers/classes/${props.classId}/lessons`,
  { credentials: 'include', server: false },
)

const lessons = computed(() => lessonsData.value?.lessons ?? [])

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

      <div v-else class="flex flex-col gap-10 py-2">
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
            <UBadge
              :label="classStatusLabels[data.status] ?? data.status"
              :color="classStatusColors[data.status] ?? 'neutral'"
              variant="subtle"
            />
          </div>
        </div>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Horários
          </h2>

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
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-clock" class="size-4" />
            Nenhum horário cadastrado
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Alunos ({{ data.students.length }} / {{ data.vacancies }})
          </h2>

          <div v-if="data.students.length" class="flex flex-col divide-y divide-default">
            <div
              v-for="student in data.students"
              :key="student.id"
              class="flex items-center justify-between py-3"
            >
              <div class="flex items-center gap-2.5">
                <UAvatar :alt="student.name" size="2xs" />
                <span class="text-sm font-medium text-highlighted">{{ student.name }}</span>
              </div>
              <UBadge
                :label="studentClassStatusLabels[student.status] ?? student.status"
                :color="studentClassStatusColors[student.status] ?? 'neutral'"
                variant="subtle"
              />
            </div>
          </div>
          <div v-else class="flex items-center gap-2 text-sm text-muted">
            <UIcon name="i-lucide-users" class="size-4" />
            Nenhum aluno matriculado
          </div>
        </section>

        <section class="flex flex-col gap-3">
          <h2 class="font-semibold text-highlighted">
            Aulas ({{ lessons.length }})
          </h2>

          <div v-if="lessons.length" class="flex max-h-[32rem] flex-col divide-y divide-default overflow-y-auto">
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
        </section>

        <section class="flex flex-col gap-3">
          <div class="flex items-center justify-between gap-2">
            <h2 class="font-semibold text-highlighted">
              Atividades ({{ activities.length }})
            </h2>
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
