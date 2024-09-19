using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;

namespace Syki.Front.Auth;

public class SkipUserRegisterAuthReq : IAuthorizationRequirement { }

public class SkipUserRegisterAuthReqHandler(ILocalStorageService localStorage) : AuthorizationHandler<SkipUserRegisterAuthReq>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SkipUserRegisterAuthReq requirement)
    {
        var canSkipUserRegister = await localStorage.GetItemAsync("SkipUserRegister");

        if (canSkipUserRegister == "True") context.Succeed(requirement);
    }
}
