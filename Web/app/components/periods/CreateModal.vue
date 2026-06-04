<script setup lang="ts">
import * as z from 'zod'
import { DateFormatter, getLocalTimeZone, type CalendarDate } from '@internationalized/date'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const df = new DateFormatter('pt-BR', { dateStyle: 'medium' })

const toDateString = (d: CalendarDate) =>
  `${d.year}-${String(d.month).padStart(2, '0')}-${String(d.day).padStart(2, '0')}`

const startDate = ref<CalendarDate | undefined>()
const endDate = ref<CalendarDate | undefined>()
const startOpen = ref(false)
const endOpen = ref(false)

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(20, 'Máximo 20 caracteres'),
  startAt: z.string().min(1, 'Data de início obrigatória'),
  endAt: z.string().min(1, 'Data de término obrigatória'),
}).refine(data => !data.startAt || !data.endAt || data.endAt > data.startAt, {
  message: 'Data de término deve ser posterior ao início',
  path: ['endAt'],
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  startAt: '',
  endAt: '',
})

watch(startDate, (val) => { formState.startAt = val ? toDateString(val) : ''; if (val) startOpen.value = false })
watch(endDate, (val) => { formState.endAt = val ? toDateString(val) : ''; if (val) endOpen.value = false })

function resetForm() {
  formState.name = ''
  formState.startAt = ''
  formState.endAt = ''
  startDate.value = undefined
  endDate.value = undefined
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/periods/academic`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Período criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar período.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo período"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar um novo período acadêmico."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: 2024.1" />
        </UFormField>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <UFormField label="Início" name="startAt">
            <UPopover v-model:open="startOpen" :content="{ align: 'start' }" :modal="true" class="w-full">
              <UButton color="neutral" variant="outline" class="w-full">
                <div class="flex items-center w-full gap-2">
                  <UIcon name="i-lucide-calendar" class="size-4 shrink-0" />
                  <span class="flex-1 text-left truncate">{{ startDate ? df.format(startDate.toDate(getLocalTimeZone())) : 'Selecionar' }}</span>
                  <UIcon :name="startOpen ? 'i-lucide-chevron-up' : 'i-lucide-chevron-down'" class="size-4 shrink-0" />
                </div>
              </UButton>
              <template #content>
                <UCalendar v-model="startDate" class="p-2" />
              </template>
            </UPopover>
          </UFormField>

          <UFormField label="Fim" name="endAt">
            <UPopover v-model:open="endOpen" :content="{ align: 'start' }" :modal="true" class="w-full">
              <UButton color="neutral" variant="outline" class="w-full">
                <div class="flex items-center w-full gap-2">
                  <UIcon name="i-lucide-calendar" class="size-4 shrink-0" />
                  <span class="flex-1 text-left truncate">{{ endDate ? df.format(endDate.toDate(getLocalTimeZone())) : 'Selecionar' }}</span>
                  <UIcon :name="endOpen ? 'i-lucide-chevron-up' : 'i-lucide-chevron-down'" class="size-4 shrink-0" />
                </div>
              </UButton>
              <template #content>
                <UCalendar v-model="endDate" class="p-2" />
              </template>
            </UPopover>
          </UFormField>
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
            label="Criar período"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
