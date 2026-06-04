<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface CourseCurriculumItem {
  id: number
  name: string
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

interface GetCourseCurriculumOut {
  id: number
  name: string
  courseId: number
  course: string
  disciplines: {
    id: number
    name: string
    period: number
    credits: number
    workload: number
  }[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ curriculum: CourseCurriculumItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)
const loadingData = ref(false)

const courseName = ref('')
const courseId = ref<number | null>(null)
const availableDisciplines = ref<DisciplineItem[]>([])
const disciplineRows = ref<DisciplineRow[]>([])

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
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
  if (!courseId.value || availableDisciplines.value.length === 0) return false
  const selectedCount = disciplineRows.value.filter(r => r.disciplineId !== undefined).length
  return selectedCount < availableDisciplines.value.length
})

function addRow() {
  disciplineRows.value.push({ disciplineId: undefined, period: 1, credits: 0, workload: 0 })
}

function removeRow(index: number) {
  disciplineRows.value.splice(index, 1)
}

async function fetchData() {
  if (!props.curriculum) return
  loadingData.value = true
  try {
    const curriculum = await $fetch<GetCourseCurriculumOut>(
      `${config.public.backendUrl}/course-curriculums/${props.curriculum.id}`,
      { credentials: 'include' },
    )

    formState.name = curriculum.name
    courseName.value = curriculum.course
    courseId.value = curriculum.courseId

    const result = await $fetch<{ items: DisciplineItem[] }>(
      `${config.public.backendUrl}/courses/${curriculum.courseId}/disciplines`,
      { credentials: 'include' },
    )
    availableDisciplines.value = result.items

    disciplineRows.value = curriculum.disciplines.map(d => ({
      disciplineId: d.id,
      period: d.period,
      credits: d.credits,
      workload: d.workload,
    }))
  } finally {
    loadingData.value = false
  }
}

function resetForm() {
  formState.name = ''
  courseName.value = ''
  courseId.value = null
  availableDisciplines.value = []
  disciplineRows.value = []
}

watch(open, (val) => {
  if (val) fetchData()
  else resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/course-curriculums`, {
      method: 'PUT',
      body: {
        id: props.curriculum!.id,
        name: event.data.name,
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
    toast.add({ title: 'Grade curricular atualizada com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar grade curricular.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar grade curricular"
    :fullscreen="isMobile"
    description="Atualize os dados da grade curricular."
  >
    <template #body>
      <div v-if="loadingData" class="flex justify-center py-12">
        <UIcon name="i-lucide-loader-circle" class="size-6 animate-spin text-muted" />
      </div>

      <UForm
        v-else
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Grade ADS 2024" />
        </UFormField>

        <UFormField label="Curso">
          <UInput :model-value="courseName" class="w-full" disabled />
        </UFormField>

        <div class="space-y-3">
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
              <UButton
                icon="i-lucide-x"
                color="neutral"
                variant="ghost"
                size="xs"
                @click="removeRow(index)"
              />
            </div>
          </div>
        </div>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="open = false"
          />
          <UButton
            label="Salvar"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
