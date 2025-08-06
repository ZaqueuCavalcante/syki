using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;

namespace Syki.Front.Auth;

public class CrossLoginAuthReq : IAuthorizationRequirement { }

public class CrossLoginAuthReqHandler(ILocalStorageService localStorage) : AuthorizationHandler<CrossLoginAuthReq>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CrossLoginAuthReq requirement)
    {
        var canCrossLogin = await localStorage.GetItemAsync<string>("CrossLogin");

        if (canCrossLogin == "True") context.Succeed(requirement);
    }
}
