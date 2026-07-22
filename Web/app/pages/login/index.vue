<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

definePageMeta({
  layout: false
})

const toast = useToast()
const router = useRouter()
const config = useRuntimeConfig()
const { fetchUser } = useAuth()

interface SocialLoginAvailability {
  googleEnabled: boolean
  googleClientId: string | null
}

const { data: socialLogin } = await useFetch<SocialLoginAvailability>(
  `${config.public.backendUrl}/identity/social-login/check-availability`,
  { default: () => ({ googleEnabled: false, googleClientId: null }) }
)

const { loggingIn: oneTapLoading, initOneTap } = useGoogleOneTap()
const googleRedirectLoading = ref(false)
const googleLoading = computed(() => oneTapLoading.value || googleRedirectLoading.value)

const schema = z.object({
  email: z.string({ error: 'Informe o email' }).min(1, 'Informe o email').email('Email inválido'),
  password: z.string({ error: 'Informe a senha' }).min(1, 'Informe a senha')
})
type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({ email: '', password: '' })
const loading = ref(false)

onMounted(async () => {
  if (!socialLogin.value?.googleEnabled || !socialLogin.value.googleClientId) return
  await initOneTap(socialLogin.value.googleClientId)
})

function loginWithGoogle() {
  googleRedirectLoading.value = true
  const email = state.email?.trim()
  const params = email ? `?email=${encodeURIComponent(email)}` : ''
  const url = `${config.public.backendUrl}/identity/social-login/challenge/Google${params}`
  requestAnimationFrame(() => { window.location.href = url })
}

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true

  try {
    await $fetch(`${config.public.backendUrl}/identity/email-password-login`, {
      method: 'POST',
      credentials: 'include',
      body: {
        email: event.data.email,
        password: event.data.password
      }
    })

    await fetchUser()
    await navigateTo('/home')
  } catch (error: any) {
    const errorData = typeof error?.data === 'string' ? JSON.parse(error.data) : error?.data

    if (errorData?.code === 'LoginRequiresTwoFactor') {
      router.push('/login/2fa')
      return
    }

    if (errorData?.code === 'LoginTwoFactorEnforced') {
      router.push('/login/setup-2fa')
      return
    }

    loading.value = false
    toast.add({
      title: 'Erro no login',
      description: errorData?.message || 'Email ou senha inválidos.',
      icon: 'i-lucide-x',
      color: 'error'
    })
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <Transition name="fade">
      <div
        v-if="oneTapLoading"
        class="fixed inset-0 z-[9999] flex flex-col items-center justify-center gap-4 bg-white dark:bg-gray-900"
      >
        <AppSpinner class="size-10 text-primary" />
        <p class="text-sm text-gray-600 dark:text-gray-400">
          Entrando com Google...
        </p>
      </div>
    </Transition>

    <AuthHeader />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[20vh]">
      <div class="w-full max-w-sm">
        <div class="text-center mb-8">
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
            Entrar
          </h1>
          <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
            Acesse sua conta Estud
          </p>
        </div>

        <UCard>
          <div class="flex flex-col gap-4">
            <UButton
              v-if="socialLogin?.googleEnabled"
              color="neutral"
              variant="outline"
              size="lg"
              block
              :loading="googleLoading"
              @click="loginWithGoogle"
            >
              <template #leading>
                <svg class="w-5 h-5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4" />
                  <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853" />
                  <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" fill="#FBBC05" />
                  <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335" />
                </svg>
              </template>
              Entrar com Google
            </UButton>

            <div v-if="socialLogin?.googleEnabled" class="flex items-center gap-3 my-2">
              <div class="flex-1 border-t border-gray-300 dark:border-gray-600" />
              <span class="text-sm text-gray-500">ou</span>
              <div class="flex-1 border-t border-gray-300 dark:border-gray-600" />
            </div>

            <UForm
              :schema="schema"
              :state="state"
              class="flex flex-col gap-4"
              @submit="onSubmit"
            >
              <UFormField label="Email" name="email" required>
                <UInput
                  v-model="state.email"
                  name="email"
                  type="email"
                  placeholder="seu@email.com"
                  icon="i-lucide-mail"
                  autocomplete="username"
                  size="lg"
                  class="w-full"
                  :disabled="googleRedirectLoading"
                />
              </UFormField>

              <UFormField label="Senha" name="password" required>
                <UInput
                  v-model="state.password"
                  name="password"
                  type="password"
                  placeholder="Sua senha"
                  icon="i-lucide-lock"
                  autocomplete="current-password"
                  size="lg"
                  class="w-full"
                  :disabled="googleRedirectLoading"
                />
              </UFormField>

              <div class="text-right">
                <NuxtLink
                  to="/forgot-password"
                  class="text-sm text-primary hover:underline"
                >
                  Esqueci minha senha
                </NuxtLink>
              </div>

              <UButton
                data-testid="login-button"
                type="submit"
                label="Entrar"
                size="lg"
                block
                :loading="loading"
                :disabled="googleRedirectLoading"
              />
            </UForm>
          </div>
        </UCard>
      </div>
    </div>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
