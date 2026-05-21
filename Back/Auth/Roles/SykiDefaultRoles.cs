using Syki.Back.Domain.Identity;
using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Roles;

public static class SykiDefaultRoles
{
	public static int DirectorId { get; set; }

	public static SykiRole Director => new()
    {
		OwnerId = null,
		Name = "Diretor",
		NormalizedName = "DIRETOR",
		Description = "Gerencia campi, usuários, perfis de acesso, cursos, professores e disciplinas.",
		Permissions = [
			SykiPermissions.ManageCampi.Id,
			SykiPermissions.ManageUsers.Id,
			SykiPermissions.ManageRoles.Id,
			SykiPermissions.ManageCourses.Id,
			SykiPermissions.ManageTeachers.Id,
			SykiPermissions.ManageDisciplines.Id,
		],
	};
}
