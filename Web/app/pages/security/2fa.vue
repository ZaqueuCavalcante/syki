<script setup lang="ts">
import type { GetTwoFactorEnforcementOut, SetTwoFactorEnforcementOut, TwoFactorEnforcementItem } from '~/types'

const config = useRuntimeConfig()
const toast = useToast()

const { data, status, refresh } = await useFetch<GetTwoFactorEnforcementOut | null>(
  `${config.public.backendUrl}/identity/2fa-enforcement`,
  { credentials: 'include', server: false },
)

const savingRoleId = ref<number | null>(null)

const baseTypeLabels: Record<string, string> = {
  Manager: 'Gestor',
  Teacher: 'Professor',
  Student: 'Aluno',
  Parent: 'Responsável',
}

async function toggleRole(role: TwoFactorEnforcementItem, required: boolean) {
  savingRoleId.value = role.roleId

  try {
    await $fetch<SetTwoFactorEnforcementOut>(
      `${config.public.backendUrl}/identity/2fa-enforcement`,
      {
        method: 'PUT',
        credentials: 'include',
        body: { roleId: role.roleId, required },
      },
    )

    role.twoFactorRequired = required

    toast.add({
      title: 'Salvo',
      description: required
        ? `2FA agora é obrigatório para o perfil "${role.name}".`
        : `2FA não é mais obrigatório para o perfil "${role.name}".`,
      icon: 'i-lucide-check',
      color: 'success',
    })
  } catch {
    toast.add({
      title: 'Erro',
      description: 'Não foi possível atualizar a obrigatoriedade de 2FA.',
      icon: 'i-lucide-x',
      color: 'error',
    })
    await refresh()
  } finally {
    savingRoleId.value = null
  }
}
</script>

<template>
  <div class="w-full lg:max-w-2xl mx-auto min-w-0">
    <UPageCard
      title="Autenticação de dois fatores"
      description="Escolha quais perfis de acesso são obrigados a usar 2FA no login."
      variant="naked"
      class="mb-4"
    />

    <template v-if="data && data.items.length">
      <UPageCard variant="subtle">
        <template v-for="(role, index) in data.items" :key="role.roleId">
          <USeparator v-if="index > 0" />
          <div class="flex items-center justify-between gap-3 py-1">
            <div class="flex flex-col min-w-0">
              <span class="text-sm font-medium truncate">{{ role.name }}</span>
              <span class="text-xs text-muted">{{ baseTypeLabels[role.baseType] ?? role.baseType }}</span>
            </div>
            <USwitch
              :model-value="role.twoFactorRequired"
              :disabled="savingRoleId !== null"
              @update:model-value="(value: boolean) => { toggleRole(role, value) }"
            />
          </div>
        </template>
      </UPageCard>
    </template>

    <TableEmptyState
      v-else
      :loading="status === 'pending'"
      icon="i-lucide-shield-check"
      message="Nenhum perfil de acesso cadastrado"
    />
  </div>
</template>
