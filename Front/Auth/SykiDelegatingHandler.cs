using System.Net;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Syki.Front.Auth;

public class SykiDelegatingHandler(ILocalStorageService storage, NavigationManager nav, SykiAuthStateProvider auth) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var token = await storage.GetItemAsync("AccessToken");

        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await storage.RemoveItemAsync("AccessToken");
            auth.MarkUserAsLoggedOut();
            if (!nav.Uri.Equals("/"))
                nav.NavigateTo("/", forceLoad: true);
        }

        response.Headers.TryGetValues("X-CrossLogin", out var crossLogin);
        await storage.SetItemAsync("CrossLogin", crossLogin?.FirstOrDefault() ?? "False");

        return response;
    }
}
