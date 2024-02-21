using Microsoft.AspNetCore.Authorization;

namespace Track.Frontend.Client.Shared;

public class AuthAttribute : AuthorizeAttribute
{
	public AuthAttribute(params string[] roles)
	{
		Roles = string.Join(",", roles);
	}
}
