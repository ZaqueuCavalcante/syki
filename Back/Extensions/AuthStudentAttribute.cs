namespace Syki.Back.Extensions;

public class AuthStudentAttribute : AuthorizeAttribute
{
	public AuthStudentAttribute()
	{
		Roles = UserRole.Student.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
