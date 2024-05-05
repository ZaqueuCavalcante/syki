namespace Syki.Back.Extensions;

public class AuthAcademicoAttribute : AuthorizeAttribute
{
	public AuthAcademicoAttribute()
	{
		Roles = AuthorizationConfigs.Academico;
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
