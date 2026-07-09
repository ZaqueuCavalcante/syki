<script setup lang="ts">
import type { GetSsoConfigurationOut } from '~/types'

const config = useRuntimeConfig()
const createModalOpen = ref(false)
const editModalOpen = ref(false)

const { data: ssoConfig, status, refresh } = await useFetch<GetSsoConfigurationOut | null>(
  `${config.public.backendUrl}/identity/sso/configuration`,
  { credentials: 'include', server: false },
)

const providerLabels: Record<string, string> = {
  AzureAd: 'Azure AD',
  GoogleWorkspace: 'Google Workspace',
  Okta: 'Okta',
  Auth0: 'Auth0',
  CustomOidc: 'OIDC Personalizado',
}
</script>

<template>
  <div class="w-full lg:max-w-2xl mx-auto min-w-0">
    <template v-if="ssoConfig">
      <UPageCard
        title="Configuração Single Sign-On"
        description="Provedor configurado para sua instituição."
        variant="naked"
        orientation="horizontal"
        class="mb-4"
      >
        <UButton
          icon="i-lucide-pencil"
          label="Editar"
          color="neutral"
          variant="subtle"
          class="w-fit lg:ms-auto"
          @click="editModalOpen = true"
        />
      </UPageCard>

      <UPageCard variant="subtle">
        <div class="flex flex-col gap-1 sm:flex-row sm:justify-between sm:items-center sm:gap-3 py-1">
          <span class="text-sm font-medium shrink-0">Provedor</span>
          <span class="text-sm text-muted break-words sm:text-right">{{ providerLabels[ssoConfig.providerType] }}</span>
        </div>
        <USeparator />
        <div class="flex flex-col gap-1 sm:flex-row sm:justify-between sm:items-center sm:gap-3 py-1">
          <span class="text-sm font-medium shrink-0">Authority URL</span>
          <span class="text-sm text-muted break-all min-w-0 sm:text-right">{{ ssoConfig.authority }}</span>
        </div>
        <USeparator />
        <div class="flex flex-col gap-1 sm:flex-row sm:justify-between sm:items-center sm:gap-3 py-1">
          <span class="text-sm font-medium shrink-0">Client ID</span>
          <span class="text-sm text-muted font-mono break-all min-w-0 sm:text-right">{{ ssoConfig.clientId }}</span>
        </div>
        <USeparator />
        <div class="flex flex-col gap-1 sm:flex-row sm:justify-between sm:items-center sm:gap-3 py-1">
          <span class="text-sm font-medium shrink-0">SSO Obrigatório</span>
          <UBadge
            class="w-fit"
            :label="ssoConfig.requireSso ? 'Sim' : 'Não'"
            :color="ssoConfig.requireSso ? 'success' : 'neutral'"
            variant="subtle"
          />
        </div>
        <USeparator />
        <div class="flex flex-col gap-1 sm:flex-row sm:justify-between sm:items-center sm:gap-3 py-1">
          <span class="text-sm font-medium shrink-0">Configuração Ativa</span>
          <UBadge
            class="w-fit"
            :label="ssoConfig.isActive ? 'Sim' : 'Não'"
            :color="ssoConfig.isActive ? 'success' : 'neutral'"
            variant="subtle"
          />
        </div>
      </UPageCard>
    </template>

    <TableEmptyState
      v-else
      :loading="status === 'pending'"
      icon="i-lucide-key-round"
      message="Nenhuma configuração SSO cadastrada"
      button-label="Configuração"
      @create="createModalOpen = true"
    />
  </div>

  <SecuritySsoAddSsoConfigurationModal v-model:open="createModalOpen" @created="refresh()" />
  <SecuritySsoEditSsoConfigurationModal
    v-if="ssoConfig"
    :sso-config="ssoConfig"
    v-model:open="editModalOpen"
    @updated="refresh()"
  />
</template>
