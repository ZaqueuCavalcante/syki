<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface TeacherItem {
  id: number
  name: string
  email: string
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ teacher: TeacherItem | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
  email: z.string().min(1, 'Email obrigatório').email('Email inválido'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  email: '',
})

watch(open, (val) => {
  if (val && props.teacher) {
    formState.name = props.teacher.name
    formState.email = props.teacher.email
  } else {
    formState.name = ''
    formState.email = ''
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/teachers/${props.teacher!.id}`, {
      method: 'PUT',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Professor atualizado com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar professor.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar professor"
    :fullscreen="isMobile"
    description="Edite o nome e email do professor."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Richard Feynman" />
        </UFormField>

        <UFormField label="Email" name="email">
          <UInput v-model="formState.email" type="email" class="w-full" placeholder="Ex: professor@instituicao.edu.br" />
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
