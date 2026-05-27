<script setup lang="ts">
definePageMeta({
  layout: false
})

const toast = useToast()
const router = useRouter()
const { getTwoFactorKey, setupTwoFactor } = useAuth()

const isLoading = ref(true)
const isSettingUp = ref(false)
const setupSuccess = ref(false)

const qrCodeBase64 = ref('')
const secretKey = ref('')
const verificationToken = ref<string[]>([])

onMounted(async () => {
  try {
    const data = await getTwoFactorKey()
    qrCodeBase64.value = data.qrCodeBase64
    secretKey.value = data.key
  } catch {
    toast.add({
      title: 'Erro',
      description: 'Não foi possível carregar as informações do 2FA. Faça login novamente.',
      icon: 'i-lucide-x',
      color: 'error'
    })
    router.push('/login')
  } finally {
    isLoading.value = false
  }
})

async function onSetup() {
  if (verificationToken.value.length !== 6 || verificationToken.value.some(t => !t)) {
    toast.add({
      title: 'Erro',
      description: 'Insira o código de 6 dígitos.',
      icon: 'i-lucide-x',
      color: 'error'
    })
    return
  }

  isSettingUp.value = true

  try {
    const tokenString = verificationToken.value.join('')
    const success = await setupTwoFactor(tokenString)

    if (success) {
      setupSuccess.value = true
      toast.add({
        title: 'Sucesso',
        description: '2FA ativado com sucesso! Faça login novamente.',
        icon: 'i-lucide-check',
        color: 'success'
      })
      router.push('/login')
    } else {
      toast.add({
        title: 'Código inválido',
        description: 'O código inserido está incorreto. Tente novamente.',
        icon: 'i-lucide-x',
        color: 'error'
      })
      verificationToken.value = []
    }
  } finally {
    isSettingUp.value = false
  }
}

function copyKey() {
  navigator.clipboard.writeText(secretKey.value)
  toast.add({
    title: 'Copiado',
    description: 'Chave copiada.',
    icon: 'i-lucide-copy',
    color: 'success'
  })
}

function goBack() {
  router.push('/login')
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <AuthHeader v-if="!setupSuccess" />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[2vh]">
      <div class="w-full max-w-lg">
        <div class="text-center mb-8">
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
            Configurar 2FA
          </h1>
          <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
            Sua organização exige autenticação de dois fatores
          </p>
        </div>

        <UCard>
          <Transition name="setup-transition" mode="out-in">
            <div v-if="setupSuccess" key="success" class="flex flex-col items-center justify-center py-12 gap-4">
              <UIcon name="i-lucide-loader-circle" class="text-primary size-10 animate-spin" />
              <p class="text-lg font-medium text-gray-900 dark:text-white">
                Redirecionando...
              </p>
            </div>

            <div v-else-if="isLoading" key="loading" class="flex items-center justify-center py-12">
              <UIcon name="i-lucide-loader-2" class="animate-spin text-2xl" />
            </div>

            <div v-else key="setup" class="flex flex-col gap-6">
              <div class="flex items-center gap-3 p-4 rounded-lg bg-amber-50 dark:bg-amber-900/20">
                <UIcon name="i-lucide-shield-alert" class="text-amber-600 dark:text-amber-400 text-xl shrink-0" />
                <span class="text-sm text-amber-700 dark:text-amber-300">
                  Configure o 2FA para continuar acessando o sistema.
                </span>
              </div>

              <div class="text-sm text-gray-600 dark:text-gray-400">
                <p class="mb-2">
                  1. Escaneie o QR Code abaixo com seu app autenticador (Google Authenticator, Authy, etc.)
                </p>
                <p>2. Insira o código de 6 dígitos gerado pelo app para confirmar a ativação.</p>
              </div>

              <div class="flex justify-center">
                <img
                  v-if="qrCodeBase64"
                  :src="qrCodeBase64"
                  alt="QR Code 2FA"
                  class="w-48 h-48 rounded-lg border dark:border-gray-700"
                >
              </div>

              <div class="flex flex-col gap-2">
                <p class="text-xs text-gray-500 dark:text-gray-400 text-center">
                  Ou insira a chave manualmente:
                </p>
                <div class="flex items-center gap-2 justify-center">
                  <code class="text-xs bg-gray-100 dark:bg-gray-800 px-2 py-1 rounded font-mono">
                    {{ secretKey }}
                  </code>
                  <UButton
                    icon="i-lucide-copy"
                    variant="ghost"
                    size="xs"
                    @click="copyKey"
                  />
                </div>
              </div>

              <div class="flex flex-col items-center gap-2">
                <p class="text-sm font-medium">
                  Código de verificação
                </p>
                <UPinInput
                  v-model="verificationToken"
                  type="number"
                  otp
                  :length="6"
                  :disabled="isSettingUp"
                  @complete="onSetup"
                />
              </div>

              <UButton
                label="Ativar 2FA"
                block
                :loading="isSettingUp"
                @click="onSetup"
              />

              <UButton
                label="Voltar ao login"
                variant="ghost"
                block
                @click="goBack"
              />
            </div>
          </Transition>
        </UCard>
      </div>
    </div>
  </div>
</template>

<style scoped>
.setup-transition-enter-active,
.setup-transition-leave-active {
  transition: opacity 0.2s ease;
}
.setup-transition-enter-from,
.setup-transition-leave-to {
  opacity: 0;
}
</style>
