<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

definePageMeta({ layout: 'landing' })

useSeoMeta({
  title: 'Criar conta — Estud',
  description: 'Crie sua conta gratuitamente e comece a usar o Estud.',
})

const config = useRuntimeConfig()
const toast = useToast()

const schema = z.object({
  email: z.string().email('E-mail inválido')
})

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({ email: '' })
const loading = ref(false)
const success = ref(false)

async function onSubmit(event: FormSubmitEvent<Schema>) {
  loading.value = true
  try {
    await $fetch(`${config.public.backendUrl}/users/register`, {
      method: 'POST',
      body: { email: event.data.email }
    })
    success.value = true
  } catch (err: unknown) {
    const msg = (err as { data?: { message?: string } })?.data?.message ?? 'Erro ao criar conta. Tente novamente.'
    toast.add({ title: 'Erro', description: msg, color: 'error' })
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-[calc(100vh-4rem)] flex items-center justify-center px-4 py-16">
    <UCard class="w-full max-w-md">
      <template #header>
        <div class="text-center space-y-1">
          <h1 class="text-2xl font-bold">
            Criar conta
          </h1>
          <p class="text-muted text-sm">
            Comece gratuitamente, sem cartão de crédito.
          </p>
        </div>
      </template>

      <div v-if="success" class="py-4 text-center space-y-3">
        <UIcon name="i-lucide-mail-check" class="size-12 text-primary mx-auto" />
        <h2 class="text-lg font-semibold">
          Verifique seu e-mail
        </h2>
        <p class="text-muted text-sm">
          Enviamos um link de acesso para <strong>{{ state.email }}</strong>.<br>
          Clique no link para concluir seu cadastro.
        </p>
      </div>

      <UForm
        v-else
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="E-mail" name="email">
          <UInput
            v-model="state.email"
            type="email"
            placeholder="seu@email.com"
            class="w-full"
            autofocus
          />
        </UFormField>

        <UButton
          type="submit"
          label="Criar conta"
          class="w-full justify-center"
          :loading="loading"
        />
      </UForm>

      <template #footer>
        <p class="text-center text-sm text-muted">
          Já tem uma conta?
          <NuxtLink to="/login" class="text-primary hover:underline font-medium">
            Entrar
          </NuxtLink>
        </p>
      </template>
    </UCard>
  </div>
</template>
