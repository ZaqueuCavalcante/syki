<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

definePageMeta({
  layout: false
})

const toast = useToast()
const router = useRouter()
const route = useRoute()
const config = useRuntimeConfig()

const token = route.query.token as string | undefined

const schema = z.object({
  password: z.string({ error: 'Informe a nova senha' }).min(1, 'Informe a nova senha'),
  confirm: z.string({ error: 'Confirme a nova senha' }).min(1, 'Confirme a nova senha')
})
type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({ password: '', confirm: '' })
const loading = ref(false)
const success = ref(false)

async function onSubmit(event: FormSubmitEvent<Schema>) {
  if (event.data.password !== event.data.confirm) {
    toast.add({
      title: 'Erro',
      description: 'As senhas não coincidem.',
      icon: 'i-lucide-x',
      color: 'error'
    })
    return
  }

  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/identity/reset-password`, {
      method: 'POST',
      body: { token, password: event.data.password }
    })
    success.value = true
  } catch (error: any) {
    const errorData = typeof error?.data === 'string' ? JSON.parse(error.data) : error?.data
    toast.add({
      title: 'Erro ao redefinir senha',
      description: errorData?.message || 'Não foi possível redefinir a senha.',
      icon: 'i-lucide-x',
      color: 'error'
    })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <AuthHeader v-if="!success" />

    <div class="flex items-start justify-center px-4 pt-4 md:pt-[20vh]">
      <div class="w-full max-w-sm">
        <!-- Token ausente ou inválido -->
        <template v-if="!token">
          <div class="text-center space-y-4">
            <UIcon name="i-lucide-circle-x" class="size-12 text-error mx-auto" />
            <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
              Link inválido
            </h1>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              Este link de redefinição é inválido ou expirou.
            </p>
            <UButton
              label="Solicitar novo link"
              block
              @click="router.push('/forgot-password')"
            />
          </div>
        </template>

        <!-- Sucesso -->
        <template v-else-if="success">
          <div class="text-center space-y-4">
            <UIcon name="i-lucide-circle-check" class="size-12 text-primary mx-auto" />
            <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
              Senha redefinida!
            </h1>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              Sua senha foi alterada com sucesso.
            </p>
            <UButton
              label="Ir para o login"
              block
              @click="router.push('/login')"
            />
          </div>
        </template>

        <!-- Formulário -->
        <template v-else>
          <div class="text-center mb-8">
            <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
              Nova senha
            </h1>
            <p class="mt-2 text-sm text-gray-600 dark:text-gray-400">
              Escolha uma senha forte para sua conta
            </p>
          </div>

          <UCard>
            <UForm
              :schema="schema"
              :state="state"
              class="flex flex-col gap-4"
              @submit="onSubmit"
            >
              <UFormField label="Nova senha" name="password" required>
                <UInput
                  v-model="state.password"
                  name="password"
                  type="password"
                  placeholder="Nova senha"
                  icon="i-lucide-lock"
                  autocomplete="new-password"
                  size="lg"
                  class="w-full"
                />
              </UFormField>

              <UFormField label="Confirmar senha" name="confirm" required>
                <UInput
                  v-model="state.confirm"
                  name="confirm"
                  type="password"
                  placeholder="Repita a nova senha"
                  icon="i-lucide-lock-keyhole"
                  autocomplete="new-password"
                  size="lg"
                  class="w-full"
                />
              </UFormField>

              <p class="text-xs text-gray-500 dark:text-gray-400">
                A senha deve conter letras maiúsculas, minúsculas, números e caracteres especiais.
              </p>

              <UButton
                type="submit"
                label="Redefinir senha"
                size="lg"
                block
                :loading="loading"
              />
            </UForm>
          </UCard>
        </template>
      </div>
    </div>
  </div>
</template>
