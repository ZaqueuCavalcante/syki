<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface ClassroomItem {
  id: number
  name: string
  campusId: number
  campus: string
  capacity: number
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ classroom: ClassroomItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

// ── Campus options ────────────────────────────────────────────
interface CampusItem { id: number; name: string }
interface GetCampiOut { total: number; items: CampusItem[] }

const { data: campiData } = await useFetch<GetCampiOut>(`${config.public.backendUrl}/campi`, {
  credentials: 'include',
  server: false,
})

const campusOptions = computed(() =>
  (campiData.value?.items ?? []).map(c => ({ label: c.name, value: c.id }))
)

// ── Capacity input ────────────────────────────────────────────
const capacityDisplay = ref('')

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

// ── Form schema ───────────────────────────────────────────────
const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
  campusId: z.number({ error: 'Campus obrigatório' }),
  capacity: z.coerce.number({ error: 'Capacidade obrigatória' }).int().gt(0, 'Deve ser maior que 0').max(999999, 'Máximo 999.999'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  campusId: undefined,
  capacity: undefined,
})

watch(open, (val) => {
  if (val && props.classroom) {
    formState.name = props.classroom.name
    formState.campusId = props.classroom.campusId
    formState.capacity = props.classroom.capacity
    capacityDisplay.value = formatCapacity(String(props.classroom.capacity))
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/classrooms`, {
      method: 'PUT',
      body: { id: props.classroom!.id, ...event.data },
      credentials: 'include',
    })
    toast.add({ title: 'Sala atualizada com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar sala.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar sala"
    :fullscreen="isMobile"
    description="Atualize os dados da sala de aula."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Sala 05" />
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

        <UFormField label="Capacidade" name="capacity">
          <UInput
            :model-value="capacityDisplay"
            type="text"
            inputmode="numeric"
            class="w-full"
            placeholder="Ex: 40"
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
