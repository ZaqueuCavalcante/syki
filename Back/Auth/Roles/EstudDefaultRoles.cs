using Estud.Back.Domain.Identity;
using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Roles;

public static class EstudDefaultRoles
{
	public static EstudRole Director => new()
    {
		OwnerId = null,
		Name = "Diretor",
		NormalizedName = "DIRETOR",
		Description = "Gerencia campi, usuários, perfis, SSO, períodos, turmas, cursos, professores, webhooks, alunos, salas, disciplinas, notificações, ofertas e grades.",
		BaseType = UserType.Manager,
		Permissions = [
			EstudPermissions.ManageSso.Id,
			EstudPermissions.ManageCampi.Id,
			EstudPermissions.ManageUsers.Id,
			EstudPermissions.ManageRoles.Id,
			EstudPermissions.ManageClasses.Id,
            EstudPermissions.ManagePeriods.Id,
			EstudPermissions.ManageCourses.Id,
			EstudPermissions.ManageTeachers.Id,
            EstudPermissions.ManageWebhooks.Id,
            EstudPermissions.ManageStudents.Id,
            EstudPermissions.ManageClassrooms.Id,
			EstudPermissions.ManageDisciplines.Id,
            EstudPermissions.ManageNotifications.Id,
            EstudPermissions.ManageCourseOfferings.Id,
            EstudPermissions.ManageCourseCurriculums.Id,
		],
	};

	public static EstudRole Teacher => new()
    {
		OwnerId = null,
		Name = "Professor",
		NormalizedName = "PROFESSOR",
		Description = "Ministra aulas e avalia os alunos.",
		BaseType = UserType.Teacher,
		Permissions = [],
	};

	public static EstudRole Student => new()
    {
		OwnerId = null,
		Name = "Aluno",
		NormalizedName = "ALUNO",
		Description = "Participa das aulas e é avaliado.",
		BaseType = UserType.Student,
		Permissions = [],
	};
}
