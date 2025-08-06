using System.Net;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Syki.Front.Auth;

public class SykiDelegatingHandler(ILocalStorageService storage, SykiAuthStateProvider auth, NavigationManager nav) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode.IsIn(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden))
        {
            await storage.RemoveItemAsync("User");
            auth.MarkUserAsLoggedOut();

            if (!nav.Uri.Equals("/login"))
                nav.NavigateTo("/login", forceLoad: true);
        }

        response.Headers.TryGetValues("X-CrossLogin", out var crossLogin);
        await storage.SetItemAsync("CrossLogin", crossLogin?.FirstOrDefault() ?? "False");

        response.Headers.TryGetValues("X-DeployHash", out var deployHashHeader);
        var deployHash = deployHashHeader?.FirstOrDefault() ?? "0";
        var storedDeployHash = await storage.GetItemAsync<string>("DeployHash");

        if (deployHash != storedDeployHash)
        {
            await storage.SetItemAsync("DeployHash", deployHash);
            nav.Refresh(true);
        }

        return response;
    }

    public SykiDelegatingHandler WithInnerHandler(HttpMessageHandler innerHandler)
    {
        InnerHandler = innerHandler;
        return this;
    }
}
