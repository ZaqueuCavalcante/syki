namespace Syki.Back.Auth;

public class SkipUserRegisterAuthReq : IAuthorizationRequirement { }

public class SkipUserRegisterAuthReqHandler(FeaturesSettings settings) : AuthorizationHandler<SkipUserRegisterAuthReq>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SkipUserRegisterAuthReq requirement)
    {
        if (settings.SkipUserRegister) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
