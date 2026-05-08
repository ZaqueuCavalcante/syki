using Syki.Back.Auth.Schemes;

namespace Syki.Back.Auth;

public class AuthAdmAttribute : AuthorizeAttribute
{
	public AuthAdmAttribute()
	{
		Roles = UserRole.Adm.ToString();
		AuthenticationSchemes = JwtBearerScheme.Name;
	}
}
