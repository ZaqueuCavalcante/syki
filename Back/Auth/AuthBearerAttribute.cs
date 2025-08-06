namespace Syki.Back.Auth;

public class AuthBearerAttribute : AuthorizeAttribute
{
	public AuthBearerAttribute()
	{
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
