using Syki.Back.Auth.Schemes;

namespace Syki.Back.Auth;

public class AuthAcademicAttribute : AuthorizeAttribute
{
	public AuthAcademicAttribute()
	{
		Roles = UserRole.Academic.ToString();
		AuthenticationSchemes = JwtBearerScheme.Name;
	}
}
