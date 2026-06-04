<script setup lang="ts">
interface TeacherItem {
  id: number
  name: string
}

interface CampusItem {
  id: number
  name: string
}

interface GetTeacherOut {
  id: number
  name: string
  email: string
  campi: CampusItem[]
  disciplines: { id: number; name: string }[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ teacher: TeacherItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const teacher = ref<GetTeacherOut | null>(null)
const loadingTeacher = ref(false)

const potentialCampi = ref<CampusItem[]>([])
const searchTerm = ref('')
const selectedCampusIds = ref<number[]>([])
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

async function fetchPotentialCampi(name: string) {
  if (!props.teacher) return
  const params = name ? `?name=${encodeURIComponent(name)}` : ''
  const result = await $fetch<{ items: CampusItem[] }>(
    `${config.public.backendUrl}/teachers/${props.teacher.id}/potential-campi${params}`,
    { credentials: 'include' }
  )
  potentialCampi.value = result.items
}

const searchDebounced = useDebounceFn((val: string) => fetchPotentialCampi(val), 300)

watch(searchTerm, val => searchDebounced(val))

async function save() {
  if (!selectedCampusIds.value.length || !teacher.value) return
  saving.value = true
  try {
    const existingIds = teacher.value.campi.map(c => c.id)
    const allIds = [...existingIds, ...selectedCampusIds.value]
    await $fetch(`${config.public.backendUrl}/teachers/${props.teacher!.id}/assign-campi`, {
      method: 'PUT',
      body: { campi: allIds },
      credentials: 'include',
    })
    toast.add({ title: 'Campus vinculados com sucesso', color: 'success' })
    selectedCampusIds.value = []
    searchTerm.value = ''
    await Promise.all([fetchTeacher(), fetchPotentialCampi('')])
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao vincular campus.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

watch(open, (val) => {
  if (val) {
    fetchTeacher()
    fetchPotentialCampi('')
  } else {
    teacher.value = null
    potentialCampi.value = []
    selectedCampusIds.value = []
    searchTerm.value = ''
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    :title="`Campus - ${props.teacher?.name ?? ''}`"
    :fullscreen="isMobile"
    description="Campus vinculados a este professor."
  >
    <template #body>
      <div class="space-y-6">
        <div class="flex gap-2">
          <USelectMenu
            v-model="selectedCampusIds"
            v-model:search-term="searchTerm"
            :items="potentialCampi"
            label-key="name"
            value-key="id"
            multiple
            ignore-filter
            placeholder="Pesquisar campus para vincular..."
            class="flex-1"
            searchable
            :search-input="{ placeholder: 'Buscar por nome...' }"
          />
          <UButton
            label="Vincular"
            :disabled="!selectedCampusIds.length"
            :loading="saving"
            @click="save"
          />
        </div>

        <div v-if="loadingTeacher" class="flex justify-center py-8">
          <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
        </div>

        <div v-else-if="!teacher?.campi.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-map-pin" class="size-10" />
          <p class="text-sm">Nenhum campus vinculado</p>
        </div>

        <ul v-else class="divide-y divide-default">
          <li
            v-for="campus in teacher?.campi"
            :key="campus.id"
            class="flex items-center py-3"
          >
            <span class="flex-1 text-sm">{{ campus.name }}</span>
          </li>
        </ul>
      </div>
    </template>
  </UModal>
</template>
