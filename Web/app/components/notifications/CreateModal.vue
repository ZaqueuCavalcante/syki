<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const targetUsersOptions = [
  { label: 'Todos', value: 'All' },
  { label: 'Alunos', value: 'Students' },
  { label: 'Professores', value: 'Teachers' },
]

const schema = z.object({
  title: z.string().min(1, 'Título obrigatório').max(100, 'Máximo 100 caracteres'),
  description: z.string().min(1, 'Descrição obrigatória').max(500, 'Máximo 500 caracteres'),
  targetUsers: z.string({ error: 'Destinatários obrigatório' }).min(1, 'Destinatários obrigatório'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  title: '',
  description: '',
  targetUsers: undefined,
})

function resetForm() {
  formState.title = ''
  formState.description = ''
  formState.targetUsers = undefined
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/notifications`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Notificação criada com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar notificação.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Nova notificação"
    :fullscreen="isMobile"
    description="Preencha os dados para enviar uma nova notificação."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Título" name="title">
          <UInput v-model="formState.title" class="w-full" placeholder="Ex: Recesso acadêmico" />
        </UFormField>

        <UFormField label="Descrição" name="description">
          <UTextarea
            v-model="formState.description"
            class="w-full"
            :rows="4"
            placeholder="Ex: Não haverá aulas na próxima semana."
          />
        </UFormField>

        <UFormField label="Destinatários" name="targetUsers">
          <USelect
            v-model="formState.targetUsers"
            :items="targetUsersOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione os destinatários"
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
            label="Criar notificação"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
