namespace Syki.Back.Auth;

public class CrossLoginAuthReq : IAuthorizationRequirement { }

public class CrossLoginAuthReqHandler(FeaturesSettings settings) : AuthorizationHandler<CrossLoginAuthReq>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CrossLoginAuthReq requirement)
    {
        if (settings.CrossLogin) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
