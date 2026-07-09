<script setup lang="ts">
interface CourseItem {
  id: number
  name: string
}

interface DisciplineItem {
  id: number
  name: string
}

interface GetCourseOut {
  id: number
  name: string
  type: string
  disciplines: DisciplineItem[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ course: CourseItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const course = ref<GetCourseOut | null>(null)
const loadingCourse = ref(false)

const potentialDisciplines = ref<DisciplineItem[]>([])
const searchTerm = ref('')
const selectedDisciplineIds = ref<number[]>([])
const saving = ref(false)
const removingDisciplineId = ref<number | null>(null)

async function fetchCourse() {
  if (!props.course) return
  loadingCourse.value = true
  try {
    course.value = await $fetch<GetCourseOut>(
      `${config.public.backendUrl}/courses/${props.course.id}`,
      { credentials: 'include' }
    )
  } finally {
    loadingCourse.value = false
  }
}

async function fetchPotentialDisciplines(name: string) {
  if (!props.course) return
  const params = name ? `?name=${encodeURIComponent(name)}` : ''
  const result = await $fetch<{ items: DisciplineItem[] }>(
    `${config.public.backendUrl}/courses/${props.course.id}/potential-disciplines${params}`,
    { credentials: 'include' }
  )
  potentialDisciplines.value = result.items
}

const searchDebounced = useDebounceFn((val: string) => fetchPotentialDisciplines(val), 300)

watch(searchTerm, val => searchDebounced(val))

async function save() {
  if (!selectedDisciplineIds.value.length || !props.course) return
  saving.value = true
  try {
    await $fetch(`${config.public.backendUrl}/courses/disciplines`, {
      method: 'POST',
      body: { courseId: props.course.id, disciplines: selectedDisciplineIds.value },
      credentials: 'include',
    })
    toast.add({ title: 'Disciplinas vinculadas com sucesso', color: 'success' })
    selectedDisciplineIds.value = []
    searchTerm.value = ''
    await Promise.all([fetchCourse(), fetchPotentialDisciplines('')])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao vincular disciplinas.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

async function removeDiscipline(disciplineId: number) {
  if (!props.course) return
  removingDisciplineId.value = disciplineId
  try {
    await $fetch(`${config.public.backendUrl}/courses/disciplines`, {
      method: 'DELETE',
      body: { courseId: props.course.id, disciplineId },
      credentials: 'include',
    })
    toast.add({ title: 'Vínculo removido com sucesso', color: 'success' })
    await Promise.all([fetchCourse(), fetchPotentialDisciplines(searchTerm.value)])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao remover vínculo.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    removingDisciplineId.value = null
  }
}

watch(open, (val) => {
  if (val) {
    fetchCourse()
    fetchPotentialDisciplines('')
  } else {
    course.value = null
    potentialDisciplines.value = []
    selectedDisciplineIds.value = []
    searchTerm.value = ''
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    :title="`Disciplinas - ${props.course?.name ?? ''}`"
    :fullscreen="isMobile"
    description="Disciplinas vinculadas a este curso."
  >
    <template #body>
      <div class="space-y-6">
        <div class="flex gap-2">
          <USelectMenu
            v-model="selectedDisciplineIds"
            v-model:search-term="searchTerm"
            :items="potentialDisciplines"
            label-key="name"
            value-key="id"
            multiple
            ignore-filter
            placeholder="Pesquisar disciplinas para vincular..."
            class="flex-1"
            searchable
            :search-input="{ placeholder: 'Buscar por nome...' }"
          />
          <UButton
            label="Vincular"
            :disabled="!selectedDisciplineIds.length"
            :loading="saving"
            @click="save"
          />
        </div>

        <div v-if="loadingCourse" class="flex justify-center py-8">
          <AppSpinner class="size-6 text-muted" />
        </div>

        <div v-else-if="!course?.disciplines.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-book-open" class="size-10" />
          <p class="text-sm">Nenhuma disciplina vinculada</p>
        </div>

        <ul v-else class="divide-y divide-default">
          <li
            v-for="discipline in course?.disciplines"
            :key="discipline.id"
            class="flex items-center py-3"
          >
            <span class="flex-1 text-sm">{{ discipline.name }}</span>
            <UTooltip text="Remover">
              <UButton
                icon="i-lucide-trash-2"
                color="neutral"
                variant="ghost"
                size="xs"
                :loading="removingDisciplineId === discipline.id"
                @click="removeDiscipline(discipline.id)"
              />
            </UTooltip>
          </li>
        </ul>
      </div>
    </template>
  </UModal>
</template>
