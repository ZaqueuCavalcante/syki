namespace Syki.Back.Auth;

/// <summary>
/// Política de acesso à feature de "Logar sem cadastro".
/// </summary>
public class SkipUserRegisterAuthRequirement : IAuthorizationRequirement { }

public class SkipUserRegisterAuthReqHandler(FeaturesSettings settings) : AuthorizationHandler<SkipUserRegisterAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SkipUserRegisterAuthRequirement requirement)
    {
        if (settings.SkipUserRegister) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
