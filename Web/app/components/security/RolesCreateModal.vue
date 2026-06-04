<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

interface PermissionItem {
  id: number
  name: string
  allowedTypes: string[]
}

interface GetPermissionsOut {
  items: PermissionItem[]
}

const { data: permissionsData } = await useFetch<GetPermissionsOut>(
  `${config.public.backendUrl}/identity/permissions`,
  { credentials: 'include', server: false }
)

const baseTypeOptions = [
  { label: 'Gestor', value: 0 },
  { label: 'Professor', value: 1 },
  { label: 'Aluno', value: 2 },
]

const userTypeNames: Record<number, string> = { 0: 'Manager', 1: 'Teacher', 2: 'Student' }

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
  description: z.string().min(1, 'Descrição obrigatória').max(200, 'Máximo 200 caracteres'),
  baseType: z.number({ error: 'Tipo base obrigatório' }),
  permissions: z.array(z.number()),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  description: '',
  baseType: undefined,
  permissions: [],
})

const filteredPermissions = computed(() => {
  if (formState.baseType === undefined) return []
  const typeName = userTypeNames[formState.baseType]
  return (permissionsData.value?.items ?? []).filter(p =>
    p.allowedTypes.includes(typeName)
  )
})

watch(() => formState.baseType, () => {
  formState.permissions = []
})

function togglePermission(id: number) {
  const idx = formState.permissions!.indexOf(id)
  if (idx === -1) formState.permissions!.push(id)
  else formState.permissions!.splice(idx, 1)
}

function resetForm() {
  formState.name = ''
  formState.description = ''
  formState.baseType = undefined
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

        <UFormField label="Tipo base" name="baseType">
          <USelect
            v-model="formState.baseType"
            :items="baseTypeOptions"
            value-key="value"
            class="w-full"
            placeholder="Selecione"
          />
        </UFormField>

        <UFormField v-if="formState.baseType !== undefined" label="Permissões" name="permissions">
          <div class="flex flex-col gap-2 w-full">
            <UCheckbox
              v-for="perm in filteredPermissions"
              :key="perm.id"
              :label="perm.name"
              :model-value="formState.permissions!.includes(perm.id)"
              @update:model-value="togglePermission(perm.id)"
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
