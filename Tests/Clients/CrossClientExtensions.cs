using Syki.Tests.Mock;
using Syki.Front.Auth;
using Syki.Front.Features.Cross.Login;
using Syki.Front.Features.Cross.Logout;
using Syki.Front.Features.Cross.SetupMfa;
using Syki.Front.Features.Cross.LoginMfa;
using Syki.Front.Features.Cross.GetMfaKey;
using Syki.Front.Features.Cross.ResetPassword;
using Syki.Front.Features.Academic.CrossLogin;
using Syki.Front.Features.Cross.GetUserAccount;
using Syki.Front.Features.Cross.ViewNotifications;
using Syki.Front.Features.Cross.FinishUserRegister;
using Syki.Front.Features.Cross.GetUserNotifications;
using Syki.Front.Features.Cross.SendResetPasswordToken;
using Syki.Front.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.Clients;

public static class CrossClientExtensions
{
    public static async Task<OneOf<SuccessOut, ErrorOut>> CreatePendingUserRegister(this HttpClient http, string email)
    {
        var client = new CreatePendingUserRegisterClient(http);
        return await client.Create(email);
    }

    public static async Task<OneOf<UserOut, ErrorOut>> FinishUserRegister(this HttpClient http, string token, string password)
    {
        var client = new FinishUserRegisterClient(http);
        return await client.Finish(token, password);
    }

    public static async Task<UserOut> RegisterAcademicUser(this HttpClient client, BackFactory factory)
    {
        var email = TestData.Email;
        var password = "Test@123";

        await client.CreatePendingUserRegister(email);
        var token = await factory.GetRegisterSetupToken(email);

        var response = await client.FinishUserRegister(token!, password);

        return new UserOut { Id = response.Success.Id, Email = email, Password = password };
    }

    public static async Task<OneOf<LoginOut, ErrorOut>> Login(this HttpClient http, string email, string password)
    {
        var storage = new LocalStorageServiceMock();
        var client = new LoginClient(http, storage, new SykiAuthStateProvider(storage));
        var response = await client.Login(email, password);

        return response;
    }

    public static async Task<GetMfaKeyOut> GetMfaKey(this HttpClient http)
    {
        var client = new GetMfaKeyClient(http);
        return await client.Get();
    }

    public static async Task<bool> SetupMfa(this HttpClient http, string token)
    {
        var client = new SetupMfaClient(http);
        return await client.Setup(token);
    }

    public static async Task<OneOf<LoginMfaOut, ErrorOut>> LoginMfa(this HttpClient http, string token)
    {
        var storage = new LocalStorageServiceMock();
        var client = new LoginMfaClient(http, storage, new SykiAuthStateProvider(storage));
        var response = await client.LoginMfa(token);

        return response;
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

    public static async Task<string> ResetPassword(this HttpClient client, string token)
    {
        var password = "My@newP4ssword";
        await client.ResetPassword(token, password);
        return password;
    }

    public static async Task<List<UserNotificationOut>> GetUserNotifications(this HttpClient http)
    {
        var client = new GetUserNotificationsClient(http);
        return await client.Get();
    }

    public static async Task ViewNotifications(this HttpClient http)
    {
        var client = new ViewNotificationsClient(http);
        await client.View();
    }

    public static async Task<HttpResponseMessage> Logout(this HttpClient http)
    {
        var client = new LogoutClient(http);
        return await client.Logout();
    }

    public static async Task<OneOf<CrossLoginOut, ErrorOut>> CrossLogin(this HttpClient http, Guid targetUserId)
    {
        var storage = new LocalStorageServiceMock();
        var client = new CrossLoginClient(http, storage, new SykiAuthStateProvider(storage));
        return await client.Login(targetUserId);
    }

    public static async Task<GetUserAccountOut> GetUserAccount(this HttpClient http)
    {
        var client = new GetUserAccountClient(http);
        return await client.Get();
    }
}
