using Syki.Back.Auth.Schemes;

namespace Syki.Back.Auth;

public class AuthTeacherAttribute : AuthorizeAttribute
{
	public AuthTeacherAttribute()
	{
		Roles = UserRole.Teacher.ToString();
		AuthenticationSchemes = JwtBearerScheme.Name;
	}
}
