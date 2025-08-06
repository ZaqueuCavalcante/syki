using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Syki.Front.Auth;

public class SignalRConnectionManager(
    IConfiguration configuration,
    ILocalStorageService storage,
    SykiAuthStateProvider auth,
    NavigationManager nav)
{
    private HubConnection? _hubConnection;

    public async Task StartAsync()
    {
        if (_hubConnection is not null && _hubConnection.State != HubConnectionState.Disconnected)
            return;

        var apiUrl = configuration.GetSection("ApiUrl").Value!;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiUrl}/syki-hub", options =>
            {
                options.HttpMessageHandlerFactory = innerHandler =>
                new SykiDelegatingHandler(storage, auth, nav).WithInnerHandler(innerHandler);
            })
            .WithAutomaticReconnect()
            .Build();

        await _hubConnection.StartAsync();
    }

    public async Task StopAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}
