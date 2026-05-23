<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface CampusItem {
  id: number
  name: string
  state: string
  city: string
  capacity: number
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ campus: CampusItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const brazilStates = [
  { label: 'Acre', value: 'AC' },
  { label: 'Alagoas', value: 'AL' },
  { label: 'Amapá', value: 'AP' },
  { label: 'Amazonas', value: 'AM' },
  { label: 'Bahia', value: 'BA' },
  { label: 'Ceará', value: 'CE' },
  { label: 'Distrito Federal', value: 'DF' },
  { label: 'Espírito Santo', value: 'ES' },
  { label: 'Goiás', value: 'GO' },
  { label: 'Maranhão', value: 'MA' },
  { label: 'Mato Grosso', value: 'MT' },
  { label: 'Mato Grosso do Sul', value: 'MS' },
  { label: 'Minas Gerais', value: 'MG' },
  { label: 'Pará', value: 'PA' },
  { label: 'Paraíba', value: 'PB' },
  { label: 'Paraná', value: 'PR' },
  { label: 'Pernambuco', value: 'PE' },
  { label: 'Piauí', value: 'PI' },
  { label: 'Rio de Janeiro', value: 'RJ' },
  { label: 'Rio Grande do Norte', value: 'RN' },
  { label: 'Rio Grande do Sul', value: 'RS' },
  { label: 'Rondônia', value: 'RO' },
  { label: 'Roraima', value: 'RR' },
  { label: 'Santa Catarina', value: 'SC' },
  { label: 'São Paulo', value: 'SP' },
  { label: 'Sergipe', value: 'SE' },
  { label: 'Tocantins', value: 'TO' },
]

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
  state: z.string().min(1, 'Estado obrigatório'),
  city: z.string().min(1, 'Cidade obrigatória').max(50, 'Máximo 50 caracteres'),
  capacity: z.coerce.number({ error: 'Capacidade obrigatória' }).int().gt(0, 'Deve ser maior que 0').max(999999, 'Máximo 999.999'),
})

type Schema = z.output<typeof schema>

const capacityDisplay = ref('')

const formState = reactive<Partial<Schema>>({
  name: '',
  state: undefined,
  city: '',
  capacity: undefined,
})

function formatCapacity(digits: string): string {
  return digits.replace(/\B(?=(\d{3})+(?!\d))/g, '.')
}

const ALLOWED_KEYS = new Set(['Backspace', 'Delete', 'Tab', 'ArrowLeft', 'ArrowRight', 'Home', 'End'])

function onCapacityKeydown(e: KeyboardEvent) {
  if (ALLOWED_KEYS.has(e.key)) return
  if (!/^\d$/.test(e.key)) { e.preventDefault(); return }
  const input = e.target as HTMLInputElement
  const hasSelection = input.selectionStart !== input.selectionEnd
  const digitCount = input.value.replace(/\D/g, '').length
  if (!hasSelection && digitCount >= 6) e.preventDefault()
}

function onCapacityInput(e: Event) {
  const input = e.target as HTMLInputElement
  const digits = input.value.replace(/\D/g, '').slice(0, 6)
  const formatted = digits ? formatCapacity(digits) : ''
  capacityDisplay.value = formatted
  input.value = formatted
  formState.capacity = digits ? Number(digits) : undefined
}

watch(open, (val) => {
  if (val && props.campus) {
    formState.name = props.campus.name
    formState.state = props.campus.state
    formState.city = props.campus.city
    formState.capacity = props.campus.capacity
    capacityDisplay.value = formatCapacity(String(props.campus.capacity))
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/campi`, {
      method: 'PUT',
      body: { id: props.campus!.id, ...event.data },
      credentials: 'include',
    })
    toast.add({ title: 'Campus atualizado com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar campus.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar campus"
    :fullscreen="isMobile"
    description="Atualize os dados do campus."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Campus Agreste" />
        </UFormField>

        <div class="grid grid-cols-2 gap-4">
          <UFormField label="Estado" name="state">
            <USelect
              v-model="formState.state"
              :items="brazilStates"
              value-key="value"
              class="w-full"
              placeholder="Selecione"
            />
          </UFormField>

          <UFormField label="Cidade" name="city">
            <UInput v-model="formState.city" class="w-full" placeholder="Ex: Caruaru" />
          </UFormField>
        </div>

        <UFormField label="Capacidade" name="capacity">
          <UInput
            :model-value="capacityDisplay"
            type="text"
            inputmode="numeric"
            class="w-full"
            placeholder="Ex: 500"
            @keydown="onCapacityKeydown"
            @input="onCapacityInput"
          />
        </UFormField>

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
