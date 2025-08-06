namespace Syki.Back.Auth;

public class AuthStudentAttribute : AuthorizeAttribute
{
	public AuthStudentAttribute()
	{
		Roles = UserRole.Student.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
