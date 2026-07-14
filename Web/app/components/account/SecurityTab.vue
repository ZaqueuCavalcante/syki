<script setup lang="ts">
const toast = useToast()
const { getTwoFactorKey, setupTwoFactor } = useAuth()

const isMobile = useIsMobile()
const twoFactorEnabled = ref(false)
const isModalOpen = ref(false)
const isLoading = ref(true)
const isSettingUp = ref(false)

const qrCodeBase64 = ref('')
const secretKey = ref('')
const verificationToken = ref<string[]>([])

onMounted(async () => {
  isLoading.value = true
  try {
    const data = await getTwoFactorKey()
    twoFactorEnabled.value = data.twoFactorEnabled
    qrCodeBase64.value = data.qrCodeBase64
    secretKey.value = data.key
  } catch {
    toast.add({ title: 'Não foi possível carregar o status do 2FA', color: 'error' })
  } finally {
    isLoading.value = false
  }
})

function openSetupModal() {
  verificationToken.value = []
  isModalOpen.value = true
}

async function onSetup() {
  if (verificationToken.value.length !== 6 || verificationToken.value.some(t => !t)) {
    toast.add({ title: 'Erro', description: 'Insira o código de 6 dígitos.', color: 'error' })
    return
  }

  isSettingUp.value = true
  try {
    const success = await setupTwoFactor(verificationToken.value.join(''))
    if (success) {
      twoFactorEnabled.value = true
      isModalOpen.value = false
      toast.add({ title: '2FA ativado com sucesso', color: 'success' })
    } else {
      toast.add({ title: 'Código inválido', description: 'O código inserido está incorreto. Tente novamente.', color: 'error' })
      verificationToken.value = []
    }
  } finally {
    isSettingUp.value = false
  }
}

function copyKey() {
  navigator.clipboard.writeText(secretKey.value)
  toast.add({ title: 'Chave copiada', color: 'success' })
}
</script>

<template>
  <div>
    <UPageCard
      title="Autenticação de Dois Fatores (2FA)"
      description="Um segundo fator de autenticação aumenta a segurança da sua conta."
      variant="naked"
      class="mb-4"
    />

    <UPageCard variant="subtle">
    <div v-if="isLoading" class="flex items-center justify-center py-8">
      <AppSpinner class="size-6" />
    </div>

    <div v-else class="flex flex-col gap-4">
      <div v-if="twoFactorEnabled" class="flex items-center gap-3 p-4 rounded-lg bg-green-50 dark:bg-green-900/20">
        <UIcon name="i-lucide-shield-check" class="text-green-600 dark:text-green-400 text-xl" />
        <span class="text-sm text-green-700 dark:text-green-300">
          Sua conta está protegida com autenticação de dois fatores.
        </span>
      </div>

      <div v-else class="flex items-center gap-3 p-4 rounded-lg bg-amber-50 dark:bg-amber-900/20">
        <UIcon name="i-lucide-shield-alert" class="text-amber-600 dark:text-amber-400 text-xl" />
        <span class="text-sm text-amber-700 dark:text-amber-300">
          Recomendamos ativar o 2FA para maior segurança.
        </span>
      </div>

      <UButton
        v-if="!twoFactorEnabled"
        label="Ativar 2FA"
        icon="i-lucide-shield-plus"
        class="w-fit ml-auto"
        @click="openSetupModal"
      />
    </div>
    </UPageCard>
  </div>

  <UModal
    v-model:open="isModalOpen"
    :fullscreen="isMobile"
    title="Configurar 2FA"
    description="Configure a autenticação de dois fatores para sua conta"
  >
    <template #body>
      <div class="flex flex-col gap-6">
        <div class="text-sm text-muted">
          <p class="mb-2">1. Escaneie o QR Code abaixo com seu app autenticador (Google Authenticator, Authy, etc.)</p>
          <p>2. Insira o código de 6 dígitos gerado pelo app para confirmar a ativação.</p>
        </div>

        <div class="flex justify-center">
          <img
            v-if="qrCodeBase64"
            :src="qrCodeBase64"
            alt="QR Code 2FA"
            class="w-48 h-48 rounded-lg border border-default"
          >
        </div>

        <div class="flex flex-col gap-2">
          <p class="text-xs text-muted text-center">Ou insira a chave manualmente:</p>
          <div class="flex items-center gap-2 justify-center">
            <code class="text-xs bg-elevated px-2 py-1 rounded font-mono">{{ secretKey }}</code>
            <UTooltip text="Copiar">
              <UButton icon="i-lucide-copy" variant="ghost" size="xs" @click="copyKey" />
            </UTooltip>
          </div>
        </div>

        <div class="flex flex-col items-center gap-2">
          <p class="text-sm font-medium">Código de verificação</p>
          <UPinInput
            v-model="verificationToken"
            type="number"
            otp
            :length="6"
            :disabled="isSettingUp"
            @complete="onSetup"
          />
        </div>

        <div class="flex justify-end gap-2">
          <UButton
            label="Cancelar"
            color="neutral"
            variant="subtle"
            :disabled="isSettingUp"
            @click="() => { isModalOpen = false }"
          />
          <UButton label="Ativar 2FA" :loading="isSettingUp" @click="onSetup" />
        </div>
      </div>
    </template>
  </UModal>
</template>
