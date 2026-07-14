<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const props = defineProps<{ activityId: number | string }>()

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const schema = z.object({
  link: z.string({ error: 'Campo obrigatório' })
    .min(1, 'Link obrigatório')
    .max(500, 'Máximo 500 caracteres')
    .url('Link inválido'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  link: '',
})

function resetForm() {
  formState.link = ''
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/students/activities/${props.activityId}/works`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Entrega feita com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao entregar atividade.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Nova entrega"
    :fullscreen="isMobile"
    description="Informe o link do material entregue (PDF, Doc, PPT)."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Link" name="link" required>
          <UInput
            v-model="formState.link"
            class="w-full"
            placeholder="Ex: https://github.com/ZaqueuCavalcante/estud"
          />
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton label="Cancelar" color="neutral" variant="subtle" :disabled="loading" @click="() => { open = false }" />
          <UButton label="Salvar" type="submit" :loading="loading" />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
