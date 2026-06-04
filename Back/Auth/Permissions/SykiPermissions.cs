using System.Reflection;

namespace Syki.Back.Auth.Permissions;

public static class SykiPermissions
{
    // Identity
    public static readonly SykiPermission ManageRoles = new(
        PermissionGroup.Identity,
        000,
        "Gerenciar perfis de acesso.",
        "Criar, editar e deletar perfis de acesso.",
        [UserType.Manager]
    );
    public static readonly SykiPermission ManageSso = new(
        PermissionGroup.Identity,
        001,
        "Gerenciar SSO.",
        "Configurar Single Sign-On (SSO) para a instituição.",
        [UserType.Manager]
    );

    // Users
    public static readonly SykiPermission ManageUsers = new(
        PermissionGroup.Users,
        100,
        "Gerenciar usuários.",
        "Criar, editar e deletar usuários.",
        [UserType.Manager]
    );

    // Campi
    public static readonly SykiPermission ManageCampi = new(
        PermissionGroup.Campi,
        200,
        "Gerenciar campus.",
        "Criar e editar campus.",
        [UserType.Manager]
    );

    // Disciplines
    public static readonly SykiPermission ManageDisciplines = new(
        PermissionGroup.Disciplines,
        300,
        "Gerenciar disciplinas.",
        "Criar e editar disciplinas.",
        [UserType.Manager]
    );

    // Courses
    public static readonly SykiPermission ManageCourses = new(
        PermissionGroup.Courses,
        400,
        "Gerenciar cursos.",
        "Criar e editar cursos.",
        [UserType.Manager]
    );

    // Teachers
    public static readonly SykiPermission ManageTeachers = new(
        PermissionGroup.Teachers,
        500,
        "Gerenciar professores.",
        "Criar e editar professores.",
        [UserType.Manager]
    );

    // Students
    public static readonly SykiPermission ManageStudents = new(
        PermissionGroup.Students,
        600,
        "Gerenciar alunos.",
        "Criar e editar alunos.",
        [UserType.Manager]
    );

    // Periods
    public static readonly SykiPermission ManagePeriods = new(
        PermissionGroup.Periods,
        700,
        "Gerenciar períodos acadêmicos.",
        "Criar e editar períodos acadêmicos.",
        [UserType.Manager]
    );

    // CourseCurriculums
    public static readonly SykiPermission ManageCourseCurriculums = new(
        PermissionGroup.CourseCurriculums,
        800,
        "Gerenciar grades curriculares.",
        "Criar e editar grades curriculares.",
        [UserType.Manager]
    );

    // CourseOfferings
    public static readonly SykiPermission ManageCourseOfferings = new(
        PermissionGroup.CourseOfferings,
        900,
        "Gerenciar ofertas de curso.",
        "Criar e editar ofertas de curso.",
        [UserType.Manager]
    );

    // Classes
    public static readonly SykiPermission ManageClasses = new(
        PermissionGroup.Classes,
        1000,
        "Gerenciar turmas.",
        "Criar e editar turmas.",
        [UserType.Manager]
    );

    public static readonly List<PermissionGroup> Groups = [];
    public static readonly List<SykiPermission> Permissions = [];
    private static readonly Dictionary<int, SykiPermission> ById = [];
    static SykiPermissions()
    {
        Groups = Enum.GetValues<PermissionGroup>().ToList();

        Permissions = typeof(SykiPermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(SykiPermission))
            .Select(f => (SykiPermission)f.GetValue(null)!)
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
