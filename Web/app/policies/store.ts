import type {
  PolicyName,
  PermissionId,
  UserTypeValue,
  PolicyRequirement,
  PolicyDefinition,
} from "./types";
import { Permissions, UserTypes } from "./types";

function hasPermission(permissionId: PermissionId): PolicyRequirement {
  return { type: "hasPermission", permissionId };
}

function hasAnyPermission(...permissionIds: PermissionId[]): PolicyRequirement {
  return { type: "hasAnyPermission", permissionIds };
}

function hasUserType(value: UserTypeValue): PolicyRequirement {
  return { type: "hasUserType", value };
}

function hasAnyUserType(...values: UserTypeValue[]): PolicyRequirement {
  return { type: "hasAnyUserType", values };
}

/**
 * Central policy store - mirrors backend policies with UI-specific additions
 */
export const Policies: Record<PolicyName, PolicyDefinition> = {
  // Home
  AccessHomePage: {
    description: "Acessar a página inicial",
    requirements: [],
  },
  GetHomeStats: {
    description: "Ver estatísticas da home",
    requirements: [],
  },

  // Campi
  AccessCampiPage: {
    description: "Acessar a página de campi",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCampi),
    ],
  },
  CreateCampus: {
    description: "Criar novos campi",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCampi),
    ],
  },
  UpdateCampus: {
    description: "Editar campi",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCampi),
    ],
  },

  // Disciplines
  AccessDisciplinesPage: {
    description: "Acessar a página de disciplinas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageDisciplines),
    ],
  },
  CreateDiscipline: {
    description: "Criar novas disciplinas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageDisciplines),
    ],
  },
  UpdateDiscipline: {
    description: "Editar disciplinas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageDisciplines),
    ],
  },

  // Courses
  AccessCoursesPage: {
    description: "Acessar a página de cursos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourses),
    ],
  },
  CreateCourse: {
    description: "Criar novos cursos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourses),
    ],
  },
  UpdateCourse: {
    description: "Editar cursos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourses),
    ],
  },

  // CourseCurriculums
  AccessCourseCurriculumsPage: {
    description: "Acessar a página de grades curriculares",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourseCurriculums),
    ],
  },
  CreateCourseCurriculum: {
    description: "Criar novas grades curriculares",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourseCurriculums),
    ],
  },

  // Periods
  AccessPeriodsPage: {
    description: "Acessar a página de períodos acadêmicos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManagePeriods),
    ],
  },
  CreateAcademicPeriod: {
    description: "Criar novos períodos acadêmicos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManagePeriods),
    ],
  },

  // Enrollments
  AccessEnrollmentsPage: {
    description: "Acessar a página de matrículas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManagePeriods),
    ],
  },
  CreateEnrollmentPeriod: {
    description: "Criar novos períodos de matrícula",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManagePeriods),
    ],
  },

  // CourseOfferings
  AccessCourseOfferingsPage: {
    description: "Acessar a página de ofertas de curso",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourseOfferings),
    ],
  },
  CreateCourseOffering: {
    description: "Criar novas ofertas de curso",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCourseOfferings),
    ],
  },

  // Classes
  AccessClassesPage: {
    description: "Acessar a página de turmas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageClasses),
    ],
  },
  CreateClass: {
    description: "Criar novas turmas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageClasses),
    ],
  },
  ReleaseClassForEnrollment: {
    description: "Liberar turmas para matrícula",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageClasses),
    ],
  },
  StartClass: {
    description: "Iniciar turmas",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageClasses),
    ],
  },
  GetTeacherClass: {
    description: "Ver os detalhes de uma turma que leciona",
    requirements: [
      hasUserType(UserTypes.Teacher),
    ],
  },
  GetStudentClass: {
    description: "Ver os detalhes de uma turma em que está matriculado",
    requirements: [
      hasUserType(UserTypes.Student),
    ],
  },

  // Teachers
  AccessTeachersPage: {
    description: "Acessar a página de professores",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageTeachers),
    ],
  },
  CreateTeacher: {
    description: "Cadastrar novos professores",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageTeachers),
    ],
  },

  // Students
  AccessStudentsPage: {
    description: "Acessar a página de alunos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageStudents),
    ],
  },
  CreateStudent: {
    description: "Cadastrar novos alunos",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageStudents),
    ],
  },

  // Security
  AccessSecurityPage: {
    description: "Acessar a página de segurança",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasAnyPermission(Permissions.ManageRoles, Permissions.ManageSso),
    ],
  },
  AccessRolesPage: {
    description: "Acessar a aba de perfis de acesso",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageRoles),
    ],
  },
  CreateRole: {
    description: "Criar novos perfis de acesso",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageRoles),
    ],
  },
  UpdateRole: {
    description: "Editar perfis de acesso",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageRoles),
    ],
  },
  AccessSsoPage: {
    description: "Acessar a aba de configurações SSO",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageSso),
    ],
  },
  CreateSsoConfiguration: {
    description: "Criar configurações SSO",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageSso),
    ],
  },
  UpdateSsoConfiguration: {
    description: "Editar configurações SSO",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageSso),
    ],
  },

  // Integrations
  AccessIntegrationsPage: {
    description: "Acessar a página de integrações",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageWebhooks),
    ],
  },
  GetWebhookSubscription: {
    description: "Ver inscrição de webhook",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageWebhooks),
    ],
  },
  GetWebhookSubscriptions: {
    description: "Listar inscrições de webhook",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageWebhooks),
    ],
  },
  CreateWebhookSubscription: {
    description: "Criar inscrições de webhook",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageWebhooks),
    ],
  },
  UpdateWebhookSubscription: {
    description: "Editar inscrições de webhook",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageWebhooks),
    ],
  },

  // Notifications
  AccessNotificationsPage: {
    description: "Acessar a página de notificações",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageNotifications),
    ],
  },
  GetInstitutionNotifications: {
    description: "Listar notificações da instituição",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageNotifications),
    ],
  },

  // Agenda
  AccessAgendaPage: {
    description: "Acessar a página de agenda",
    requirements: [
      hasAnyUserType(UserTypes.Teacher, UserTypes.Student),
    ],
  },

  // TeacherClasses
  GetTeacherCurrentClasses: {
    description: "Ver as turmas atuais do professor",
    requirements: [
      hasUserType(UserTypes.Teacher),
    ],
  },

  // Calendar
  AccessCalendarPage: {
    description: "Acessar a página de calendário acadêmico",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCalendar),
    ],
  },
  GetInstitutionCalendar: {
    description: "Ver o calendário acadêmico da instituição",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCalendar),
    ],
  },
  CreateCalendarDay: {
    description: "Customizar dias do calendário acadêmico",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCalendar),
    ],
  },
  UpdateCalendarDay: {
    description: "Editar dias customizados do calendário acadêmico",
    requirements: [
      hasUserType(UserTypes.Manager),
      hasPermission(Permissions.ManageCalendar),
    ],
  },
};

export function getPolicy(name: PolicyName): PolicyDefinition {
  return Policies[name];
}

export function getPolicyNames(): PolicyName[] {
  return Object.keys(Policies) as PolicyName[];
}
