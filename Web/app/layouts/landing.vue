<script setup lang="ts">
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <UHeader title="Estud">
      <template #right>
        <UTooltip text="Documentação">
          <UButton icon="i-lucide-book-open" color="neutral" variant="ghost" to="/docs" aria-label="Documentação" />
        </UTooltip>
        <UTooltip text="Alternar tema">
          <UColorModeButton />
        </UTooltip>
        <UButton to="/login">
          Entrar
        </UButton>
      </template>

      <template #body>
        <div class="flex flex-col gap-2 mt-4">
          <UButton to="/login" block>
            Entrar
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
          <NuxtLink to="/terms-of-service" class="hover:text-default transition-colors">Termos</NuxtLink>
          <NuxtLink to="/privacy-policy" class="hover:text-default transition-colors">Privacidade</NuxtLink>
          <NuxtLink to="/contact" class="hover:text-default transition-colors">Contato</NuxtLink>
        </div>
      </template>
    </UFooter>
  </div>
</template>
