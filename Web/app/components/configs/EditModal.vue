<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import type { InstitutionConfig } from '~/types/configs'

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ config: InstitutionConfig | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const schema = z.object({
  noteLimit: z.coerce.number({ error: 'Nota mínima obrigatória' })
    .min(0, 'Deve ser no mínimo 0')
    .max(10, 'Deve ser no máximo 10'),
  frequencyLimit: z.coerce.number({ error: 'Frequência mínima obrigatória' })
    .min(0, 'Deve ser no mínimo 0%')
    .max(100, 'Deve ser no máximo 100%'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  noteLimit: undefined,
  frequencyLimit: undefined,
})

// ── Number inputs ─────────────────────────────────────────────
// Só dígitos e uma vírgula decimal: letras e sinais são bloqueados na digitação
// e removidos no paste.
const noteDisplay = ref('')
const frequencyDisplay = ref('')

const ALLOWED_KEYS = new Set(['Backspace', 'Delete', 'Tab', 'ArrowLeft', 'ArrowRight', 'Home', 'End'])

function sanitize(value: string, maxIntDigits: number): string {
  const [int = '', ...rest] = value.replace(/\./g, ',').replace(/[^\d,]/g, '').split(',')

  const digits = int.slice(0, maxIntDigits)
  if (rest.length === 0) return digits

  return `${digits},${rest.join('').slice(0, 2)}`
}

function toNumber(display: string): number | undefined {
  if (!display || display === ',') return undefined
  return Number(display.replace(',', '.'))
}

function format(value: number): string {
  return String(value).replace('.', ',')
}

function onNumberKeydown(e: KeyboardEvent) {
  if (ALLOWED_KEYS.has(e.key) || e.ctrlKey || e.metaKey) return
  if (/^\d$/.test(e.key)) return

  const input = e.target as HTMLInputElement
  const isSeparator = e.key === ',' || e.key === '.'
  if (isSeparator && !input.value.includes(',')) return

  e.preventDefault()
}

function onNoteInput(e: Event) {
  const input = e.target as HTMLInputElement
  const display = sanitize(input.value, 2) // "10"
  noteDisplay.value = display
  input.value = display
  formState.noteLimit = toNumber(display)
}

function onFrequencyInput(e: Event) {
  const input = e.target as HTMLInputElement
  const display = sanitize(input.value, 3) // "100"
  frequencyDisplay.value = display
  input.value = display
  formState.frequencyLimit = toNumber(display)
}

watch(open, (val) => {
  if (val && props.config) {
    formState.noteLimit = props.config.noteLimit
    formState.frequencyLimit = props.config.frequencyLimit
    noteDisplay.value = format(props.config.noteLimit)
    frequencyDisplay.value = format(props.config.frequencyLimit)
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/institutions/config`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Configurações atualizadas com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar as configurações.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar configurações"
    :fullscreen="isMobile"
    description="Atualize os critérios de aprovação da sua instituição."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField
          label="Nota mínima"
          name="noteLimit"
          description="Para o aluno ser aprovado numa disciplina (de 0 a 10)."
        >
          <UInput
            :model-value="noteDisplay"
            type="text"
            inputmode="decimal"
            class="w-full"
            placeholder="Ex: 7"
            @keydown="onNumberKeydown"
            @input="onNoteInput"
          />
        </UFormField>

        <UFormField
          label="Frequência mínima (%)"
          name="frequencyLimit"
          description="Para o aluno ser aprovado numa disciplina (de 0% a 100%)."
        >
          <UInput
            :model-value="frequencyDisplay"
            type="text"
            inputmode="decimal"
            class="w-full"
            placeholder="Ex: 70"
            @keydown="onNumberKeydown"
            @input="onFrequencyInput"
          />
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="() => { open = false }"
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
