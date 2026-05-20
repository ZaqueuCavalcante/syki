using System.Reflection;
using System.Collections.Concurrent;

namespace Syki.Back.Emails;

public class EmailsService : IEmailsService
{
    private readonly HttpClient _client;
    private readonly EmailSettings _settings;
    private static readonly ConcurrentDictionary<string, string> Templates = new();

    public EmailsService(EmailSettings settings)
    {
        _settings = settings;
        _client = new HttpClient { BaseAddress = new Uri(_settings.ApiUrl) };
        _client.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
    }

    public async Task SendFirstAccessMagicLinkEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/magic-link?token={token}";

        var body = new BrevoEmailMessage(
            sender: "suporte@syki.com",
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
