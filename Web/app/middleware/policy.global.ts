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
  '/security/2fa': 'AccessTwoFactorEnforcementPage',
  '/security': 'AccessSecurityPage',
  '/teachers': 'AccessTeachersPage',
  '/students': 'AccessStudentsPage',
  '/parents': 'AccessParentsPage',
  '/disciplines': 'AccessDisciplinesPage',
  '/course-offerings': 'AccessCourseOfferingsPage',
  '/course-curriculums': 'AccessCourseCurriculumsPage',
  '/notifications': 'AccessNotificationsPage',
  '/agenda': 'AccessAgendaPage',
  '/frequencies': 'AccessFrequenciesPage',
  '/calendar': 'AccessCalendarPage',
  '/configs': 'AccessConfigsPage',
  '/children': 'AccessChildrenPage',
}

// A página de detalhe da turma é a mesma rota para os 3 perfis, mas cada um
// consome um endpoint próprio — a policy exigida depende do UserType.
const classDetailPolicies: Record<UserType, PolicyName> = {
  Manager: 'AccessClassesPage',
  Teacher: 'GetTeacherClass',
  Student: 'GetStudentClass',
  Parent: 'GetStudentClass', // responsável ainda não tem detalhe de turma; a policy exige Student e o redireciona pra /home
}

export default defineNuxtRouteMiddleware(async (to) => {
  if (import.meta.server) return

  const routePolicy = routePolicies[to.path]
  const isClassDetail = to.path.startsWith('/classes/')
  const isClassroomDetail = to.path.startsWith('/classrooms/')
  const isTeacherDetail = to.path.startsWith('/teachers/')
  const isStudentDetail = to.path.startsWith('/students/')
  if (!routePolicy && !isClassDetail && !isClassroomDetail && !isTeacherDetail && !isStudentDetail) return

  const { account, fetchAccount } = useUserAccount()

  if (!account.value) {
    try {
      await fetchAccount()
    } catch {
      return navigateTo('/')
    }
  }

  const policyName = routePolicy
    ?? (isClassroomDetail
      ? 'GetClassroom' as PolicyName
      : isTeacherDetail
        ? 'AccessTeachersPage' as PolicyName
        : isStudentDetail
          ? 'AccessStudentsPage' as PolicyName
          : classDetailPolicies[account.value!.userType])
  if (!policyName) return

  const { can } = usePolicy()
  if (!can(policyName).value) {
    return navigateTo('/home')
  }
})
