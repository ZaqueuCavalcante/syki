using System.Text;
using Hangfire.Dashboard;
using Hangfire.Annotations;
using System.Net.Http.Headers;

namespace Syki.Daemon;

public class HangfireAuthFilter(string user, string password) : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
	{
		var authHeader = context.GetHttpContext().Request.Headers.Authorization;
		return CheckAuth(authHeader) || Challenge(context);
	}

	private static bool Challenge([NotNull] DashboardContext context)
	{
		context.GetHttpContext().Response.StatusCode = 401;
		context.GetHttpContext().Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
		return false;
	}

	private bool CheckAuth(string authHeader)
	{
		if (authHeader.IsEmpty()) return false;

		var authValues = AuthenticationHeaderValue.Parse(authHeader);
		if ("Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
		{
			var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
			var parts = parameter.Split(':');
			if (parts.Length > 1)
			{
				return parts[0] == user && parts[1] == password;
			}
		}
		return false;
	}
}
