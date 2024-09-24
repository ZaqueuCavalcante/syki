namespace Syki.Back.Auth;

/// <summary>
/// Política de acesso à feature de "Login Cruzado".
/// </summary>
public class CrossLoginAuthRequirement : IAuthorizationRequirement { }

public class CrossLoginAuthReqHandler(FeaturesSettings settings) : AuthorizationHandler<CrossLoginAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CrossLoginAuthRequirement requirement)
    {
        if (settings.CrossLogin) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
