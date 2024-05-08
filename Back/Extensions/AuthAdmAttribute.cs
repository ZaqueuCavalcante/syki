namespace Syki.Back.Extensions;

public class AuthAdmAttribute : AuthorizeAttribute
{
	public AuthAdmAttribute()
	{
		Roles = UserRole.Adm.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
