import type { PolicyName } from '~/policies'
import type { UserType } from '~/composables/useUserAccount'

const routePolicies: Record<string, PolicyName> = {
  '/home': 'AccessHomePage',
  '/classes': 'AccessClassesPage',
  '/classrooms': 'AccessClassroomsPage',
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
  '/configs': 'AccessConfigsPage',
}

// A página de detalhe da turma é a mesma rota para os 3 perfis, mas cada um
// consome um endpoint próprio — a policy exigida depende do UserType.
const classDetailPolicies: Record<UserType, PolicyName> = {
  Manager: 'AccessClassesPage',
  Teacher: 'GetTeacherClass',
  Student: 'GetStudentClass',
}

export default defineNuxtRouteMiddleware(async (to) => {
  if (import.meta.server) return

  const routePolicy = routePolicies[to.path]
  const isClassDetail = to.path.startsWith('/classes/')
  const isClassroomDetail = to.path.startsWith('/classrooms/')
  if (!routePolicy && !isClassDetail && !isClassroomDetail) return

  const { account, fetchAccount } = useUserAccount()

  if (!account.value) {
    try {
      await fetchAccount()
    } catch {
      return navigateTo('/')
    }
  }

  const policyName = routePolicy
    ?? (isClassroomDetail ? 'GetClassroom' as PolicyName : classDetailPolicies[account.value!.userType])
  if (!policyName) return

  const { can } = usePolicy()
  if (!can(policyName).value) {
    return navigateTo('/home')
  }
})
