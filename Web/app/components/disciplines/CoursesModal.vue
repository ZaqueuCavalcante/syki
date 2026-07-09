<script setup lang="ts">
interface DisciplineItem {
  id: number
  name: string
}

interface CourseItem {
  id: number
  name: string
}

interface GetDisciplineOut {
  id: number
  name: string
  code: string
  courses: CourseItem[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ discipline: DisciplineItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const discipline = ref<GetDisciplineOut | null>(null)
const loadingDiscipline = ref(false)

const potentialCourses = ref<CourseItem[]>([])
const searchTerm = ref('')
const selectedCourseIds = ref<number[]>([])
const saving = ref(false)
const removingCourseId = ref<number | null>(null)

async function fetchDiscipline() {
  if (!props.discipline) return
  loadingDiscipline.value = true
  try {
    discipline.value = await $fetch<GetDisciplineOut>(
      `${config.public.backendUrl}/disciplines/${props.discipline.id}`,
      { credentials: 'include' }
    )
  } finally {
    loadingDiscipline.value = false
  }
}

async function fetchPotentialCourses(name: string) {
  if (!props.discipline) return
  const params = name ? `?name=${encodeURIComponent(name)}` : ''
  const result = await $fetch<{ items: CourseItem[] }>(
    `${config.public.backendUrl}/disciplines/${props.discipline.id}/potential-courses${params}`,
    { credentials: 'include' }
  )
  potentialCourses.value = result.items
}

const searchDebounced = useDebounceFn((val: string) => fetchPotentialCourses(val), 300)

watch(searchTerm, val => searchDebounced(val))

async function save() {
  if (!selectedCourseIds.value.length || !props.discipline) return
  saving.value = true
  try {
    await $fetch(`${config.public.backendUrl}/disciplines/courses`, {
      method: 'POST',
      body: { disciplineId: props.discipline.id, courses: selectedCourseIds.value },
      credentials: 'include',
    })
    toast.add({ title: 'Cursos vinculados com sucesso', color: 'success' })
    selectedCourseIds.value = []
    searchTerm.value = ''
    await Promise.all([fetchDiscipline(), fetchPotentialCourses('')])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao vincular cursos.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

async function removeCourse(courseId: number) {
  if (!props.discipline) return
  removingCourseId.value = courseId
  try {
    await $fetch(`${config.public.backendUrl}/disciplines/courses`, {
      method: 'DELETE',
      body: { disciplineId: props.discipline.id, courseId },
      credentials: 'include',
    })
    toast.add({ title: 'Vínculo removido com sucesso', color: 'success' })
    await Promise.all([fetchDiscipline(), fetchPotentialCourses(searchTerm.value)])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao remover vínculo.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    removingCourseId.value = null
  }
}

watch(open, (val) => {
  if (val) {
    fetchDiscipline()
    fetchPotentialCourses('')
  } else {
    discipline.value = null
    potentialCourses.value = []
    selectedCourseIds.value = []
    searchTerm.value = ''
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    :title="`Cursos - ${props.discipline?.name ?? ''}`"
    :fullscreen="isMobile"
    description="Cursos vinculados a esta disciplina."
  >
    <template #body>
      <div class="space-y-6">
        <div class="flex gap-2">
          <USelectMenu
            v-model="selectedCourseIds"
            v-model:search-term="searchTerm"
            :items="potentialCourses"
            label-key="name"
            value-key="id"
            multiple
            ignore-filter
            placeholder="Pesquisar cursos para vincular..."
            class="flex-1"
            searchable
            :search-input="{ placeholder: 'Buscar por nome...' }"
          />
          <UButton
            label="Vincular"
            :disabled="!selectedCourseIds.length"
            :loading="saving"
            @click="save"
          />
        </div>

        <div v-if="loadingDiscipline" class="flex justify-center py-8">
          <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
        </div>

        <div v-else-if="!discipline?.courses.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-notebook" class="size-10" />
          <p class="text-sm">Nenhum curso vinculado</p>
        </div>

        <ul v-else class="divide-y divide-default">
          <li
            v-for="course in discipline?.courses"
            :key="course.id"
            class="flex items-center py-3"
          >
            <span class="flex-1 text-sm">{{ course.name }}</span>
            <UTooltip text="Remover">
              <UButton
                icon="i-lucide-trash-2"
                color="neutral"
                variant="ghost"
                size="xs"
                :loading="removingCourseId === course.id"
                @click="removeCourse(course.id)"
              />
            </UTooltip>
          </li>
        </ul>
      </div>
    </template>
  </UModal>
</template>
