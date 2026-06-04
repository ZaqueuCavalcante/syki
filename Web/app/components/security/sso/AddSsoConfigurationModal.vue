<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import type { SsoProviderType } from '~/types'

const open = defineModel<boolean>('open', { default: false })
const emit = defineEmits<{ created: [] }>()

const isMobile = useIsMobile()
const popoverMode = usePopoverMode()
const config = useRuntimeConfig()
const toast = useToast()
const loading = ref(false)

const requireSsoInfo = {
  active: 'Usuários do domínio configurado devem obrigatoriamente fazer login via SSO.',
  inactive: 'Usuários podem escolher entre login tradicional ou SSO.',
}

const providerOptions = [
  { label: 'Azure AD', value: 'AzureAd' },
  { label: 'Google Workspace', value: 'GoogleWorkspace' },
  { label: 'Okta', value: 'Okta' },
  { label: 'Auth0', value: 'Auth0' },
  { label: 'OIDC Personalizado', value: 'CustomOidc' },
]

const schema = z.object({
  providerType: z.enum(['AzureAd', 'GoogleWorkspace', 'Okta', 'Auth0', 'CustomOidc'], {
    error: 'Provedor inválido',
  }),
  authority: z.string({ error: 'Authority obrigatória' })
    .min(1, 'Authority obrigatória')
    .url('Authority deve ser uma URL válida'),
  clientId: z.string({ error: 'Client ID obrigatório' })
    .min(1, 'Client ID obrigatório'),
  clientSecret: z.string({ error: 'Client Secret obrigatório' })
    .min(1, 'Client Secret obrigatório'),
  requireSso: z.boolean().default(false),
})

type Schema = z.output<typeof schema>

const formState = reactive<Partial<Schema>>({
  providerType: undefined,
  authority: undefined,
  clientId: undefined,
  clientSecret: undefined,
  requireSso: false,
})

function resetForm() {
  formState.providerType = undefined
  formState.authority = undefined
  formState.clientId = undefined
  formState.clientSecret = undefined
  formState.requireSso = false
}

const providerAuthority: Partial<Record<string, string>> = {
  GoogleWorkspace: 'https://accounts.google.com/o/oauth2/v2/auth',
}

watch(() => formState.providerType, (provider) => {
  if (provider && providerAuthority[provider]) {
    formState.authority = providerAuthority[provider]
  }
})

watch(open, (val) => {
  if (!val) resetForm()
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/identity/sso/configurations`, {
      method: 'POST',
      body: event.data,
      credentials: 'include',
    })
    toast.add({ title: 'Configuração SSO criada com sucesso', color: 'success' })
    open.value = false
    emit('created')
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar configuração SSO.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    :fullscreen="isMobile"
    title="Nova Configuração SSO"
    description="Configure um provedor de Single Sign-On."
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="formState"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Provedor" name="providerType">
          <USelect
            v-model="formState.providerType as SsoProviderType"
            :items="providerOptions"
            placeholder="Selecione o provedor"
            class="w-full"
            autofocus
          />
        </UFormField>

        <UFormField label="Authority URL" name="authority">
          <UInput
            v-model="formState.authority"
            placeholder="https://login.microsoftonline.com/tenant-id/v2.0"
            class="w-full"
          />
        </UFormField>

        <UFormField label="Client ID" name="clientId">
          <UInput
            v-model="formState.clientId"
            placeholder="00000000-0000-0000-0000-000000000000"
            autocomplete="one-time-code"
            class="w-full"
          />
        </UFormField>

        <UFormField label="Client Secret" name="clientSecret">
          <UInput
            v-model="formState.clientSecret"
            placeholder="Insira o client secret"
            autocomplete="off"
            class="w-full"
          />
        </UFormField>

        <UFormField name="requireSso">
          <div class="flex items-center justify-between w-full">
            <UCheckbox v-model="formState.requireSso" label="SSO Obrigatório" />
            <UPopover :content="{ side: 'top', align: 'end' }" :mode="popoverMode" :open-delay="0">
              <UIcon name="i-lucide-info" class="size-4 text-muted" />
              <template #content>
                <div class="p-3 space-y-3 w-72">
                  <div class="flex items-start gap-2">
                    <UIcon name="i-lucide-check-circle" class="size-4 text-success shrink-0 mt-0.5" />
                    <div>
                      <p class="text-xs font-semibold text-success">Ativo</p>
                      <p class="text-xs text-muted">{{ requireSsoInfo.active }}</p>
                    </div>
                  </div>
                  <div class="flex items-start gap-2">
                    <UIcon name="i-lucide-x-circle" class="size-4 text-neutral shrink-0 mt-0.5" />
                    <div>
                      <p class="text-xs font-semibold">Inativo</p>
                      <p class="text-xs text-muted">{{ requireSsoInfo.inactive }}</p>
                    </div>
                  </div>
                </div>
              </template>
            </UPopover>
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
            label="Criar"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
