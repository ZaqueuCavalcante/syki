<script setup lang="ts">
import type { PolicyName } from '~/policies'

const open = ref(false)
const { can } = usePolicy()

const allLinks = [
  { label: 'Home',        icon: 'i-lucide-house',          to: '/',                   policy: 'AccessHomePage'              as PolicyName },
  // { label: 'Inbox',    icon: 'i-lucide-inbox',          to: '/inbox',    badge: '4', policy: 'AccessHomePage'             as PolicyName },
  // { label: 'Customers', icon: 'i-lucide-users',         to: '/customers',           policy: 'AccessHomePage'             as PolicyName },
  { label: 'Campi',       icon: 'i-lucide-map-pin',         to: '/campi',              policy: 'AccessCampiPage'             as PolicyName },
  { label: 'Disciplinas', icon: 'i-lucide-book-open',       to: '/disciplines',        policy: 'AccessDisciplinesPage'       as PolicyName },
  { label: 'Cursos',      icon: 'i-lucide-graduation-cap',  to: '/courses',            policy: 'AccessCoursesPage'           as PolicyName },
  { label: 'Grades',      icon: 'i-lucide-layout-list',     to: '/course-curriculums', policy: 'AccessCourseCurriculumsPage' as PolicyName },
  { label: 'Períodos',    icon: 'i-lucide-calendar',        to: '/periods',            policy: 'AccessPeriodsPage'           as PolicyName },
  { label: 'Ofertas',     icon: 'i-lucide-library',         to: '/course-offerings',   policy: 'AccessCourseOfferingsPage'   as PolicyName },
  { label: 'Professores', icon: 'i-lucide-user-pen',        to: '/teachers',           policy: 'AccessTeachersPage'          as PolicyName },
  { label: 'Alunos',      icon: 'i-lucide-user-round',      to: '/students',           policy: 'AccessStudentsPage'          as PolicyName },
  { label: 'Segurança',   icon: 'i-lucide-shield',          to: '/security',           policy: 'AccessSecurityPage'          as PolicyName },
]

const links = computed(() =>
  allLinks
    .filter(({ policy }) => can(policy).value)
    .map(({ label, icon, to }) => ({
      label,
      icon,
      to,
      onSelect: () => { open.value = false },
    }))
)

const groups = computed(() => [{
  id: 'links',
  label: 'Go to',
  items: links.value,
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
          :items="links"
          orientation="vertical"
          tooltip
          popover
        />

        <!-- <UNavigationMenu
          :collapsed="collapsed"
          :items="links[1]"
          orientation="vertical"
          tooltip
          class="mt-auto"
        /> -->
      </template>

      <template #footer="{ collapsed }">
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <UDashboardSearch :groups="groups" />

    <slot />
  </UDashboardGroup>
</template>
