<script setup lang="ts">
definePageMeta({ layout: 'landing' })

useSeoMeta({ title: 'Entrando - Estud' })

const config = useRuntimeConfig()
const route = useRoute()
const { fetchAccount } = useUserAccount()

const token = route.query.token as string | undefined

const error = ref(false)

onMounted(async () => {
  if (!token) {
    error.value = true
    return
  }

  try {
    await $fetch(`${config.public.backendUrl}/identity/magic-link-login`, {
      method: 'POST',
      body: { token },
      credentials: 'include'
    })
    await fetchAccount()
    await navigateTo('/home')
  } catch {
    error.value = true
  }
})
</script>

<template>
  <div class="min-h-[calc(100vh-4rem)] flex items-center justify-center px-4">
    <UCard class="w-full max-w-sm text-center">
      <div v-if="!error" class="py-6 space-y-4">
        <AppSpinner class="size-10 text-primary mx-auto" />
        <p class="text-muted text-sm">
          Autenticando, aguarde...
        </p>
      </div>

      <div v-else class="py-6 space-y-4">
        <UIcon name="i-lucide-circle-x" class="size-10 text-error mx-auto" />
        <div class="space-y-1">
          <h2 class="text-lg font-semibold">
            Link inválido ou expirado
          </h2>
          <p class="text-muted text-sm">
            Solicite um novo link de acesso.
          </p>
        </div>
        <UButton to="/register" variant="outline" color="neutral" label="Voltar ao início" />
      </div>
    </UCard>
  </div>
</template>
