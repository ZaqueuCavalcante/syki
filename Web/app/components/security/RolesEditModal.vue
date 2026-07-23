<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const open = defineModel<boolean>('open', { default: false })
const props = defineProps<{ roleId: number | null }>()
const emit = defineEmits<{ updated: [] }>()

const isMobile = useIsMobile()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)
const fetching = ref(false)

interface PermissionItem {
  id: number
  name: string
  allowedTypes: string[]
}

interface GetPermissionsOut {
  items: PermissionItem[]
}

interface GetRoleOut {
  id: number
  name: string
  description: string
  baseType: string
  permissions: number[]
}

const { data: permissionsData } = await useFetch<GetPermissionsOut>(
  `${config.public.backendUrl}/identity/permissions`,
  { credentials: 'include', server: false }
)

const baseTypeLabels: Record<string, string> = {
  Manager: 'Gestor',
  Teacher: 'Professor',
  Student: 'Aluno',
  Parent: 'Responsável',
}

// Tipo base é imutável: apenas exibido, nunca enviado no update
const baseType = ref<string | null>(null)

const baseTypeLabel = computed(() => {
  if (baseType.value === null) return ''
  return baseTypeLabels[baseType.value] ?? baseType.value
})

const schema = z.object({
  name: z.string().min(1, 'Nome obrigatório').max(50, 'Máximo 50 caracteres'),
  description: z.string().min(1, 'Descrição obrigatória').max(200, 'Máximo 200 caracteres'),
  permissions: z.array(z.number()),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  name: '',
  description: '',
  permissions: [],
})

const filteredPermissions = computed(() => {
  if (baseType.value === null) return []
  return (permissionsData.value?.items ?? []).filter(p =>
    p.allowedTypes.includes(baseType.value!)
  )
})

function togglePermission(id: number) {
  const idx = formState.permissions!.indexOf(id)
  if (idx === -1) formState.permissions!.push(id)
  else formState.permissions!.splice(idx, 1)
}

watch(open, async (val) => {
  if (val && props.roleId !== null) {
    fetching.value = true
    try {
      const role = await $fetch<GetRoleOut>(
        `${config.public.backendUrl}/identity/roles/${props.roleId}`,
        { credentials: 'include' }
      )
      formState.name = role.name
      formState.description = role.description
      baseType.value = role.baseType
      formState.permissions = [...role.permissions]
      await nextTick()
    } catch {
      toast.add({ title: 'Erro ao carregar perfil', color: 'error' })
      open.value = false
    } finally {
      fetching.value = false
    }
  }
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/identity/roles`, {
      method: 'PUT',
      body: { id: props.roleId, ...event.data },
      credentials: 'include',
    })
    toast.add({ title: 'Perfil atualizado com sucesso', color: 'success' })
    open.value = false
    emit('updated')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao atualizar perfil.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    title="Editar perfil"
    :fullscreen="isMobile"
    description="Atualize os dados do perfil de acesso."
  >
    <template #body>
      <div v-if="fetching" class="flex justify-center py-8">
        <AppSpinner class="size-6" />
      </div>

      <UForm
        v-else
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

        <UFormField v-if="baseType !== null" label="Tipo base">
          <div class="flex items-center gap-2">
            <UBadge color="neutral" variant="subtle" :label="baseTypeLabel" />
            <span class="text-xs text-muted">Não pode ser alterado</span>
          </div>
        </UFormField>

        <UFormField v-if="baseType !== null" label="Permissões" name="permissions">
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
