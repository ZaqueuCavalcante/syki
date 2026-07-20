<script setup lang="ts">
import type { ClassTeacherItem } from '~/types/classes'

interface GetDisciplineTeachersOut { items: ClassTeacherItem[] }

const MAX_TEACHERS = 2

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{
  classId: number
  disciplineId: number
  teachers: ClassTeacherItem[]
}>()
const emit = defineEmits<{ assigned: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()

const options = ref<ClassTeacherItem[]>([])
const selectedIds = ref<number[]>([])
const loadingOptions = ref(false)
const saving = ref(false)

const atLimit = computed(() => selectedIds.value.length >= MAX_TEACHERS)

async function fetchOptions() {
  loadingOptions.value = true
  try {
    const result = await $fetch<GetDisciplineTeachersOut>(
      `${config.public.backendUrl}/disciplines/${props.disciplineId}/teachers`,
      { credentials: 'include' },
    )
    options.value = result.items
  } catch {
    options.value = []
    toast.add({ title: 'Erro', description: 'Erro ao carregar os professores da disciplina.', color: 'error' })
  } finally {
    loadingOptions.value = false
  }
}

// Um professor já vinculado pode ter perdido o vínculo com a disciplina depois,
// então ele não vem nas opções e sumiria da seleção ao abrir o modal.
const items = computed(() => {
  const known = new Set(options.value.map(t => t.id))
  return [...options.value, ...props.teachers.filter(t => !known.has(t.id))]
})

function isSelected(id: number) {
  return selectedIds.value.includes(id)
}

function isDisabled(id: number) {
  return atLimit.value && !isSelected(id)
}

function toggleTeacher(id: number) {
  if (isSelected(id)) {
    selectedIds.value = selectedIds.value.filter(x => x !== id)
  } else if (!atLimit.value) {
    selectedIds.value = [...selectedIds.value, id]
  }
}

async function save() {
  saving.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classes/${props.classId}/teachers`, {
      method: 'PUT',
      body: { teachers: selectedIds.value },
      credentials: 'include',
    })
    toast.add({ title: 'Professores definidos com sucesso', color: 'success' })
    open.value = false
    emit('assigned')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao definir os professores.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    saving.value = false
  }
}

watch(open, (val) => {
  if (val) {
    selectedIds.value = props.teachers.map(t => t.id)
    fetchOptions()
  } else {
    options.value = []
    selectedIds.value = []
  }
})
</script>

<template>
  <UModal
    v-model:open="open"
    title="Professores da turma"
    :fullscreen="isMobile"
    description="Selecione até 2 professores entre os aptos a lecionar a disciplina."
  >
    <template #body>
      <div class="space-y-4">
        <div v-if="loadingOptions" class="flex justify-center py-8">
          <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
        </div>

        <div v-else-if="!items.length" class="flex flex-col items-center gap-3 py-8 text-muted">
          <UIcon name="i-lucide-user-x" class="size-10" />
          <p class="text-sm text-center">
            Nenhum professor vinculado a esta disciplina
          </p>
        </div>

        <template v-else>
          <div class="flex max-h-96 flex-col divide-y divide-default overflow-y-auto">
            <UCheckbox
              v-for="teacher in items"
              :key="teacher.id"
              :model-value="isSelected(teacher.id)"
              :disabled="isDisabled(teacher.id)"
              :label="teacher.name"
              class="py-3"
              @update:model-value="() => { toggleTeacher(teacher.id) }"
            />
          </div>

          <p class="text-xs text-muted">
            {{ selectedIds.length }} de {{ MAX_TEACHERS }} selecionados
          </p>
        </template>

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="saving" @click="() => { open = false }" />
          <UButton label="Salvar" :loading="saving" :disabled="loadingOptions" @click="() => { save() }" />
        </div>
      </div>
    </template>
  </UModal>
</template>
