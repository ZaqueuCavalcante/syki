/**
 * Permission IDs aligned with backend EstudPermissions
 */
export const Permissions = {
  // Identity
  ManageRoles: 0,
  ManageSso: 1,

  // Campi
  ManageCampi: 200,

  // Disciplines
  ManageDisciplines: 300,

  // Courses
  ManageCourses: 400,

  // Teachers
  ManageTeachers: 500,

  // Students
  ManageStudents: 600,

  // Periods
  ManagePeriods: 700,

  // CourseCurriculums
  ManageCourseCurriculums: 800,

  // CourseOfferings
  ManageCourseOfferings: 900,

  // Classes
  ManageClasses: 1000,

  // Classrooms
  ManageClassrooms: 1300,

  // Webhooks
  ManageWebhooks: 1100,

  // Notifications
  ManageNotifications: 1200,

  // Calendar
  ManageCalendar: 1400,

  // Institutions
  ManageInstitutionConfig: 1500,
} as const

export type PermissionId = typeof Permissions[keyof typeof Permissions]

/**
 * User type values aligned with backend UserType enum
 */
export const UserTypes = {
  Manager: 'Manager',
  Teacher: 'Teacher',
  Student: 'Student',
} as const

export type UserTypeValue = typeof UserTypes[keyof typeof UserTypes]

/**
 * Requires a specific permission
 */
export interface HasPermissionRequirement {
  type: 'hasPermission'
  permissionId: PermissionId
}

/**
 * Requires any of the specified permissions (OR)
 */
export interface HasAnyPermissionRequirement {
  type: 'hasAnyPermission'
  permissionIds: PermissionId[]
}

/**
 * Requires a specific user type
 */
export interface HasUserTypeRequirement {
  type: 'hasUserType'
  value: UserTypeValue
}

/**
 * Requires any of the specified user types (OR)
 */
export interface HasAnyUserTypeRequirement {
  type: 'hasAnyUserType'
  values: UserTypeValue[]
}

export type PolicyRequirement
  = | HasPermissionRequirement
    | HasAnyPermissionRequirement
    | HasUserTypeRequirement
    | HasAnyUserTypeRequirement

/**
 * Definition of a policy
 */
export interface PolicyDefinition {
  description: string
  requirements: PolicyRequirement[]
}

/**
 * All available policy names
 */
export type PolicyName
  // Home
  = | 'AccessHomePage'
    | 'GetHomeStats'
  // Campi
    | 'AccessCampiPage'
    | 'CreateCampus'
    | 'UpdateCampus'
  // Disciplines
    | 'AccessDisciplinesPage'
    | 'CreateDiscipline'
    | 'UpdateDiscipline'
  // Courses
    | 'AccessCoursesPage'
    | 'CreateCourse'
    | 'UpdateCourse'
  // CourseCurriculums
    | 'AccessCourseCurriculumsPage'
    | 'CreateCourseCurriculum'
  // Periods
    | 'AccessPeriodsPage'
    | 'CreateAcademicPeriod'
  // Enrollments
    | 'AccessEnrollmentsPage'
    | 'CreateEnrollmentPeriod'
  // CourseOfferings
    | 'AccessCourseOfferingsPage'
    | 'CreateCourseOffering'
  // Teachers
    | 'AccessTeachersPage'
    | 'CreateTeacher'
  // Students
    | 'AccessStudentsPage'
    | 'CreateStudent'
  // Classes
    | 'AccessClassesPage'
    | 'CreateClass'
    | 'ReleaseClassForEnrollment'
    | 'StartClass'
    | 'GetTeacherClass'
    | 'GetStudentClass'
  // Classrooms
    | 'AccessClassroomsPage'
    | 'GetClassroom'
    | 'CreateClassroom'
    | 'UpdateClassroom'
  // Security
    | 'AccessSecurityPage'
    | 'AccessRolesPage'
    | 'CreateRole'
    | 'UpdateRole'
    | 'AccessSsoPage'
    | 'CreateSsoConfiguration'
    | 'UpdateSsoConfiguration'
  // Integrations
    | 'AccessIntegrationsPage'
    | 'GetWebhookSubscription'
    | 'GetWebhookSubscriptions'
    | 'CreateWebhookSubscription'
    | 'UpdateWebhookSubscription'
  // Notifications
    | 'AccessNotificationsPage'
    | 'GetInstitutionNotifications'
  // Agenda
    | 'AccessAgendaPage'
  // TeacherClasses
    | 'GetTeacherCurrentClasses'
  // Calendar
    | 'AccessCalendarPage'
    | 'GetInstitutionCalendar'
    | 'CreateCalendarDay'
    | 'UpdateCalendarDay'
  // Institutions
    | 'AccessConfigsPage'
    | 'GetInstitutionConfig'
    | 'SetupInstitutionConfig'
