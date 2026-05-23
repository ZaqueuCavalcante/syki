<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const courseTypes = [
  { label: 'Bacharelado', value: 'Bacharelado' },
  { label: 'Licenciatura', value: 'Licenciatura' },
  { label: 'Tecnólogo', value: 'Tecnologo' },
  { label: 'Especialização', value: 'Especializacao' },
  { label: 'Mestrado', value: 'Mestrado' },
  { label: 'Doutorado', value: 'Doutorado' },
  { label: 'Pós-Doutorado', value: 'PosDoutorado' },
]

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  type: z.string().min(1, 'Tipo obrigatório'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  type: undefined,
})

function resetForm() {
  formState.name = ''
  formState.type = undefined
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/courses`, {
      method: 'POST',
      body: { name: event.data.name, type: event.data.type, disciplines: [] },
      credentials: 'include',
    })
    toast.add({ title: 'Curso criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar curso.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo curso"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar um novo curso."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Análise e Desenvolvimento de Sistemas" />
        </UFormField>

        <UFormField label="Tipo" name="type">
          <USelect
            v-model="formState.type"
            :items="courseTypes"
            value-key="value"
            class="w-full"
            placeholder="Selecione o tipo"
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
            label="Criar curso"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
