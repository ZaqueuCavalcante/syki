<script setup lang="ts">
interface TeacherItem {
  id: number
  name: string
}

interface DisciplineItem {
  id: number
  name: string
}

interface GetTeacherOut {
  id: number
  name: string
  email: string
  campi: { id: number; name: string }[]
  disciplines: DisciplineItem[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ teacher: TeacherItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const teacher = ref<GetTeacherOut | null>(null)
const loadingTeacher = ref(false)

const potentialDisciplines = ref<DisciplineItem[]>([])
const searchTerm = ref('')
const selectedDisciplineIds = ref<number[]>([])
const saving = ref(false)

async function fetchTeacher() {
  if (!props.teacher) return
  loadingTeacher.value = true
  try {
    teacher.value = await $fetch<GetTeacherOut>(
      `${config.public.backendUrl}/teachers/${props.teacher.id}`,
      { credentials: 'include' }
    )
  } finally {
    loadingTeacher.value = false
  }
}

async function fetchPotentialDisciplines(name: string) {
  if (!props.teacher) return
  const params = name ? `?name=${encodeURIComponent(name)}` : ''
  const result = await $fetch<{ items: DisciplineItem[] }>(
    `${config.public.backendUrl}/teachers/${props.teacher.id}/potential-disciplines${params}`,
    { credentials: 'include' }
  )
  potentialDisciplines.value = result.items
}

const searchDebounced = useDebounceFn((val: string) => fetchPotentialDisciplines(val), 300)

watch(searchTerm, val => searchDebounced(val))

async function save() {
  if (!selectedDisciplineIds.value.length || !teacher.value) return
  saving.value = true
  try {
    const existingIds = teacher.value.disciplines.map(d => d.id)
    const allIds = [...existingIds, ...selectedDisciplineIds.value]
    await $fetch(`${config.public.backendUrl}/teachers/${props.teacher!.id}/assign-disciplines`, {
      method: 'PUT',
      body: { disciplines: allIds },
      credentials: 'include',
    })
    toast.add({ title: 'Disciplinas vinculadas com sucesso', color: 'success' })
    selectedDisciplineIds.value = []
    searchTerm.value = ''
    await Promise.all([fetchTeacher(), fetchPotentialDisciplines('')])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao vincular disciplinas.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

watch(open, (val) => {
  if (val) {
    fetchTeacher()
    fetchPotentialDisciplines('')
  } else {
    teacher.value = null
    potentialDisciplines.value = []
    selectedDisciplineIds.value = []
    searchTerm.value = ''
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    :title="`Disciplinas - ${props.teacher?.name ?? ''}`"
    :fullscreen="isMobile"
    description="Disciplinas vinculadas a este professor."
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

        <div v-if="loadingTeacher" class="flex justify-center py-8">
          <AppSpinner class="size-6 text-muted" />
        </div>

        <div v-else-if="!teacher?.disciplines.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-book-open" class="size-10" />
          <p class="text-sm">Nenhuma disciplina vinculada</p>
        </div>

        <ul v-else class="divide-y divide-default">
          <li
            v-for="discipline in teacher?.disciplines"
            :key="discipline.id"
            class="flex items-center py-3"
          >
            <span class="flex-1 text-sm">{{ discipline.name }}</span>
          </li>
        </ul>
      </div>
    </template>
  </UModal>
</template>
