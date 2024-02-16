using Syki.Back.Settings;

namespace Syki.Back.Services;

public class FakeEmailsService : IEmailsService
{
    private readonly EmailSettings _settings;
    public FakeEmailsService(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendResetPasswordEmail(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{_settings.FrontUrl}/reset-password?token={token}";
        Console.WriteLine($"[LINK -> {link}]");
    }

    public async Task SendDemoEmailConfirmation(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{_settings.FrontUrl}/demo-setup?token={token}";
        Console.WriteLine($"[LINK -> {link}]");
    }
}