<script setup lang="ts">
import type { ClassLessonItem, ClassStudentItem } from '~/types/classes'

const props = defineProps<{
  lesson: ClassLessonItem | null
  students: ClassStudentItem[]
}>()

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ saved: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const presentStudents = ref<number[]>([])

const isPresent = (studentId: number) => presentStudents.value.includes(studentId)

function toggleStudent(studentId: number, present: boolean) {
  if (present) {
    if (!isPresent(studentId)) presentStudents.value.push(studentId)
  } else {
    presentStudents.value = presentStudents.value.filter(id => id !== studentId)
  }
}

function selectAll() {
  presentStudents.value = props.students.map(s => s.id)
}

function unselectAll() {
  presentStudents.value = []
}

function resetFromLesson() {
  const ids = props.students.map(s => s.id)
  presentStudents.value = (props.lesson?.presentStudents ?? []).filter(id => ids.includes(id))
}

watch(open, (val) => {
  if (val) resetFromLesson()
})

const title = computed(() =>
  props.lesson ? `Chamada · Aula ${props.lesson.number}` : 'Chamada',
)

const description = computed(() =>
  props.lesson
    ? `${formatClassLesson(props.lesson)} — marque os alunos presentes.`
    : 'Marque os alunos presentes.',
)

async function onSubmit() {
  if (!props.lesson) return

  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/teachers/lessons/${props.lesson.id}/attendance`, {
      method: 'PUT',
      body: { presentStudents: presentStudents.value },
      credentials: 'include',
    })
    toast.add({ title: 'Chamada salva com sucesso', color: 'success' })
    open.value = false
    emit('saved')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao salvar a chamada.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    :title="title"
    :description="description"
    :fullscreen="isMobile"
  >
    <template #body>
      <div class="flex flex-col gap-4">
        <div class="flex flex-wrap items-center justify-between gap-2">
          <UBadge
            :label="`${presentStudents.length} / ${students.length} presentes`"
            color="neutral"
            variant="subtle"
            icon="i-lucide-user-check"
          />

          <div class="flex gap-2">
            <UButton
              label="Marcar todos"
              icon="i-lucide-check-check"
              color="neutral"
              variant="subtle"
              size="sm"
              :disabled="!students.length"
              @click="() => { selectAll() }"
            />
            <UButton
              label="Desmarcar todos"
              icon="i-lucide-x"
              color="neutral"
              variant="subtle"
              size="sm"
              :disabled="!students.length"
              @click="() => { unselectAll() }"
            />
          </div>
        </div>

        <div v-if="students.length" class="flex max-h-96 flex-col divide-y divide-default overflow-y-auto">
          <UCheckbox
            v-for="student in students"
            :key="student.id"
            :model-value="isPresent(student.id)"
            :label="student.name"
            class="py-3"
            @update:model-value="(value) => { toggleStudent(student.id, value === true) }"
          />
        </div>
        <div v-else class="flex flex-col items-center gap-3 py-6">
          <UIcon name="i-lucide-users" class="size-10 text-muted" />
          <p class="text-sm text-muted">
            Nenhum aluno matriculado
          </p>
        </div>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="() => { open = false }"
          />
          <UButton
            label="Salvar chamada"
            :loading="loading"
            @click="() => { onSubmit() }"
          />
        </div>
      </div>
    </template>
  </UModal>
</template>
