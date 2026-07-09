using System.Net.Http.Json;

namespace Estud.Tests.Clients;

public static class CrossClientExtensions
{
    public static async Task<OneOf<SuccessOut, ErrorOut>> CreatePendingUserRegister(this HttpClient http, string email)
    {
        var data = new CreatePendingUserRegisterIn(email);
        var response = await http.PostAsJsonAsync("/users", data);
        return await response.Resolve<SuccessOut>();
    }

    public static async Task<OneOf<UserOut, ErrorOut>> FinishUserRegister(this HttpClient http, string token, string password)
    {
        var data = new FinishUserRegisterIn(token, password);
        var response = await http.PutAsJsonAsync("/users", data);
        return await response.Resolve<UserOut>();
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
        var body = new LoginIn(email, password);
        var response = await http.PostAsJsonAsync("/login", body);
        return await response.Resolve<LoginOut>();
    }

    public static async Task<GetMfaKeyOut> GetMfaKey(this HttpClient http)
    {
        return await http.GetFromJsonAsync<GetMfaKeyOut>("/mfa/key", HttpConfigs.JsonOptions) ?? new();
    }

    public static async Task<bool> SetupMfa(this HttpClient http, string token)
    {
        var data = new SetupMfaIn { Token = token };
        var response = await http.PostAsJsonAsync("/mfa/setup", data);
        return response.IsSuccessStatusCode;
    }

    public static async Task<OneOf<LoginMfaOut, ErrorOut>> LoginMfa(this HttpClient http, string token)
    {
        var body = new LoginMfaIn { Token = token };
        var response = await http.PostAsJsonAsync("/login/mfa", body);
        return await response.Resolve<LoginMfaOut>();
    }

    public static async Task<HttpResponseMessage> SendResetPasswordToken(this HttpClient http, string email)
    {
        var data = new SendResetPasswordTokenIn(email);
        return await http.PostAsJsonAsync("/reset-password-token", data);
    }

    public static async Task<HttpResponseMessage> ResetPassword(this HttpClient http, string token, string password)
    {
        var data = new ResetPasswordIn { Token = token, Password = password };
        return await http.PostAsJsonAsync("/reset-password", data);
    }

    public static async Task<string> ResetPassword(this HttpClient client, string token)
    {
        var password = "My@newP4ssword";
        await client.ResetPassword(token, password);
        return password;
    }

    public static async Task<List<UserNotificationOut>> GetUserNotifications(this HttpClient http)
    {
        return await http.GetFromJsonAsync<List<UserNotificationOut>>("/notifications/user", HttpConfigs.JsonOptions) ?? [];
    }

    public static async Task ViewNotifications(this HttpClient http)
    {
        await http.PutAsJsonAsync("/notifications/user", new {});
    }

    public static async Task<HttpResponseMessage> Logout(this HttpClient http)
    {
        return await http.PostAsJsonAsync("/logout", new {});
    }

    public static async Task<OneOf<CrossLoginOut, ErrorOut>> CrossLogin(this HttpClient http, Guid targetUserId)
    {
        var body = new CrossLoginIn { TargetUserId = targetUserId };
        var response = await http.PostAsJsonAsync("/academic/cross-login", body);
        return await response.Resolve<CrossLoginOut>();
    }

    public static async Task<GetUserAccountOut> GetUserAccount(this HttpClient http)
    {
        return await http.GetFromJsonAsync<GetUserAccountOut>("/user/account", HttpConfigs.JsonOptions) ?? new();
    }
}
