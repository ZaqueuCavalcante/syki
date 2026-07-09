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
    toast.add({ title: 'Erro', description: 'Insira o código de 6 dígitos.', color: 'error' })
    return
  }

  loading.value = true
  const success = await twoFactorLogin(tokenString)

  if (success) {
    loginSuccess.value = true
    await fetchUser()
    router.push('/home')
  } else {
    loading.value = false
    toast.add({ title: 'Código inválido', description: 'O código inserido está incorreto. Tente novamente.', color: 'error' })
    token.value = []
  }
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
          <div v-if="loginSuccess" class="flex flex-col items-center justify-center py-12 gap-4">
            <AppSpinner class="text-primary size-10" />
            <p class="text-lg font-medium text-gray-900 dark:text-white">
              Entrando...
            </p>
          </div>

          <form
            v-else
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
                @complete="onSubmit"
              />
            </div>

            <UButton type="submit" label="Verificar" block :loading="loading" />

            <UButton
              type="button"
              label="Voltar ao login"
              variant="ghost"
              block
              @click="router.push('/login')"
            />
          </form>
        </UCard>
      </div>
    </div>
  </div>
</template>
