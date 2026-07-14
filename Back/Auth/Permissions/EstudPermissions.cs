using System.Reflection;

namespace Estud.Back.Auth.Permissions;

public static class EstudPermissions
{
    // Identity
    public static readonly EstudPermission ManageRoles = new(
        PermissionGroup.Identity,
        000,
        "Gerenciar perfis de acesso.",
        "Criar, editar e deletar perfis de acesso.",
        [UserType.Manager]
    );
    public static readonly EstudPermission ManageSso = new(
        PermissionGroup.Identity,
        001,
        "Gerenciar SSO.",
        "Configurar Single Sign-On (SSO) para a instituição.",
        [UserType.Manager]
    );

    // Users
    public static readonly EstudPermission ManageUsers = new(
        PermissionGroup.Users,
        100,
        "Gerenciar usuários.",
        "Criar, editar e deletar usuários.",
        [UserType.Manager]
    );

    // Campi
    public static readonly EstudPermission ManageCampi = new(
        PermissionGroup.Campi,
        200,
        "Gerenciar campus.",
        "Criar e editar campus.",
        [UserType.Manager]
    );

    // Disciplines
    public static readonly EstudPermission ManageDisciplines = new(
        PermissionGroup.Disciplines,
        300,
        "Gerenciar disciplinas.",
        "Criar e editar disciplinas.",
        [UserType.Manager]
    );

    // Courses
    public static readonly EstudPermission ManageCourses = new(
        PermissionGroup.Courses,
        400,
        "Gerenciar cursos.",
        "Criar e editar cursos.",
        [UserType.Manager]
    );

    // Teachers
    public static readonly EstudPermission ManageTeachers = new(
        PermissionGroup.Teachers,
        500,
        "Gerenciar professores.",
        "Criar e editar professores.",
        [UserType.Manager]
    );

    // Students
    public static readonly EstudPermission ManageStudents = new(
        PermissionGroup.Students,
        600,
        "Gerenciar alunos.",
        "Criar e editar alunos.",
        [UserType.Manager]
    );

    // Periods
    public static readonly EstudPermission ManagePeriods = new(
        PermissionGroup.Periods,
        700,
        "Gerenciar períodos acadêmicos.",
        "Criar e editar períodos acadêmicos.",
        [UserType.Manager]
    );

    // CourseCurriculums
    public static readonly EstudPermission ManageCourseCurriculums = new(
        PermissionGroup.CourseCurriculums,
        800,
        "Gerenciar grades curriculares.",
        "Criar e editar grades curriculares.",
        [UserType.Manager]
    );

    // CourseOfferings
    public static readonly EstudPermission ManageCourseOfferings = new(
        PermissionGroup.CourseOfferings,
        900,
        "Gerenciar ofertas de curso.",
        "Criar e editar ofertas de curso.",
        [UserType.Manager]
    );

    // Classes
    public static readonly EstudPermission ManageClasses = new(
        PermissionGroup.Classes,
        1000,
        "Gerenciar turmas.",
        "Criar e editar turmas.",
        [UserType.Manager]
    );

    // Webhooks
    public static readonly EstudPermission ManageWebhooks = new(
        PermissionGroup.Webhooks,
        1100,
        "Gerenciar webhooks.",
        "Criar e visualizar inscrições de webhook.",
        [UserType.Manager]
    );

    // Notifications
    public static readonly EstudPermission ManageNotifications = new(
        PermissionGroup.Notifications,
        1200,
        "Gerenciar notificações.",
        "Criar e visualizar notificações.",
        [UserType.Manager]
    );

    // Classrooms
    public static readonly EstudPermission ManageClassrooms = new(
        PermissionGroup.Classrooms,
        1300,
        "Gerenciar salas de aula.",
        "Criar e editar salas de aula.",
        [UserType.Manager]
    );

    // Calendar
    public static readonly EstudPermission ManageCalendar = new(
        PermissionGroup.Calendar,
        1400,
        "Gerenciar calendário acadêmico.",
        "Criar e editar calendário acadêmico.",
        [UserType.Manager]
    );

    // Institutions
    public static readonly EstudPermission ManageInstitutionConfig = new(
        PermissionGroup.Institutions,
        1500,
        "Gerenciar configurações da instituição.",
        "Configurar nota e frequência mínimas para aprovação.",
        [UserType.Manager]
    );

    public static readonly List<PermissionGroup> Groups = [];
    public static readonly List<EstudPermission> Permissions = [];
    private static readonly Dictionary<int, EstudPermission> ById = [];
    static EstudPermissions()
    {
        Groups = Enum.GetValues<PermissionGroup>().ToList();

        Permissions = typeof(EstudPermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(EstudPermission))
            .Select(f => (EstudPermission)f.GetValue(null)!)
            .OrderBy(f => f.Group)
                .ThenBy(f => f.Id)
            .ToList();

        if (!Permissions.Select(x => x.Id).IsAllDistinct()) throw new Exception("Duplicated permission ids!");

        if (!Permissions.Select(x => x.Name).IsAllDistinct()) throw new Exception("Duplicated permission names!");

        if (Permissions.Any(x => x.AllowedTypes == null || x.AllowedTypes.Count == 0)) throw new Exception("All permissions must declare AllowedTypes!");

        ById = Permissions.ToDictionary(x => x.Id);
    }

    public static bool IsAllowedFor(int permissionId, UserType userType)
    {
        return ById.TryGetValue(permissionId, out var p) && p.AllowedTypes.Contains(userType);
    }
}
