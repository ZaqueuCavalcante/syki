<script setup lang="ts">
import * as z from 'zod'
import { DateFormatter, getLocalTimeZone, type CalendarDate } from '@internationalized/date'
import type { FormSubmitEvent } from '@nuxt/ui'

const props = defineProps<{ classId: number | string }>()

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const df = new DateFormatter('pt-BR', { dateStyle: 'medium' })

const toDateString = (d: CalendarDate) =>
  `${d.year}-${String(d.month).padStart(2, '0')}-${String(d.day).padStart(2, '0')}`

const dueDate = ref<CalendarDate | undefined>()
const dueDateOpen = ref(false)

// ── Options ───────────────────────────────────────────────────
const noteOptions = [
  { label: 'N1', value: 'N1' },
  { label: 'N2', value: 'N2' },
  { label: 'N3', value: 'N3' },
]

const typeOptions = [
  { label: 'Prova', value: 'Exam' },
  { label: 'Projeto', value: 'Project' },
  { label: 'Trabalho', value: 'Work' },
  { label: 'Apresentação', value: 'Presentation' },
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

// ── Weight input ──────────────────────────────────────────────
const weightDisplay = ref('')

const ALLOWED_KEYS = new Set(['Backspace', 'Delete', 'Tab', 'ArrowLeft', 'ArrowRight', 'Home', 'End'])

function onWeightKeydown(e: KeyboardEvent) {
  if (ALLOWED_KEYS.has(e.key)) return
  if (!/^\d$/.test(e.key)) { e.preventDefault(); return }
  const input = e.target as HTMLInputElement
  const hasSelection = input.selectionStart !== input.selectionEnd
  const digitCount = input.value.replace(/\D/g, '').length
  if (!hasSelection && digitCount >= 3) e.preventDefault()
}

function onWeightInput(e: Event) {
  const input = e.target as HTMLInputElement
  const digits = input.value.replace(/\D/g, '').slice(0, 3)
  weightDisplay.value = digits
  input.value = digits
  formState.weight = digits ? Number(digits) : undefined
}

// ── Form schema ───────────────────────────────────────────────
const schema = z.object({
  note: z.string({ error: 'Campo obrigatório' }).min(1, 'Campo obrigatório'),
  title: z.string().min(1, 'Título obrigatório').max(100, 'Máximo 100 caracteres'),
  description: z.string().min(1, 'Descrição obrigatória').max(500, 'Máximo 500 caracteres'),
  type: z.string({ error: 'Campo obrigatório' }).min(1, 'Campo obrigatório'),
  weight: z.coerce.number({ error: 'Campo obrigatório' }).int().min(0, 'Mínimo 0').max(100, 'Máximo 100'),
  dueDate: z.string({ error: 'Campo obrigatório' }).min(1, 'Data limite obrigatória'),
  dueHour: z.string({ error: 'Campo obrigatório' }).min(1, 'Hora limite obrigatória'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  note: 'N1',
  title: '',
  description: '',
  type: undefined,
  weight: undefined,
  dueDate: '',
  dueHour: undefined,
})

watch(dueDate, (val) => {
  formState.dueDate = val ? toDateString(val) : ''
  if (val) dueDateOpen.value = false
})

function resetForm() {
  formState.note = 'N1'
  formState.title = ''
  formState.description = ''
  formState.type = undefined
  formState.weight = undefined
  weightDisplay.value = ''
  formState.dueDate = ''
  formState.dueHour = undefined
  dueDate.value = undefined
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/teachers/classes/${props.classId}/activities`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Atividade criada com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar atividade.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Nova atividade"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar uma nova atividade da turma."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Título" name="title" required>
          <UInput v-model="formState.title" class="w-full" placeholder="Ex: Modelagem de Banco de Dados" />
        </UFormField>

        <UFormField label="Descrição" name="description" required>
          <UTextarea
            v-model="formState.description"
            class="w-full"
            :rows="3"
            placeholder="Descreva o que o aluno deve entregar"
          />
        </UFormField>

        <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
          <UFormField label="Nota" name="note" required>
            <USelect
              v-model="formState.note"
              :items="noteOptions"
              value-key="value"
              class="w-full"
              placeholder="Selecione"
            />
          </UFormField>

          <UFormField label="Tipo" name="type" required>
            <USelect
              v-model="formState.type"
              :items="typeOptions"
              value-key="value"
              class="w-full"
              placeholder="Selecione"
            />
          </UFormField>

          <UFormField label="Peso" name="weight" required>
            <UInput
              :model-value="weightDisplay"
              type="text"
              inputmode="numeric"
              class="w-full"
              placeholder="Ex: 40"
              @keydown="onWeightKeydown"
              @input="onWeightInput"
            />
          </UFormField>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <UFormField label="Data limite" name="dueDate" required>
            <UPopover v-model:open="dueDateOpen" :content="{ align: 'start' }" :modal="true" class="w-full">
              <UButton color="neutral" variant="outline" class="w-full">
                <div class="flex items-center w-full gap-2">
                  <UIcon name="i-lucide-calendar" class="size-4 shrink-0" />
                  <span class="flex-1 text-left truncate">{{ dueDate ? df.format(dueDate.toDate(getLocalTimeZone())) : 'Selecionar' }}</span>
                  <UIcon :name="dueDateOpen ? 'i-lucide-chevron-up' : 'i-lucide-chevron-down'" class="size-4 shrink-0" />
                </div>
              </UButton>
              <template #content>
                <UCalendar v-model="dueDate" class="p-2" />
              </template>
            </UPopover>
          </UFormField>

          <UFormField label="Hora limite" name="dueHour" required>
            <USelect
              v-model="formState.dueHour"
              :items="hourOptions"
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
