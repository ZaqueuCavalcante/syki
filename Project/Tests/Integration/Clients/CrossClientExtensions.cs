using Exato.Front.Auth;
using Exato.Tests.Integration.Mocks;
using Exato.Front.Features.Cross.Login;
using Exato.Front.Features.Cross.Logout;
using Exato.Shared.Features.Cross.Login;
using Exato.Front.Features.Cross.ResetPassword;
using Exato.Front.Features.Cross.GetUserAccount;
using Exato.Shared.Features.Cross.GetUserAccount;
using Exato.Front.Features.Cross.SendResetPasswordToken;
using Exato.Front.Features.Cross.SetupTwoFactorAuthentication;
using Exato.Front.Features.Cross.TwoFactorAuthenticationLogin;
using Exato.Front.Features.Cross.GetTwoFactorAuthenticationKey;
using Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;
using Exato.Shared.Features.Cross.GetTwoFactorAuthenticationKey;

namespace Exato.Tests.Integration.Clients;

public static class CrossClientExtensions
{
    public static async Task<OneOf<LoginOut, ErrorOut>> Login(this HttpClient http, string email, string password)
    {
        var storage = new LocalStorageServiceMock();
        var client = new LoginClient(http, storage, new ExatoAuthStateProvider(storage));
        return await client.Login(email, password);
    }

    public static async Task<HttpResponseMessage> Logout(this HttpClient http)
    {
        var client = new LogoutClient(http);
        return await client.Logout();
    }

    public static async Task<HttpResponseMessage> SendResetPasswordToken(this HttpClient http, string email)
    {
        var client = new SendResetPasswordTokenClient(http);
        return await client.Send(email);
    }

    public static async Task<HttpResponseMessage> ResetPassword(this HttpClient http, string token, string password)
    {
        var client = new ResetPasswordClient(http);
        return await client.Reset(token, password);
    }

    public static async Task<GetUserAccountOut> GetUserAccount(this HttpClient http)
    {
        var client = new GetUserAccountClient(http);
        return await client.Get();
    }

    public static async Task<GetTwoFactorAuthenticationKeyOut> GetTwoFactorAuthenticationKey(this HttpClient http)
    {
        var client = new GetTwoFactorAuthenticationKeyClient(http);
        return await client.Get();
    }

    public static async Task<bool> SetupTwoFactorAuthentication(this HttpClient http, string token)
    {
        var client = new SetupTwoFactorAuthenticationClient(http);
        return await client.Setup(token);
    }

    public static async Task<OneOf<TwoFactorAuthenticationLoginOut, ErrorOut>> TwoFactorAuthenticationLogin(this HttpClient http, string token)
    {
        var storage = new LocalStorageServiceMock();
        var client = new TwoFactorAuthenticationLoginClient(http, storage, new ExatoAuthStateProvider(storage));
        return await client.Login(token);
    }
}
