using Syki.Back.Auth.Schemes;

namespace Syki.Back.Auth;

public class AuthBearerAttribute : AuthorizeAttribute
{
	public AuthBearerAttribute()
	{
		AuthenticationSchemes = JwtBearerScheme.Name;
	}
}
