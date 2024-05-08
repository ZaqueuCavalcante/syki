namespace Syki.Back.Extensions;

public class AuthAcademicAttribute : AuthorizeAttribute
{
	public AuthAcademicAttribute()
	{
		Roles = UserRole.Academic.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
