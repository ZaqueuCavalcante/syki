using Syki.Back.Domain.Identity;
using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Roles;

public static class SykiDefaultRoles
{
	public static SykiRole Director => new()
    {
		OwnerId = null,
		Name = "Diretor",
		NormalizedName = "DIRETOR",
		Description = "Gerencia campi, usuários, perfis, períodos, cursos, professores, alunos, disciplinas, ofertas e grades.",
		BaseType = UserType.Manager,
		Permissions = [
			SykiPermissions.ManageCampi.Id,
			SykiPermissions.ManageUsers.Id,
			SykiPermissions.ManageRoles.Id,
            SykiPermissions.ManagePeriods.Id,
			SykiPermissions.ManageCourses.Id,
			SykiPermissions.ManageTeachers.Id,
            SykiPermissions.ManageStudents.Id,
			SykiPermissions.ManageDisciplines.Id,
            SykiPermissions.ManageCourseOfferings.Id,
            SykiPermissions.ManageCourseCurriculums.Id,
		],
	};

	public static SykiRole Teacher => new()
    {
		OwnerId = null,
		Name = "Professor",
		NormalizedName = "PROFESSOR",
		Description = "Ministra aulas e avalia os alunos.",
		BaseType = UserType.Teacher,
		Permissions = [],
	};

	public static SykiRole Student => new()
    {
		OwnerId = null,
		Name = "Aluno",
		NormalizedName = "ALUNO",
		Description = "Participa das aulas e é avaliado.",
		BaseType = UserType.Student,
		Permissions = [],
	};
}
