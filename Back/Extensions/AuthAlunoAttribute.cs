namespace Syki.Back.Extensions;

public class AuthAlunoAttribute : AuthorizeAttribute
{
	public AuthAlunoAttribute()
	{
		Roles = AuthorizationConfigs.Aluno;
	}
}
