<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const open = ref(false)

const links = [[{
  label: 'Home',
  icon: 'i-lucide-house',
  to: '/',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Inbox',
  icon: 'i-lucide-inbox',
  to: '/inbox',
  badge: '4',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Customers',
  icon: 'i-lucide-users',
  to: '/customers',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Campi',
  icon: 'i-lucide-map-pin',
  to: '/campi',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Disciplinas',
  icon: 'i-lucide-book-open',
  to: '/disciplines',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Cursos',
  icon: 'i-lucide-graduation-cap',
  to: '/courses',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Ofertas',
  icon: 'i-lucide-library',
  to: '/course-offerings',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Grades',
  icon: 'i-lucide-layout-list',
  to: '/course-curriculums',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Períodos',
  icon: 'i-lucide-calendar',
  to: '/periods',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Professores',
  icon: 'i-lucide-user-pen',
  to: '/teachers',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Alunos',
  icon: 'i-lucide-user-round',
  to: '/students',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Segurança',
  icon: 'i-lucide-shield',
  to: '/security',
  onSelect: () => {
    open.value = false
  }
}, {
  label: 'Settings',
  to: '/settings',
  icon: 'i-lucide-settings',
  defaultOpen: true,
  type: 'trigger',
  children: [{
    label: 'General',
    to: '/settings',
    exact: true,
    onSelect: () => {
      open.value = false
    }
  }, {
    label: 'Members',
    to: '/settings/members',
    onSelect: () => {
      open.value = false
    }
  }, {
    label: 'Notifications',
    to: '/settings/notifications',
    onSelect: () => {
      open.value = false
    }
  }, {
    label: 'Security',
    to: '/settings/security',
    onSelect: () => {
      open.value = false
    }
  }]
}], [{
  label: 'Feedback',
  icon: 'i-lucide-message-circle',
  to: 'https://github.com/nuxt-ui-templates/dashboard',
  target: '_blank'
}, {
  label: 'Help & Support',
  icon: 'i-lucide-info',
  to: 'https://github.com/nuxt-ui-templates/dashboard',
  target: '_blank'
}]] satisfies NavigationMenuItem[][]

const groups = computed(() => [{
  id: 'links',
  label: 'Go to',
  items: links.flat()
}])
</script>

<template>
  <UDashboardGroup unit="rem">
    <UDashboardSidebar
      id="default"
      v-model:open="open"
      collapsible
      resizable
      class="bg-elevated/25"
      :ui="{ footer: 'lg:border-t lg:border-default' }"
    >
      <template #header="{ collapsed }">
        <TeamsMenu :collapsed="collapsed" />
      </template>

      <template #default="{ collapsed }">
        <UDashboardSearchButton :collapsed="collapsed" class="bg-transparent ring-default" />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[0]"
          orientation="vertical"
          tooltip
          popover
        />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[1]"
          orientation="vertical"
          tooltip
          class="mt-auto"
        />
      </template>

      <template #footer="{ collapsed }">
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <UDashboardSearch :groups="groups" />

    <slot />

    <NotificationsSlideover />
  </UDashboardGroup>
</template>
