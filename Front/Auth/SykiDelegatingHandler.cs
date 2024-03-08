using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Syki.Front.Auth;

public class SykiDelegatingHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var token = await localStorage.GetItemAsync("AccessToken");

        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await base.SendAsync(request, cancellationToken);

        // if (response.StatusCode == HttpStatusCode.Unauthorized)
        // {
        //     await authService.Logout();
        //     navigationManager.NavigateTo("/login", forceLoad: true);
        // }

        return response;
    }
}
