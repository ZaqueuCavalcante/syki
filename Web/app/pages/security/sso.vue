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
  <div class="w-full lg:max-w-2xl mx-auto">
    <div class="flex justify-end mb-4">
      <UButton
        v-if="!ssoConfig"
        icon="i-lucide-plus"
        label="Configuração"
        @click="createModalOpen = true"
      />
    </div>

    <template v-if="ssoConfig">
      <UPageCard
        title="Configuração SSO"
        description="Provedor de Single Sign-On configurado para esta organização."
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
        <div class="flex justify-between items-center py-1">
          <span class="text-sm font-medium">Provedor</span>
          <span class="text-sm text-muted">{{ providerLabels[ssoConfig.providerType] }}</span>
        </div>
        <USeparator />
        <div class="flex justify-between items-center py-1">
          <span class="text-sm font-medium">Authority URL</span>
          <span class="text-sm text-muted truncate max-w-xs">{{ ssoConfig.authority }}</span>
        </div>
        <USeparator />
        <div class="flex justify-between items-center py-1">
          <span class="text-sm font-medium">Client ID</span>
          <span class="text-sm text-muted font-mono">{{ ssoConfig.clientId }}</span>
        </div>
        <USeparator />
        <div class="flex justify-between items-center py-1">
          <span class="text-sm font-medium">SSO Obrigatório</span>
          <UBadge
            :label="ssoConfig.requireSso ? 'Sim' : 'Não'"
            :color="ssoConfig.requireSso ? 'success' : 'neutral'"
            variant="subtle"
          />
        </div>
        <USeparator />
        <div class="flex justify-between items-center py-1">
          <span class="text-sm font-medium">Configuração Ativa</span>
          <UBadge
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
