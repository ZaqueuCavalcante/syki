using System.Reflection;
using System.Collections.Concurrent;

namespace Syki.Back.Emails;

public class EmailsService : IEmailsService
{
    private readonly HttpClient _client;
    private readonly EmailSettings _settings;
    private static readonly ConcurrentDictionary<string, string> Templates = new();

    public EmailsService(EmailSettings settings, IHttpClientFactory httpClientFactory)
    {
        _settings = settings;
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri(_settings.ApiUrl);
        _client.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
    }

    public async Task SendResetPasswordEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/reset-password?token={token}";

        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki - Redefinição de senha",
            content: LoadTemplate("ResetPassword.html", link)
        );

        await _client.PostAsJsonAsync("", body);
    }

    public async Task SendFirstAccessMagicLinkEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/magic-link?token={token}";

        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki - Acesse sua conta",
            content: LoadTemplate("FirstAccessMagicLink.html", link)
        );

        await _client.PostAsJsonAsync("", body);
    }

    private static string LoadTemplate(string name, string link)
    {
        var raw = Templates.GetOrAdd(name, static n =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(n));

            using var stream = assembly.GetManifestResourceStream(resourcePath)!;
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        });

        return raw.Replace("{{link}}", link);
    }
}
