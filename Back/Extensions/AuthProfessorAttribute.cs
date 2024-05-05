namespace Syki.Back.Extensions;

public class AuthProfessorAttribute : AuthorizeAttribute
{
	public AuthProfessorAttribute()
	{
		Roles = AuthorizationConfigs.Professor;
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
