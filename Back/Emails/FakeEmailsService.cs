namespace Syki.Back.Emails;

public class FakeEmailsService : IEmailsService
{
    private readonly EmailSettings _settings;
    public List<string> FirstAccessMagicLinkEmails = [];

    public FakeEmailsService(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendFirstAccessMagicLinkEmail(string to, string token)
    {
        await Task.Yield();
        var link = $"{_settings.FrontUrl}/magic-link?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        FirstAccessMagicLinkEmails.Add($"[{to} -> {link}]");
    }
}
