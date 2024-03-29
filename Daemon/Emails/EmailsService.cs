using System.Net.Http.Json;
using Syki.Daemon.Settings;

namespace Syki.Daemon.Emails;

public class EmailsService : IEmailsService
{
    private readonly HttpClient _client;
    private readonly EmailSettings _settings;
    public EmailsService(EmailSettings settings)
    {
        _settings = settings;
        _client = new HttpClient { BaseAddress = new Uri(settings.ApiUrl) };
        _client.DefaultRequestHeaders.Add("api-key", settings.ApiKey);
    }

    public async Task SendResetPasswordEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/reset-password?token={token}";
        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki Reset Password",
            content: $"<html><head></head><body>Redefina sua senha aqui: <a href=\"{link}\"><strong>{link}</strong></a></body></html>"
        );

        await _client.PostAsJsonAsync("", body);
    }

    public async Task SendUserRegisterEmailConfirmation(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/register-setup?token={token}";
        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki Cadastro",
            content: $"<html><head></head><body>Crie sua senha aqui: <a href=\"{link}\"><strong>{link}</strong></a></body></html>"
        );

        await _client.PostAsJsonAsync("", body);
    }
}
