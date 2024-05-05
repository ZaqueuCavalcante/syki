namespace Syki.Back.Extensions;

public class AuthAdmAttribute : AuthorizeAttribute
{
	public AuthAdmAttribute()
	{
		Roles = AuthorizationConfigs.Adm;
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
