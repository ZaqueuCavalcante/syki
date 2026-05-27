<script setup lang="ts">
definePageMeta({
  layout: false
})

const toast = useToast()
const router = useRouter()
const { fetchUser, twoFactorLogin } = useAuth()

const token = ref<string[]>([])
const loading = ref(false)
const loginSuccess = ref(false)
const pinInputRef = ref()

onMounted(() => {
  nextTick(() => {
    pinInputRef.value?.$el?.querySelector('input')?.focus()
  })
})

async function onSubmit() {
  const tokenString = token.value.join('')
  if (tokenString.length !== 6) {
    toast.add({
      title: 'Erro',
      description: 'Insira o código de 6 dígitos.',
      icon: 'i-lucide-x',
      color: 'error'
    })
    return
  }

  loading.value = true
  const success = await twoFactorLogin(tokenString)

  if (success) {
    loginSuccess.value = true
    await fetchUser()
    router.push('/')
  } else {
    loading.value = false
    toast.add({
      title: 'Código inválido',
      description: 'O código inserido está inválido.',
      icon: 'i-lucide-x',
      color: 'error'
    })
    token.value = []
  }
}

function goBack() {
  router.push('/login')
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <AuthHeader v-if="!loginSuccess" />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[20vh]">
      <div class="w-full max-w-sm">
        <div class="text-center mb-8">
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
            Verificação 2FA
          </h1>
          <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
            Insira o código do seu app autenticador
          </p>
        </div>

        <UCard>
          <Transition name="login-transition" mode="out-in">
            <div v-if="loginSuccess" key="success" class="flex flex-col items-center justify-center py-12 gap-4">
              <UIcon name="i-lucide-loader-circle" class="text-primary size-10 animate-spin" />
              <p class="text-lg font-medium text-gray-900 dark:text-white">
                Entrando...
              </p>
            </div>
            <form
              v-else
              key="form"
              class="flex flex-col gap-6"
              @submit.prevent="onSubmit"
            >
              <div class="flex flex-col items-center gap-2">
                <UPinInput
                  ref="pinInputRef"
                  v-model="token"
                  type="number"
                  otp
                  :length="6"
                  :disabled="loading"
                />
              </div>

              <UButton
                type="submit"
                label="Verificar"
                block
                :loading="loading"
              />

              <UButton
                type="button"
                label="Voltar ao login"
                variant="ghost"
                block
                @click="goBack"
              />
            </form>
          </Transition>
        </UCard>
      </div>
    </div>
  </div>
</template>

<style scoped>
.login-transition-enter-active,
.login-transition-leave-active {
  transition: opacity 0.2s ease;
}
.login-transition-enter-from,
.login-transition-leave-to {
  opacity: 0;
}
</style>
