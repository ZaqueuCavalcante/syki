<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import type { CalendarItem, DayType } from '~/types/calendar'

const props = defineProps<{ day: CalendarItem | null }>()

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ saved: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const dayTypeOptions: { label: string, value: DayType }[] = [
  { label: 'Dia letivo', value: 'Default' },
  { label: 'Férias', value: 'Vacation' },
  { label: 'Recesso', value: 'Recess' },
  { label: 'Feriado', value: 'Holiday' },
]

const schema = z.object({
  dayType: z.enum(['Default', 'Vacation', 'Recess', 'Holiday'], { error: 'Tipo obrigatório' }),
  description: z.string().max(100, 'Máximo 100 caracteres').optional(),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  dayType: undefined,
  description: '',
})

const formattedDate = computed(() => {
  if (!props.day) return ''
  const [year, month, day] = props.day.date.slice(0, 10).split('-')
  return `${day}/${month}/${year}`
})

watch(open, (val) => {
  if (!val || !props.day) return
  formState.dayType = props.day.dayType
  formState.description = props.day.description ?? ''
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  if (!props.day) return

  loading.value = true
  try {
    const description = event.data.description || null

    if (props.day.id) {
      await $fetch(`${config.public.backendUrl}/calendar/days`, {
        method: 'PUT',
        body: { id: props.day.id, dayType: event.data.dayType, description },
        credentials: 'include',
      })
    } else {
      await $fetch(`${config.public.backendUrl}/calendar/days`, {
        method: 'POST',
        body: { date: props.day.date, dayType: event.data.dayType, description },
        credentials: 'include',
      })
    }

    toast.add({ title: 'Dia atualizado com sucesso', color: 'success' })
    open.value = false
    emit('saved')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao salvar o dia.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    :title="`Dia ${formattedDate}`"
    :fullscreen="isMobile"
    description="Defina o tipo do dia no calendário acadêmico da instituição."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Tipo" name="dayType">
          <USelectMenu
            v-model="formState.dayType"
            :items="dayTypeOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecionar"
          />
        </UFormField>

        <UFormField label="Descrição" name="description">
          <UInput
            v-model="formState.description"
            class="w-full"
            placeholder="Ex: Semana de provas"
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
