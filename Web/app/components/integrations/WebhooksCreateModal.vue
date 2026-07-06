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

const headerSchema = z.object({
  key: z.string().min(1, 'Chave obrigatória').max(100, 'Máximo 100 caracteres'),
  value: z.string().min(1, 'Valor obrigatório').max(1000, 'Máximo 1000 caracteres'),
})

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  url: z.string().min(1, 'URL obrigatória').url('URL inválida'),
  events: z.array(z.string()).min(1, 'Selecione ao menos um evento'),
  customHeaders: z.array(headerSchema).max(20, 'Máximo 20 headers'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  url: '',
  events: [],
  customHeaders: [],
})

function toggleEvent(value: string) {
  const idx = formState.events!.indexOf(value)
  if (idx === -1) formState.events!.push(value)
  else formState.events!.splice(idx, 1)
}

function addHeader() {
  formState.customHeaders!.push({ key: '', value: '' })
}

function removeHeader(idx: number) {
  formState.customHeaders!.splice(idx, 1)
}

function resetForm() {
  formState.name = ''
  formState.url = ''
  formState.events = []
  formState.customHeaders = []
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    const customHeaders = Object.fromEntries(
      event.data.customHeaders.map(h => [h.key, h.value]),
    )
    await $fetch(`${config.public.backendUrl}/webhooks/subscriptions`, {
      method: 'POST',
      body: { name: event.data.name, url: event.data.url, events: event.data.events, customHeaders },
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

        <UFormField
          label="Headers customizados"
          name="customHeaders"
          help="Enviados em todas as chamadas feitas para a URL. Útil para autenticação via header."
        >
          <div class="flex flex-col gap-2 w-full">
            <div
              v-for="(header, idx) in formState.customHeaders"
              :key="idx"
              class="flex items-start gap-2"
            >
              <UFormField :name="`customHeaders.${idx}.key`" class="flex-1">
                <UInput v-model="header.key" class="w-full" placeholder="Ex: Exato-AuthToken" />
              </UFormField>
              <UFormField :name="`customHeaders.${idx}.value`" class="flex-1">
                <UInput v-model="header.value" class="w-full" placeholder="Ex: 6r4g654rs6g4we6f4qw684f68qwf4" />
              </UFormField>
              <UButton
                icon="i-lucide-trash-2"
                color="error"
                variant="ghost"
                @click="removeHeader(idx)"
              />
            </div>
            <UButton
              label="Adicionar header"
              icon="i-lucide-plus"
              color="neutral"
              variant="subtle"
              class="self-start"
              @click="addHeader"
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
