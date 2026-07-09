<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

interface CourseItem {
  id: number
  name: string
  type: string
}

interface DisciplineItem {
  id: number
  name: string
}

interface DisciplineRow {
  disciplineId: number | undefined
  period: number
  credits: number
  workload: number
}

const courses = ref<CourseItem[]>([])
const availableDisciplines = ref<DisciplineItem[]>([])
const loadingDisciplines = ref(false)
const disciplineRows = ref<DisciplineRow[]>([])

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  courseId: z.number({ error: 'Curso obrigatório' }),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  courseId: undefined,
})

watch(() => formState.courseId, async (newId) => {
  disciplineRows.value = []
  availableDisciplines.value = []
  if (!newId) return
  loadingDisciplines.value = true
  try {
    const result = await $fetch<{ items: DisciplineItem[] }>(
      `${config.public.backendUrl}/courses/${newId}/disciplines`,
      { credentials: 'include' },
    )
    availableDisciplines.value = result.items
  } finally {
    loadingDisciplines.value = false
  }
})

function disciplinesForRow(rowIndex: number): DisciplineItem[] {
  const selectedIds = new Set(
    disciplineRows.value
      .filter((_, i) => i !== rowIndex)
      .map(r => r.disciplineId)
      .filter((id): id is number => id !== undefined),
  )
  return availableDisciplines.value.filter(d => !selectedIds.has(d.id))
}

const canAddDiscipline = computed(() => {
  if (!formState.courseId || availableDisciplines.value.length === 0) return false
  const selectedCount = disciplineRows.value.filter(r => r.disciplineId !== undefined).length
  return selectedCount < availableDisciplines.value.length
})

function addRow() {
  disciplineRows.value.push({ disciplineId: undefined, period: 1, credits: 0, workload: 0 })
}

function removeRow(index: number) {
  disciplineRows.value.splice(index, 1)
}

async function fetchCourses() {
  const result = await $fetch<{ items: CourseItem[] }>(`${config.public.backendUrl}/courses`, {
    credentials: 'include',
  })
  courses.value = result.items
}

function resetForm() {
  formState.name = ''
  formState.courseId = undefined
  disciplineRows.value = []
  availableDisciplines.value = []
}

watch(open, (val) => {
  if (val) fetchCourses()
  else resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/course-curriculums`, {
      method: 'POST',
      body: {
        name: event.data.name,
        courseId: event.data.courseId,
        disciplines: disciplineRows.value
          .filter(r => r.disciplineId !== undefined)
          .map(r => ({
            id: r.disciplineId,
            period: r.period,
            credits: r.credits,
            workload: r.workload,
          })),
      },
      credentials: 'include',
    })
    toast.add({ title: 'Grade curricular criada com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar grade curricular.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Nova grade curricular"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar uma nova grade curricular."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Grade ADS 2024" />
        </UFormField>

        <UFormField label="Curso" name="courseId">
          <USelectMenu
            v-model="formState.courseId"
            :items="courses"
            label-key="name"
            value-key="id"
            class="w-full"
            placeholder="Selecione o curso"
          />
        </UFormField>

        <div v-if="formState.courseId" class="space-y-3">
          <div class="flex items-center justify-between">
            <span class="text-sm font-medium text-highlighted">Disciplinas</span>
            <UButton
              v-if="canAddDiscipline"
              icon="i-lucide-plus"
              label="Adicionar"
              color="neutral"
              variant="subtle"
              size="sm"
              @click="addRow"
            />
          </div>

          <div v-if="loadingDisciplines" class="flex justify-center py-6">
            <UIcon name="i-lucide-loader" class="size-5 animate-spin text-muted" />
          </div>

          <template v-else>
            <div v-if="!disciplineRows.length" class="flex flex-col items-center gap-2 py-6 text-muted">
              <UIcon name="i-lucide-book-open" class="size-8" />
              <p class="text-sm">Nenhuma disciplina adicionada</p>
              <UButton
                v-if="availableDisciplines.length"
                icon="i-lucide-plus"
                label="Adicionar disciplina"
                color="neutral"
                variant="subtle"
                size="sm"
                @click="addRow"
              />
            </div>

            <div v-else class="space-y-2">
              <div class="grid grid-cols-[1fr_4rem_4rem_4rem_2rem] gap-2 px-1">
                <span class="text-xs text-muted">Disciplina</span>
                <span class="text-xs text-muted text-center">Período</span>
                <span class="text-xs text-muted text-center">Créditos</span>
                <span class="text-xs text-muted text-center">C.H.</span>
                <span />
              </div>

              <div
                v-for="(row, index) in disciplineRows"
                :key="index"
                class="grid grid-cols-[1fr_4rem_4rem_4rem_2rem] items-center gap-2"
              >
                <USelectMenu
                  v-model="row.disciplineId"
                  :items="disciplinesForRow(index)"
                  label-key="name"
                  value-key="id"
                  placeholder="Selecionar..."
                />
                <UInputNumber
                  v-model="row.period"
                  :min="1"
                  :max="10"
                  :increment="false"
                  :decrement="false"
                  class="text-center"
                />
                <UInputNumber
                  v-model="row.credits"
                  :min="0"
                  :max="100"
                  :increment="false"
                  :decrement="false"
                  class="text-center"
                />
                <UInputNumber
                  v-model="row.workload"
                  :min="0"
                  :max="500"
                  :increment="false"
                  :decrement="false"
                  class="text-center"
                />
                <UTooltip text="Remover">
                  <UButton
                    icon="i-lucide-x"
                    color="neutral"
                    variant="ghost"
                    size="xs"
                    @click="removeRow(index)"
                  />
                </UTooltip>
              </div>
            </div>
          </template>
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
            label="Criar grade"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
