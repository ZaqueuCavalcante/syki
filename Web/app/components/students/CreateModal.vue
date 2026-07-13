<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

function onlyDigits(value: string) {
  return value.replace(/\D/g, '')
}

function formatBirthdate(value: string) {
  const digits = onlyDigits(value).slice(0, 8)

  if (digits.length <= 2) return digits
  if (digits.length <= 4) return `${digits.slice(0, 2)}/${digits.slice(2)}`

  return `${digits.slice(0, 2)}/${digits.slice(2, 4)}/${digits.slice(4)}`
}

function formatPhoneNumber(value: string) {
  const digits = onlyDigits(value).slice(0, 11)

  if (digits.length === 0) return ''
  if (digits.length <= 2) return `(${digits}`
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`
  if (digits.length <= 10) return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`

  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`
}

// Escreve o valor mascarado de volta no elemento: sem isso, quando a máscara descarta
// o que foi digitado (uma letra), o model não muda, o Vue não re-renderiza e o caractere fica no DOM
function maskInput(event: Event, format: (value: string) => string) {
  const input = event.target as HTMLInputElement
  const formatted = format(input.value)
  input.value = formatted

  return formatted
}

// Converte 'dd/mm/yyyy' pra 'yyyy-mm-dd', ou null quando a data não existe no calendário
function toIsoDate(value: string) {
  const match = /^(\d{2})\/(\d{2})\/(\d{4})$/.exec(value)
  if (!match) return null

  const [, day, month, year] = match as unknown as [string, string, string, string]
  const date = new Date(`${year}-${month}-${day}T00:00:00`)

  const isRealDate = date.getFullYear() === Number(year)
    && date.getMonth() + 1 === Number(month)
    && date.getDate() === Number(day)

  return isRealDate ? `${year}-${month}-${day}` : null
}

function isValidBirthdate(value: string) {
  const isoDate = toIsoDate(value)
  if (!isoDate) return false

  const today = new Date().toISOString().slice(0, 10)

  return isoDate >= '1900-01-01' && isoDate <= today
}

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  email: z.string().min(1, 'Email obrigatório').email('Email inválido'),
  phoneNumber: z.string()
    .refine(value => !value || /^\d{10,11}$/.test(onlyDigits(value)), 'Telefone inválido')
    .optional(),
  birthdate: z.string()
    .refine(value => !value || isValidBirthdate(value), 'Data de nascimento inválida')
    .optional(),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  email: '',
  phoneNumber: '',
  birthdate: '',
})

function resetForm() {
  formState.name = ''
  formState.email = ''
  formState.phoneNumber = ''
  formState.birthdate = ''
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    const phoneNumber = onlyDigits(event.data.phoneNumber ?? '')
    const birthdate = event.data.birthdate ? toIsoDate(event.data.birthdate) : null
    await $fetch(`${config.public.backendUrl}/students`, {
      method: 'POST',
      body: {
        name: event.data.name,
        email: event.data.email,
        phoneNumber: phoneNumber || null,
        birthdate,
      },
      credentials: 'include',
    })
    toast.add({ title: 'Aluno criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar aluno.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo aluno"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar um novo aluno."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: João Silva" />
        </UFormField>

        <UFormField label="Email" name="email">
          <UInput v-model="formState.email" type="email" class="w-full" placeholder="Ex: aluno@instituicao.edu.br" />
        </UFormField>

        <UFormField label="Data de nascimento" name="birthdate" hint="Opcional">
          <UInput
            :model-value="formState.birthdate"
            inputmode="numeric"
            class="w-full"
            placeholder="Ex: 25/09/1998"
            @input="(e) => { formState.birthdate = maskInput(e, formatBirthdate) }"
          />
        </UFormField>

        <UFormField label="Telefone" name="phoneNumber" hint="Opcional">
          <UInput
            :model-value="formState.phoneNumber"
            inputmode="numeric"
            class="w-full"
            placeholder="Ex: (81) 98570-6838"
            @input="(e) => { formState.phoneNumber = maskInput(e, formatPhoneNumber) }"
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
            label="Criar aluno"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
