using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Extensions;

public class AuthAcademicoAttribute : AuthorizeAttribute
{
	public AuthAcademicoAttribute()
	{
		Roles = Academico;
	}
}
