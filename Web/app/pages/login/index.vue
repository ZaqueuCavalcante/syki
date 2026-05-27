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

const schema = z.object({
  email: z.string({ error: 'Informe o email' }).min(1, 'Informe o email').email('Email inválido'),
  password: z.string({ error: 'Informe a senha' }).min(1, 'Informe a senha')
})
type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({ email: '', password: '' })
const loading = ref(false)
const loginSuccess = ref(false)

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

    loginSuccess.value = true
    await fetchUser()
    router.push('/')
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
    <AuthHeader v-if="!loginSuccess" />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[20vh]">
      <div v-if="loginSuccess" class="text-center space-y-4">
        <UIcon name="i-lucide-loader-2" class="size-12 text-primary-500 animate-spin" />
        <p class="text-gray-600 dark:text-gray-400 text-lg">
          Preparando tudo pra você...
        </p>
      </div>

      <div v-else class="w-full max-w-sm">
        <div class="text-center mb-8">
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
            Entrar
          </h1>
          <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
            Acesse sua conta Syki
          </p>
        </div>

        <UCard>
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
            />
          </UForm>
        </UCard>
      </div>
    </div>
  </div>
</template>
