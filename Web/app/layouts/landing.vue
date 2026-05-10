<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const links: NavigationMenuItem[] = [
  { label: 'Funcionalidades', to: '/landing#features' },
  { label: 'Preços', to: '/landing#pricing' },
]
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <UHeader title="Estud">
      <UNavigationMenu :items="links" />

      <template #right>
        <UColorModeButton />
        <UButton color="neutral" variant="ghost" to="/login" class="hidden sm:flex">
          Entrar
        </UButton>
        <UButton to="/register">
          Começar grátis
        </UButton>
      </template>

      <template #body>
        <UNavigationMenu :items="links" orientation="vertical" class="justify-start" />
        <div class="flex flex-col gap-2 mt-4">
          <UButton color="neutral" variant="ghost" to="/login" block>
            Entrar
          </UButton>
          <UButton to="/register" block>
            Começar grátis
          </UButton>
        </div>
      </template>
    </UHeader>

    <UMain class="flex-1">
      <slot />
    </UMain>

    <UFooter>
      <template #left>
        <p class="text-sm text-muted">
          © {{ new Date().getFullYear() }} Estud. Todos os direitos reservados.
        </p>
      </template>

      <template #right>
        <div class="flex items-center gap-4 text-sm text-muted">
          <NuxtLink to="#" class="hover:text-default transition-colors">Termos</NuxtLink>
          <NuxtLink to="#" class="hover:text-default transition-colors">Privacidade</NuxtLink>
          <NuxtLink to="#" class="hover:text-default transition-colors">Contato</NuxtLink>
        </div>
      </template>
    </UFooter>
  </div>
</template>
