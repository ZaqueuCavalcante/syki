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

interface PeriodItem { id: number; name: string }
interface GetPeriodsOut { total: number; items: PeriodItem[] }

interface CampusItem { id: number; name: string }
interface GetCampiOut { total: number; items: CampusItem[] }

const [{ data: disciplinesData }, { data: periodsData }, { data: campiData }] = await Promise.all([
  useFetch<GetDisciplinesOut>(`${config.public.backendUrl}/disciplines`, { credentials: 'include', server: false, query: { pageSize: 100 } }),
  useFetch<GetPeriodsOut>(`${config.public.backendUrl}/periods/academic`, { credentials: 'include', server: false }),
  useFetch<GetCampiOut>(`${config.public.backendUrl}/campi`, { credentials: 'include', server: false }),
])

const disciplineOptions = computed(() =>
  (disciplinesData.value?.items ?? []).map(d => ({ label: d.name, value: d.id }))
)
const periodOptions = computed(() =>
  (periodsData.value?.items ?? []).map(p => ({ label: p.name, value: p.id }))
)
const campusOptions = computed(() =>
  (campiData.value?.items ?? []).map(c => ({ label: c.name, value: c.id }))
)

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
const schema = z.object({
  disciplineId: z.number({ error: 'Campo obrigatório' }),
  campusId:     z.number().optional(),
  periodId:     z.number({ error: 'Campo obrigatório' }),
  vacancies:    z.coerce.number({ error: 'Campo obrigatório' }).int().gt(0, 'Deve ser maior que 0').max(999999, 'Máximo 999.999'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  disciplineId: undefined,
  campusId:     undefined,
  periodId:     undefined,
  vacancies:    undefined,
})

function resetForm() {
  formState.disciplineId = undefined
  formState.campusId     = undefined
  formState.periodId     = undefined
  formState.vacancies    = undefined
  vacanciesDisplay.value = ''
}

watch(open, (val) => { if (!val) resetForm() })

// ── Submit ────────────────────────────────────────────────────
async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    const body = {
      disciplineId: event.data.disciplineId,
      campusId:     event.data.campusId ?? null,
      periodId:     event.data.periodId,
      vacancies:    event.data.vacancies,
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
          <USelectMenu
            v-model="formState.disciplineId"
            :items="disciplineOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione a disciplina"
            :search-input="{ placeholder: 'Buscar por nome...' }"
          />
        </UFormField>

        <UFormField label="Campus" name="campusId">
          <USelectMenu
            v-model="formState.campusId"
            :items="campusOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione o campus"
            :search-input="{ placeholder: 'Buscar por nome...' }"
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

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="loading" @click="() => { open = false }" />
          <UButton label="Salvar" type="submit" :loading="loading" />
        </div>

      </UForm>
    </template>
  </UModal>
</template>
