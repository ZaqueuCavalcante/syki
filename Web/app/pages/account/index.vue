<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const { account, fetchAccount, updateAccount } = useUserAccount()

if (!account.value) await fetchAccount()

const isMobile = useIsMobile()
const toast = useToast()
const editNameOpen = ref(false)
const loading = ref(false)

const schema = z.object({
  name: z.string({ error: 'Campo obrigatório' }).min(1, 'Nome obrigatório').max(100, 'Máximo 100 caracteres'),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({ name: '' })

watch(editNameOpen, (val) => {
  formState.name = val ? (account.value?.name ?? '') : ''
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await updateAccount(event.data.name)
    toast.add({ title: 'Nome atualizado com sucesso', color: 'success' })
    editNameOpen.value = false
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar nome.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div>
    <UPageCard
      title="Geral"
      description="Informações da sua conta."
      variant="naked"
      class="mb-4"
    />

    <UPageCard variant="subtle">
    <div class="flex flex-col divide-y divide-default">
      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Nome</p>
        </div>
        <div class="flex items-center gap-2">
          <p class="text-sm text-muted">{{ account?.name }}</p>
          <UButton
            icon="i-lucide-pencil"
            size="xs"
            color="neutral"
            variant="ghost"
            aria-label="Editar nome"
            @click="() => { editNameOpen = true }"
          />
        </div>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Email</p>
        </div>
        <p class="text-sm text-muted">{{ account?.email }}</p>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Instituição</p>
        </div>
        <p class="text-sm text-muted">{{ account?.institution }}</p>
      </div>

      <div class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Perfil de acesso</p>
        </div>
        <p class="text-sm text-muted">{{ account?.role }}</p>
      </div>

      <div v-if="account?.course" class="flex max-sm:flex-col justify-between items-start gap-4 py-4 first:pt-0 last:pb-0">
        <div>
          <p class="text-sm font-medium text-highlighted">Curso</p>
        </div>
        <p class="text-sm text-muted">{{ account?.course }}</p>
      </div>
    </div>
    </UPageCard>
  </div>

  <UModal
    v-model:open="editNameOpen"
    title="Editar nome"
    :fullscreen="isMobile"
    description="Atualize o seu nome de exibição."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Seu nome" />
        </UFormField>

        <div class="flex justify-end gap-2 pt-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="() => { editNameOpen = false }"
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
