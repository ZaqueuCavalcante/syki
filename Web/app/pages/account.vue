<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const links = [[{
  label: 'Geral',
  icon: 'i-lucide-user',
  to: '/account',
  exact: true
}, {
  label: 'Notificações',
  icon: 'i-lucide-bell',
  to: '/account/notifications'
}, {
  label: 'Segurança',
  icon: 'i-lucide-shield',
  to: '/account/security'
}]
] satisfies NavigationMenuItem[][]
</script>

<template>
  <UDashboardPanel id="account" :ui="{ body: 'lg:py-12' }">
    <template #header>
      <UDashboardNavbar title="Conta">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>

      <UDashboardToolbar>
        <!-- NOTE: The `-mx-1` class is used to align with the `DashboardSidebarCollapse` button here. -->
        <UNavigationMenu :items="links" highlight class="-mx-1 flex-1" />
      </UDashboardToolbar>
    </template>

    <template #body>
      <div class="flex flex-col gap-4 sm:gap-6 lg:gap-12 w-full lg:max-w-2xl mx-auto">
        <NuxtPage />
      </div>
    </template>
  </UDashboardPanel>
</template>
