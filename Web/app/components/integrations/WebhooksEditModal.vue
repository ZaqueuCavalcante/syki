<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface WebhookSubscriptionItem {
  id: number
  name: string
  url: string
  isActive: boolean
  events: string[]
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ subscription: WebhookSubscriptionItem | null }>()
const emit = defineEmits<{ updated: [] }>()

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
  isActive: z.boolean(),
  events: z.array(z.string()).min(1, 'Selecione ao menos um evento'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  url: '',
  isActive: true,
  events: [],
})

function toggleEvent(value: string) {
  const idx = formState.events!.indexOf(value)
  if (idx === -1) formState.events!.push(value)
  else formState.events!.splice(idx, 1)
}

watch(open, (val) => {
  if (val && props.subscription) {
    formState.name = props.subscription.name
    formState.url = props.subscription.url
    formState.isActive = props.subscription.isActive
    formState.events = [...props.subscription.events]
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/webhooks/subscriptions`, {
      method: 'PUT',
      body: { id: props.subscription!.id, ...event.data },
      credentials: 'include',
    })
    toast.add({ title: 'Webhook atualizado com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar webhook.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar webhook"
    :fullscreen="isMobile"
    description="Atualize os dados da inscrição de webhook."
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

        <UFormField label="Ativo" name="isActive">
          <UToggle v-model="formState.isActive" />
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
