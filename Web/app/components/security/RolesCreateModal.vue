<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const availablePermissions = [
  { id: 0,   label: 'Gerenciar perfis de acesso' },
  { id: 1,   label: 'Gerenciar SSO' },
  { id: 100, label: 'Gerenciar usuários' },
  { id: 200, label: 'Gerenciar campus' },
  { id: 300, label: 'Gerenciar disciplinas' },
  { id: 400, label: 'Gerenciar cursos' },
  { id: 500, label: 'Gerenciar professores' },
  { id: 600, label: 'Gerenciar alunos' },
  { id: 700, label: 'Gerenciar períodos acadêmicos' },
]

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
  description: z.string().min(1, 'Descrição obrigatória').max(200, 'Máximo 200 caracteres'),
  permissions: z.array(z.number()),
})

type Schema = z.output<typeof schema>

const formState = reactive<Schema>({
  name: '',
  description: '',
  permissions: [],
})

function togglePermission(id: number) {
  const idx = formState.permissions.indexOf(id)
  if (idx === -1) formState.permissions.push(id)
  else formState.permissions.splice(idx, 1)
}

function resetForm() {
  formState.name = ''
  formState.description = ''
  formState.permissions = []
}

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/identity/roles`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Perfil criado com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar perfil.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Novo perfil"
    :fullscreen="isMobile"
    description="Preencha os dados para cadastrar um novo perfil de acesso."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Nome" name="name">
          <UInput v-model="formState.name" class="w-full" placeholder="Ex: Coordenador" />
        </UFormField>

        <UFormField label="Descrição" name="description">
          <UTextarea v-model="formState.description" class="w-full" placeholder="Ex: Perfil com acesso a cursos e disciplinas" :rows="3" />
        </UFormField>

        <UFormField label="Permissões" name="permissions">
          <div class="flex flex-col gap-2 w-full">
            <UCheckbox
              v-for="perm in availablePermissions"
              :key="perm.id"
              :label="perm.label"
              :checked="formState.permissions.includes(perm.id)"
              @update:checked="togglePermission(perm.id)"
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
            label="Criar perfil"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
