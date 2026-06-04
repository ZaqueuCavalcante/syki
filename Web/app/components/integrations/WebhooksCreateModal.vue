<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const eventOptions = [
  { label: 'Aluno criado', value: 'StudentCreated' },
  { label: 'Atividade publicada', value: 'ClassActivityCreated' },
]

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  url: z.string().min(1, 'URL obrigatória').url('URL inválida'),
  events: z.array(z.string()).min(1, 'Selecione ao menos um evento'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  url: '',
  events: [],
})

function toggleEvent(value: string) {
  const idx = formState.events!.indexOf(value)
  if (idx === -1) formState.events!.push(value)
  else formState.events!.splice(idx, 1)
}

function resetForm() {
  formState.name = ''
  formState.url = ''
  formState.events = []
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/webhooks/subscriptions`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Webhook criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar webhook.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo webhook"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar uma nova inscrição de webhook."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Aluno criado" />
        </UFormField>

        <UFormField label="URL" name="url">
          <UInput v-model="formState.url" class="w-full" placeholder="https://meu-site.com/webhook" />
        </UFormField>

        <UFormField label="Eventos" name="events">
          <div class="flex flex-col gap-2 w-full">
            <UCheckbox
              v-for="opt in eventOptions"
              :key="opt.value"
              :label="opt.label"
              :model-value="formState.events!.includes(opt.value)"
              @update:model-value="toggleEvent(opt.value)"
            />
          </div>
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
            label="Criar webhook"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
