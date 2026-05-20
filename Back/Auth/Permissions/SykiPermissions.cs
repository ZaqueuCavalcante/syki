using System.Reflection;

namespace Syki.Back.Auth.Permissions;

public static class SykiPermissions
{
    // Identity
    public static readonly SykiPermission ManageRoles = new(
        PermissionGroup.Identity,
        000,
        "Gerenciar perfis de acesso.",
        "Criar, editar e deletar perfis de acesso."
    );
    public static readonly SykiPermission ManageSso = new(
        PermissionGroup.Identity,
        001,
        "Gerenciar SSO.",
        "Configurar Single Sign-On (SSO) para a instituição."
    );

    // Users
    public static readonly SykiPermission ManageUsers = new(
        PermissionGroup.Users,
        100,
        "Gerenciar usuários.",
        "Criar, editar e deletar usuários."
    );

    // Campi
    public static readonly SykiPermission ManageCampi = new(
        PermissionGroup.Campi,
        200,
        "Gerenciar campus.",
        "Criar e editar campus."
    );

    // Disciplines
    public static readonly SykiPermission ManageDisciplines = new(
        PermissionGroup.Disciplines,
        300,
        "Gerenciar disciplinas.",
        "Criar e editar disciplinas."
    );

    // Courses
    public static readonly SykiPermission ManageCourses = new(
        PermissionGroup.Courses,
        400,
        "Gerenciar cursos.",
        "Criar e editar cursos."
    );

    public static readonly List<PermissionGroup> Groups = [];
    public static readonly List<SykiPermission> Permissions = [];
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
    }
}
