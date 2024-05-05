namespace Syki.Back.Extensions;

public class AuthBearerAttribute : AuthorizeAttribute
{
	public AuthBearerAttribute()
	{
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
