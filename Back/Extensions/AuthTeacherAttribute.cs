namespace Syki.Back.Extensions;

public class AuthTeacherAttribute : AuthorizeAttribute
{
	public AuthTeacherAttribute()
	{
		Roles = UserRole.Teacher.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
