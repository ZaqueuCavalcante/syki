<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const links = [[{
  label: 'Perfis',
  icon: 'i-lucide-user-cog',
  to: '/security',
  exact: true,
}, {
  label: 'SSO',
  icon: 'i-lucide-key-round',
  to: '/security/sso',
}, {
  label: '2FA',
  icon: 'i-lucide-shield-check',
  to: '/security/2fa',
}]] satisfies NavigationMenuItem[][]
</script>

<template>
  <UDashboardPanel id="security">
    <template #header>
      <UDashboardNavbar title="Segurança">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>

      <UDashboardToolbar>
        <UNavigationMenu :items="links" highlight class="-mx-1 flex-1" />
      </UDashboardToolbar>
    </template>

    <template #body>
      <div class="flex flex-col gap-4 sm:gap-6 w-full min-w-0">
        <NuxtPage />
      </div>
    </template>
  </UDashboardPanel>
</template>
