<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

// ── Options data ──────────────────────────────────────────────
interface DisciplineItem { id: number; name: string }
interface GetDisciplinesOut { total: number; items: DisciplineItem[] }

interface TeacherItem { id: number; name: string }
interface GetTeachersOut { total: number; items: TeacherItem[] }

interface PeriodItem { id: number; name: string }
interface GetPeriodsOut { total: number; items: PeriodItem[] }

const [{ data: disciplinesData }, { data: teachersData }, { data: periodsData }] = await Promise.all([
  useFetch<GetDisciplinesOut>(`${config.public.backendUrl}/disciplines`, { credentials: 'include', server: false }),
  useFetch<GetTeachersOut>(`${config.public.backendUrl}/teachers`, { credentials: 'include', server: false }),
  useFetch<GetPeriodsOut>(`${config.public.backendUrl}/periods/academic`, { credentials: 'include', server: false }),
])

const disciplineOptions = computed(() =>
  (disciplinesData.value?.items ?? []).map(d => ({ label: d.name, value: d.id }))
)
const teacherOptions = computed(() =>
  (teachersData.value?.items ?? []).map(t => ({ label: t.name, value: t.id }))
)
const periodOptions = computed(() =>
  (periodsData.value?.items ?? []).map(p => ({ label: p.name, value: p.id }))
)

// ── Day / Hour options ────────────────────────────────────────
const dayOptions = [
  { label: 'Segunda', value: 'Monday' },
  { label: 'Terça', value: 'Tuesday' },
  { label: 'Quarta', value: 'Wednesday' },
  { label: 'Quinta', value: 'Thursday' },
  { label: 'Sexta', value: 'Friday' },
  { label: 'Sábado', value: 'Saturday' },
]

function buildHourOptions() {
  const opts = []
  for (let h = 7; h <= 23; h++) {
    for (let m = 0; m < 60; m += 15) {
      const hh = h.toString().padStart(2, '0')
      const mm = m.toString().padStart(2, '0')
      opts.push({ label: `${hh}:${mm}`, value: `H${hh}_${mm}` })
    }
  }
  return opts
}
const hourOptions = buildHourOptions()

// ── Schedules ─────────────────────────────────────────────────
interface ScheduleRow {
  day: string | undefined
  start: string | undefined
  end: string | undefined
}

const schedules = ref<ScheduleRow[]>([{ day: undefined, start: undefined, end: undefined }])

function addSchedule() {
  if (schedules.value.length < 3) schedules.value.push({ day: undefined, start: undefined, end: undefined })
}

function removeSchedule(index: number) {
  schedules.value.splice(index, 1)
}

// ── Vacancies input ───────────────────────────────────────────
const vacanciesDisplay = ref('')

function formatVacancies(digits: string): string {
  return digits.replace(/\B(?=(\d{3})+(?!\d))/g, '.')
}

const ALLOWED_KEYS = new Set(['Backspace', 'Delete', 'Tab', 'ArrowLeft', 'ArrowRight', 'Home', 'End'])

function onVacanciesKeydown(e: KeyboardEvent) {
  if (ALLOWED_KEYS.has(e.key)) return
  if (!/^\d$/.test(e.key)) { e.preventDefault(); return }
  const input = e.target as HTMLInputElement
  const hasSelection = input.selectionStart !== input.selectionEnd
  const digitCount = input.value.replace(/\D/g, '').length
  if (!hasSelection && digitCount >= 6) e.preventDefault()
}

function onVacanciesInput(e: Event) {
  const input = e.target as HTMLInputElement
  const digits = input.value.replace(/\D/g, '').slice(0, 6)
  const formatted = digits ? formatVacancies(digits) : ''
  vacanciesDisplay.value = formatted
  input.value = formatted
  formState.vacancies = digits ? Number(digits) : undefined
}

// ── Form schema ───────────────────────────────────────────────
const scheduleSchema = z.object({
  day:   z.string({ error: 'Dia obrigatório' }).min(1, 'Dia obrigatório'),
  start: z.string({ error: 'Início obrigatório' }).min(1, 'Início obrigatório'),
  end:   z.string({ error: 'Fim obrigatório' }).min(1, 'Fim obrigatório'),
})

const schema = z.object({
  disciplineId: z.number({ error: 'Campo obrigatório' }),
  teacherId:    z.number().optional(),
  periodId:     z.number({ error: 'Campo obrigatório' }),
  vacancies:    z.coerce.number({ error: 'Campo obrigatório' }).int().gt(0, 'Deve ser maior que 0').max(999999, 'Máximo 999.999'),
  schedules:    z.array(scheduleSchema).min(1, 'Adicione pelo menos um horário'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  disciplineId: undefined,
  teacherId:    undefined,
  periodId:     undefined,
  vacancies:    undefined,
  schedules:    [],
})

function syncSchedules() {
  formState.schedules = schedules.value as Schema['schedules']
}

watch(schedules, syncSchedules, { deep: true })

function resetForm() {
  formState.disciplineId = undefined
  formState.teacherId    = undefined
  formState.periodId     = undefined
  formState.vacancies    = undefined
  vacanciesDisplay.value = ''
  formState.schedules    = []
  schedules.value = [{ day: undefined, start: undefined, end: undefined }]
}

watch(open, (val) => { if (!val) resetForm() })

// ── Submit ────────────────────────────────────────────────────
async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    const body = {
      disciplineId: event.data.disciplineId,
      campusId:     null,
      teacherId:    event.data.teacherId ?? null,
      periodId:     event.data.periodId,
      vacancies:    event.data.vacancies,
      schedules:    event.data.schedules.map(s => ({ day: s.day, start: s.start, end: s.end })),
    }
    await $fetch(`${config.public.backendUrl}/classes`, {
      method: 'POST',
      body,
      credentials: 'include',
    })
    toast.add({ title: 'Turma criada com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar turma.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Nova Turma"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar uma nova turma."
  >
    <template #body>
      <UForm :schema="schema" :state="formState" class="space-y-4" @submit="onSubmit">

        <UFormField label="Disciplina" name="disciplineId" required>
          <USelect
            v-model="formState.disciplineId"
            :items="disciplineOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione a disciplina"
          />
        </UFormField>

        <UFormField label="Professor" name="teacherId">
          <USelect
            v-model="formState.teacherId"
            :items="teacherOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione o professor"
          />
        </UFormField>

        <div class="grid grid-cols-2 gap-4">
          <UFormField label="Vagas" name="vacancies" required>
            <UInput
              :model-value="vacanciesDisplay"
              type="text"
              inputmode="numeric"
              class="w-full"
              placeholder="Ex: 40"
              @keydown="onVacanciesKeydown"
              @input="onVacanciesInput"
            />
          </UFormField>

          <UFormField label="Período" name="periodId" required>
            <USelect
              v-model="formState.periodId"
              :items="periodOptions"
              value-key="value"
              class="w-full"
              placeholder="Selecione"
            />
          </UFormField>
        </div>

        <!-- Schedules -->
        <div class="space-y-2">
          <div
            v-for="(schedule, index) in schedules"
            :key="index"
            class="flex items-center gap-2"
          >
            <USelect
              v-model="schedule.day"
              :items="dayOptions"
              value-key="value"
              class="flex-1"
              placeholder="Dia"
              @update:model-value="syncSchedules"
            />
            <USelect
              v-model="schedule.start"
              :items="hourOptions"
              value-key="value"
              class="flex-1"
              placeholder="Início"
              @update:model-value="syncSchedules"
            />
            <USelect
              v-model="schedule.end"
              :items="hourOptions"
              value-key="value"
              class="flex-1"
              placeholder="Fim"
              @update:model-value="syncSchedules"
            />
            <UTooltip text="Remover">
              <UButton
                icon="i-lucide-trash-2"
                color="neutral"
                variant="ghost"
                :disabled="schedules.length === 1"
                @click="removeSchedule(index)"
              />
            </UTooltip>
          </div>

          <button
            v-if="schedules.length < 3"
            type="button"
            class="w-fit border border-dashed border-default rounded-md px-4 py-2 text-sm text-muted hover:text-default hover:border-accented transition-colors"
            @click="addSchedule"
          >
            + NOVO HORÁRIO
          </button>
        </div>

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="loading" @click="() => { open = false }" />
          <UButton label="Salvar" type="submit" :loading="loading" />
        </div>

      </UForm>
    </template>
  </UModal>
</template>
