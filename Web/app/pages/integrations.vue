<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const links = [[{
  label: 'Webhooks',
  icon: 'i-lucide-webhook',
  to: '/integrations',
  exact: true,
}, {
  label: 'Chamadas',
  icon: 'i-lucide-radio',
  to: '/integrations/calls',
  exact: true,
}]] satisfies NavigationMenuItem[][]
</script>

<template>
  <UDashboardPanel id="integrations">
    <template #header>
      <UDashboardNavbar title="Integrações">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>

      <UDashboardToolbar>
        <UNavigationMenu :items="links" highlight class="-mx-1 flex-1" />
      </UDashboardToolbar>
    </template>

    <template #body>
      <div class="flex flex-col gap-4 sm:gap-6 w-full">
        <NuxtPage />
      </div>
    </template>
  </UDashboardPanel>
</template>
