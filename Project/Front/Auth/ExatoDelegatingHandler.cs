using System.Net;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Exato.Front.Auth;

public class ExatoDelegatingHandler(ILocalStorageService storage, ExatoAuthStateProvider auth, NavigationManager nav) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
            await storage.RemoveItemAsync("User");
            auth.MarkUserAsLoggedOut();

            if (!nav.Uri.Equals("") && !nav.Uri.Equals("/") && !nav.Uri.Equals("/login"))
                nav.NavigateTo("/login", forceLoad: true);
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine($"403 ON -> {nav.Uri}");
        }

        return response;
    }
}
