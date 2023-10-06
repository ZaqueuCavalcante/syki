using Microsoft.JSInterop;

namespace Syki.Front.Auth;

public class SykiDelegatingHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    public SykiDelegatingHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync("AccessToken");

        request.Headers.Add("Authorization", $"Bearer {token}");

        return await base.SendAsync(request, cancellationToken);
    }
}
