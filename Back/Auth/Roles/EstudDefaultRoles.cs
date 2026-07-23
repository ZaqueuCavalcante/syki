using Estud.Back.Domain.Identity;
using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Roles;

public static class EstudDefaultRoles
{
	public static EstudRole Director => new()
    {
		Name = "Diretor",
		NormalizedName = "DIRETOR",
		Description = "Gerencia todos os cadastros e configurações da instituição.",
		BaseType = UserType.Manager,
		Permissions = [
			EstudPermissions.ManageSso.Id,
			EstudPermissions.ManageCampi.Id,
			EstudPermissions.ManageUsers.Id,
			EstudPermissions.ManageRoles.Id,
			EstudPermissions.ManageClasses.Id,
            EstudPermissions.ManagePeriods.Id,
			EstudPermissions.ManageCourses.Id,
            EstudPermissions.ManageParents.Id,
			EstudPermissions.ManageTeachers.Id,
            EstudPermissions.ManageWebhooks.Id,
            EstudPermissions.ManageStudents.Id,
            EstudPermissions.ManageCalendar.Id,
			EstudPermissions.ManageTwoFactor.Id,
            EstudPermissions.ManageClassrooms.Id,
			EstudPermissions.ManageDisciplines.Id,
            EstudPermissions.ManageNotifications.Id,
            EstudPermissions.ManageCourseOfferings.Id,
            EstudPermissions.ManageCourseCurriculums.Id,
            EstudPermissions.ManageInstitutionConfig.Id,
		],
	};

	public static EstudRole Teacher => new()
    {
		Name = "Professor",
		NormalizedName = "PROFESSOR",
		Description = "Ministra aulas e avalia os alunos.",
		BaseType = UserType.Teacher,
		Permissions = [],
	};

	public static EstudRole Student => new()
    {
		Name = "Aluno",
		NormalizedName = "ALUNO",
		Description = "Participa das aulas e é avaliado.",
		BaseType = UserType.Student,
		Permissions = [],
	};

	public static EstudRole Parent => new()
    {
		Name = "Responsável",
		NormalizedName = "RESPONSAVEL",
		Description = "Acompanha a vida escolar dos alunos vinculados.",
		BaseType = UserType.Parent,
		Permissions = [],
	};
}
