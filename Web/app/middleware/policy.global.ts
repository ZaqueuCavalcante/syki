import type { PolicyName } from '~/policies'

const routePolicies: Record<string, PolicyName> = {
  '/home': 'AccessHomePage',
  '/classes': 'AccessClassesPage',
  '/campi': 'AccessCampiPage',
  '/courses': 'AccessCoursesPage',
  '/periods': 'AccessPeriodsPage',
  '/enrollments': 'AccessEnrollmentsPage',
  '/security/sso': 'AccessSsoPage',
  '/security': 'AccessSecurityPage',
  '/teachers': 'AccessTeachersPage',
  '/students': 'AccessStudentsPage',
  '/disciplines': 'AccessDisciplinesPage',
  '/course-offerings': 'AccessCourseOfferingsPage',
  '/course-curriculums': 'AccessCourseCurriculumsPage',
  '/notifications': 'AccessNotificationsPage',
  '/agenda': 'AccessAgendaPage',
  '/calendar': 'AccessCalendarPage',
}

export default defineNuxtRouteMiddleware(async (to) => {
  if (import.meta.server) return

  const policyName = routePolicies[to.path]
    ?? (to.path.startsWith('/classes/') ? 'AccessClassesPage' : undefined)
  if (!policyName) return

  const { account, fetchAccount } = useUserAccount()

  if (!account.value) {
    try {
      await fetchAccount()
    } catch {
      return navigateTo('/')
    }
  }

  const { can } = usePolicy()
  if (!can(policyName).value) {
    return navigateTo('/home')
  }
})
