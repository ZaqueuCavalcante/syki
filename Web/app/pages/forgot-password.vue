<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

definePageMeta({
  layout: false
})

const router = useRouter()
const config = useRuntimeConfig()

const schema = z.object({
  email: z.string({ error: 'Informe o email' }).min(1, 'Informe o email').email('Email inválido')
})
type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({ email: '' })
const loading = ref(false)
const sent = ref(false)

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/identity/reset-password-token`, {
      method: 'POST',
      body: { email: event.data.email }
    })
  } catch {
    // Sempre exibir confirmação para não revelar se o email está cadastrado
  } finally {
    sent.value = true
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <AuthHeader />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[20vh]">
      <div class="w-full max-w-sm">
        <!-- Confirmação de envio -->
        <template v-if="sent">
          <div class="text-center space-y-4">
            <UIcon name="i-lucide-mail-check" class="size-12 text-primary mx-auto" />
            <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
              Verifique seu email
            </h1>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              Se o email informado estiver cadastrado, você receberá um link para redefinir sua senha.
            </p>
            <UButton
              label="Voltar ao login"
              variant="ghost"
              block
              @click="router.push('/login')"
            />
          </div>
        </template>

        <!-- Formulário de solicitação -->
        <template v-else>
          <div class="text-center mb-8">
            <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
              Esqueceu a senha?
            </h1>
            <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
              Informe seu email e enviaremos um link para redefinição
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

              <UButton
                type="submit"
                label="Enviar link"
                size="lg"
                block
                :loading="loading"
              />

              <UButton
                type="button"
                label="Voltar ao login"
                variant="ghost"
                size="sm"
                block
                @click="router.push('/login')"
              />
            </UForm>
          </UCard>
        </template>
      </div>
    </div>
  </div>
</template>
