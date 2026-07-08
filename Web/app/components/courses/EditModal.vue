<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

interface CourseItem {
  id: number
  name: string
  typeValue: string
}

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ course: CourseItem | null }>()
const emit = defineEmits<{ updated: [] }>()

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
  type: z.string({ error: 'Tipo obrigatório' }).min(1, 'Tipo obrigatório'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  type: undefined,
})

watch(open, (val) => {
  if (val && props.course) {
    formState.name = props.course.name
    formState.type = props.course.typeValue
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/courses`, {
      method: 'PUT',
      body: { id: props.course!.id, name: event.data.name, type: event.data.type },
      credentials: 'include',
    })
    toast.add({ title: 'Curso atualizado com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar curso.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar curso"
    :fullscreen="isMobile"
    description="Atualize os dados do curso."
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
