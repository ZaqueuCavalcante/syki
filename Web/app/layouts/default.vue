<script setup lang="ts">
import type { PolicyName } from '~/policies'
import type { NavigationMenuItem } from '@nuxt/ui'

const open = ref(false)
const { can } = usePolicy()
const { account } = useUserAccount()
const { unreadCount } = useNotifications()
const { isNotificationsSlideoverOpen } = useDashboard()
const { classes: teacherClasses, fetchClasses: fetchTeacherClasses } = useTeacherClasses()

const allLinks = [
  { label: 'Home',          icon: 'i-lucide-house',          to: '/home',               policy: 'AccessHomePage'              as PolicyName },
  { label: 'Filhos',        icon: 'i-lucide-users-round',    to: '/children',           policy: 'AccessChildrenPage'          as PolicyName },
  { label: 'Campi',         icon: 'i-lucide-map-pin',        to: '/campi',              policy: 'AccessCampiPage'             as PolicyName },
  { label: 'Disciplinas',   icon: 'i-lucide-book-open',      to: '/disciplines',        policy: 'AccessDisciplinesPage'       as PolicyName },
  { label: 'Cursos',        icon: 'i-lucide-notebook',       to: '/courses',            policy: 'AccessCoursesPage'           as PolicyName },
  { label: 'Grades',        icon: 'i-lucide-layout-list',    to: '/course-curriculums', policy: 'AccessCourseCurriculumsPage' as PolicyName },
  { label: 'Períodos',      icon: 'i-lucide-calendar',       to: '/periods',            policy: 'AccessPeriodsPage'           as PolicyName },
  { label: 'Matrículas',    icon: 'i-lucide-clipboard-list', to: '/enrollments',        policy: 'AccessEnrollmentsPage'       as PolicyName },
  { label: 'Ofertas',       icon: 'i-lucide-library',        to: '/course-offerings',   policy: 'AccessCourseOfferingsPage'   as PolicyName },
  { label: 'Turmas',        icon: 'i-lucide-door-open',      to: '/classes',            policy: 'AccessClassesPage'           as PolicyName },
  { label: 'Salas',         icon: 'i-lucide-school',         to: '/classrooms',         policy: 'AccessClassroomsPage'        as PolicyName },
  { label: 'Professores',   icon: 'i-lucide-user-pen',       to: '/teachers',           policy: 'AccessTeachersPage'          as PolicyName },
  { label: 'Alunos',        icon: 'i-lucide-graduation-cap', to: '/students',           policy: 'AccessStudentsPage'          as PolicyName },
  { label: 'Responsáveis',  icon: 'i-lucide-users',          to: '/parents',            policy: 'AccessParentsPage'           as PolicyName },
  { label: 'Agenda',        icon: 'i-lucide-calendar-days',  to: '/agenda',             policy: 'AccessAgendaPage'            as PolicyName },
  { label: 'Frequência',    icon: 'i-lucide-calendar-check', to: '/frequencies',        policy: 'AccessFrequenciesPage'       as PolicyName },
  { label: 'Calendário',    icon: 'i-lucide-calendar-range', to: '/calendar',           policy: 'AccessCalendarPage'          as PolicyName },
  { label: 'Segurança',     icon: 'i-lucide-shield',         to: '/security',           policy: 'AccessSecurityPage'          as PolicyName },
  { label: 'Integrações',   icon: 'i-lucide-webhook',        to: '/integrations',       policy: 'AccessIntegrationsPage'      as PolicyName },
  { label: 'Notificações',  icon: 'i-lucide-bell',           to: '/notifications',      policy: 'AccessNotificationsPage'     as PolicyName },
  { label: 'Configurações', icon: 'i-lucide-settings',       to: '/configs',            policy: 'AccessConfigsPage'           as PolicyName },
]

const route = useRoute()

const canSeeTeacherClasses = can('GetTeacherCurrentClasses')

watch(account, () => { fetchTeacherClasses() }, { immediate: true })

const teacherClassesLinks = computed<NavigationMenuItem[]>(() =>
  teacherClasses.value.map(({ id, name }) => {
    const to = `/classes/${id}`
    return {
      label: name,
      to,
      active: route.path === to,
      onSelect: () => { open.value = false },
    }
  })
)

const links = computed<NavigationMenuItem[]>(() => {
  const items: NavigationMenuItem[] = allLinks
    .filter(({ policy }) => can(policy).value)
    .map(({ label, icon, to }) => ({
      label,
      icon,
      to,
      active: route.path === to || route.path.startsWith(`${to}/`),
      onSelect: () => { open.value = false },
    }))

  if (canSeeTeacherClasses.value) {
    items.push({
      label: 'Turmas',
      icon: 'i-lucide-door-open',
      defaultOpen: true,
      children: teacherClassesLinks.value,
    })
  }

  return items
})

const groups = computed(() => [{
  id: 'links',
  label: 'Go to',
  items: [
    ...allLinks
      .filter(({ policy }) => can(policy).value)
      .map(({ label, icon, to }) => ({
        label,
        icon,
        to,
        onSelect: () => { open.value = false },
      })),
    ...teacherClassesLinks.value.map(item => ({ ...item, icon: 'i-lucide-door-open' })),
  ],
}])
</script>

<template>
  <UDashboardGroup unit="rem">
    <UDashboardSidebar
      id="default"
      v-model:open="open"
      toggle-side="right"
      collapsible
      resizable
      class="bg-elevated/25"
      :ui="{ footer: 'lg:border-t lg:border-default' }"
    >
      <template #header="{ collapsed }">
        <TeamsMenu :collapsed="collapsed" />
      </template>

      <template #default="{ collapsed }">
        <UNavigationMenu
          :collapsed="collapsed"
          :items="links"
          orientation="vertical"
          tooltip
          popover
        />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="[{ label: 'Documentação', icon: 'i-lucide-book-open', to: '/docs', target: '_blank' }]"
          orientation="vertical"
          :external-icon="false"
          tooltip
          class="mt-auto"
        />
      </template>

      <template #footer="{ collapsed }">
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <UDashboardSearch :groups="groups" />

    <NotificationsSlideover />

    <div class="fixed top-4 right-6 z-50 flex items-center gap-3">
      <ChildrenSelector />
      <UChip
        :text="unreadCount > 99 ? '99+' : unreadCount"
        :show="unreadCount > 0"
        color="error"
        size="2xl"
        inset
      >
        <UTooltip text="Notificações">
          <UButton
            icon="i-lucide-bell"
            color="neutral"
            variant="ghost"
            :square="true"
            @click="(e: MouseEvent) => { (e.currentTarget as HTMLElement).blur(); isNotificationsSlideoverOpen = true }"
          />
        </UTooltip>
      </UChip>
    </div>

    <slot />
  </UDashboardGroup>
</template>
