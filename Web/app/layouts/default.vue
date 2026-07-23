<script setup lang="ts">
import type { PolicyName } from '~/policies'
import type { NavigationMenuItem } from '@nuxt/ui'

const open = ref(false)
const { can } = usePolicy()
const { account } = useUserAccount()
const { unreadCount } = useNotifications()
const { isNotificationsSlideoverOpen } = useDashboard()
const { classes: teacherClasses, fetchClasses: fetchTeacherClasses } = useTeacherClasses()

interface SidebarLink {
  label: string
  icon: string
  to: string
  policy: PolicyName
}

interface SidebarGroup {
  id: string
  label?: string
  // Só os grupos com título são colapsáveis, e aí o ícone é obrigatório: com a
  // sidebar colapsada ele vira o gatilho do popover com os itens do grupo.
  icon?: string
  items: SidebarLink[]
}

const TEACHER_CLASSES_GROUP_ID = 'turmas'

// O primeiro grupo não tem título: pro Manager ele fica vazio (e é descartado),
// e pros demais perfis (Professor, Aluno, Responsável) é a sidebar inteira —
// poucos itens, que não ganham nada em serem categorizados. A Home não está
// aqui: o logo do Estud no topo já leva pra ela.
const sidebarGroups: SidebarGroup[] = [
  {
    id: 'geral',
    items: [
      { label: 'Filhos',        icon: 'i-lucide-users-round',    to: '/children',           policy: 'AccessChildrenPage' },
      { label: 'Agenda',        icon: 'i-lucide-calendar-days',  to: '/agenda',             policy: 'AccessAgendaPage' },
      { label: 'Frequência',    icon: 'i-lucide-calendar-check', to: '/frequencies',        policy: 'AccessFrequenciesPage' },
    ],
  },
  {
    id: 'academico',
    label: 'Acadêmico',
    icon: 'i-lucide-book-marked',
    items: [
      { label: 'Cursos',        icon: 'i-lucide-notebook',       to: '/courses',            policy: 'AccessCoursesPage' },
      { label: 'Grades',        icon: 'i-lucide-layout-list',    to: '/course-curriculums', policy: 'AccessCourseCurriculumsPage' },
      { label: 'Disciplinas',   icon: 'i-lucide-book-open',      to: '/disciplines',        policy: 'AccessDisciplinesPage' },
    ],
  },
  {
    id: 'secretaria',
    label: 'Secretaria',
    icon: 'i-lucide-folder-open',
    items: [
      { label: 'Períodos',      icon: 'i-lucide-calendar',       to: '/periods',            policy: 'AccessPeriodsPage' },
      { label: 'Turmas',        icon: 'i-lucide-door-open',      to: '/classes',            policy: 'AccessClassesPage' },
      { label: 'Ofertas',       icon: 'i-lucide-library',        to: '/course-offerings',   policy: 'AccessCourseOfferingsPage' },
      { label: 'Matrículas',    icon: 'i-lucide-clipboard-list', to: '/enrollments',        policy: 'AccessEnrollmentsPage' },
      { label: 'Calendário',    icon: 'i-lucide-calendar-range', to: '/calendar',           policy: 'AccessCalendarPage' },
      { label: 'Notificações',  icon: 'i-lucide-bell',           to: '/notifications',      policy: 'AccessNotificationsPage' },
    ],
  },
  {
    id: 'pessoas',
    label: 'Pessoas',
    icon: 'i-lucide-contact',
    items: [
      { label: 'Alunos',        icon: 'i-lucide-graduation-cap', to: '/students',           policy: 'AccessStudentsPage' },
      { label: 'Professores',   icon: 'i-lucide-user-pen',       to: '/teachers',           policy: 'AccessTeachersPage' },
      { label: 'Responsáveis',  icon: 'i-lucide-users',          to: '/parents',            policy: 'AccessParentsPage' },
    ],
  },
  {
    id: 'infraestrutura',
    label: 'Infraestrutura',
    icon: 'i-lucide-building-2',
    items: [
      { label: 'Campi',         icon: 'i-lucide-map-pin',        to: '/campi',              policy: 'AccessCampiPage' },
      { label: 'Salas',         icon: 'i-lucide-school',         to: '/classrooms',         policy: 'AccessClassroomsPage' },
    ],
  },
  {
    id: 'sistema',
    label: 'Sistema',
    icon: 'i-lucide-cog',
    items: [
      { label: 'Segurança',     icon: 'i-lucide-shield',         to: '/security',           policy: 'AccessSecurityPage' },
      { label: 'Integrações',   icon: 'i-lucide-webhook',        to: '/integrations',       policy: 'AccessIntegrationsPage' },
      { label: 'Configurações', icon: 'i-lucide-settings',       to: '/configs',            policy: 'AccessConfigsPage' },
    ],
  },
]

const route = useRoute()

// Grupos que sobraram depois do filtro de permissões. Grupos sem nenhum item
// visível são descartados, pra um perfil restrito não ver um título órfão.
const visibleGroups = computed<SidebarGroup[]>(() =>
  sidebarGroups
    .map(group => ({ ...group, items: group.items.filter(({ policy }) => can(policy).value) }))
    .filter(group => group.items.length > 0)
)

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

function isLinkActive(to: string) {
  return route.path === to || route.path.startsWith(`${to}/`)
}

function toLink({ label, icon, to }: SidebarLink): NavigationMenuItem {
  return {
    label,
    icon,
    to,
    active: isLinkActive(to),
    onSelect: () => { open.value = false },
  }
}

// Quais grupos estão expandidos. O accordion do UNavigationMenu é
// não-controlado por padrão, e o `defaultOpen` dos itens só é lido na
// montagem — como os grupos só aparecem depois que a conta carrega, nesse
// momento a lista ainda está vazia e tudo nasceria fechado. Controlando via
// v-model o estado fica correto e ainda sobrevive ao reload.
const collapsibleGroupIds = [
  ...sidebarGroups.filter(group => group.label).map(group => group.id),
  TEACHER_CLASSES_GROUP_ID,
]
const openGroups = useLocalStorage<string[]>('estud-sidebar-open-groups', collapsibleGroupIds)

// Uma lista só: os itens soltos no topo seguidos dos grupos colapsáveis. Se
// fossem listas separadas o UNavigationMenu desenharia um separador entre
// elas. Grupo sem título (o primeiro) não vira accordion.
const links = computed<NavigationMenuItem[]>(() => {
  const ungrouped = visibleGroups.value.filter(group => !group.label)
  const grouped = visibleGroups.value.filter(group => group.label)

  return [
    ...ungrouped.flatMap(group => group.items.map(toLink)),
    ...grouped.map(group => ({
      value: group.id,
      label: group.label,
      icon: group.icon,
      children: group.items.map(toLink),
    })),
    ...(canSeeTeacherClasses.value
      ? [{
          value: TEACHER_CLASSES_GROUP_ID,
          label: 'Turmas',
          icon: 'i-lucide-door-open',
          children: teacherClassesLinks.value,
        }]
      : []),
  ]
})

// Navegar pra dentro de um grupo fechado (pelo Ctrl+K ou por um link interno)
// abre o grupo, senão o item ativo ficaria escondido. Só adiciona — colapsar
// um grupo estando numa página dele continua valendo.
watch([() => route.path, visibleGroups], () => {
  const activeGroup = visibleGroups.value.find(
    group => group.label && group.items.some(({ to }) => isLinkActive(to))
  )
  if (activeGroup && !openGroups.value.includes(activeGroup.id)) {
    openGroups.value = [...openGroups.value, activeGroup.id]
  }
}, { immediate: true })

const groups = computed(() => [
  ...visibleGroups.value.map(group => ({
    id: group.id,
    label: group.label,
    items: group.items.map(({ label, icon, to }) => ({
      label,
      icon,
      to,
      onSelect: () => { open.value = false },
    })),
  })),
  ...(canSeeTeacherClasses.value
    ? [{
        id: 'turmas',
        label: 'Turmas',
        items: teacherClassesLinks.value.map(item => ({ ...item, icon: 'i-lucide-door-open' })),
      }]
    : []),
])
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
          v-model="openGroups"
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
        :text="unreadCount > 9 ? '+9' : unreadCount"
        :show="unreadCount > 0"
        color="error"
        size="3xl"
        :ui="{ base: 'font-semibold text-xs leading-none h-3.5 min-w-3.5 px-1 rounded-full -translate-x-0.5 translate-y-0.5 pointer-events-none' }"
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
