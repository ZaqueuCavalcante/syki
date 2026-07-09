using System.Text.Json;
using System.Reflection;
using System.Collections.Concurrent;

namespace Estud.Back.Emails;

public class EmailsService : IEmailsService
{
    private readonly HttpClient _client;
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailsService> _logger;
    private static readonly ConcurrentDictionary<string, string> Templates = new();
    private static readonly JsonSerializerOptions _logOptions = new() { WriteIndented = false };

    public EmailsService(EmailSettings settings, IHttpClientFactory httpClientFactory, ILogger<EmailsService> logger)
    {
        _logger = logger;
        _settings = settings;
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri(_settings.ApiUrl);
        _client.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
    }

    public async Task SendResetPasswordEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/reset-password?token={token}";
        await SendEmail("SendResetPasswordEmail", to, "Redefinição de senha", "ResetPassword.html", link);
    }

    public async Task SendFirstAccessMagicLinkEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/magic-link?token={token}";
        await SendEmail("SendFirstAccessMagicLinkEmail", to, "Acesse sua conta", "FirstAccessMagicLink.html", link);
    }

    private async Task SendEmail(string method, string to, string subject, string templateName, string link)
    {
        try
        {
            var content = LoadTemplate(templateName, link);
            var body = new BrevoEmailMessage(sender: "suporte@estud.com.br", to: to, subject: subject, content: content);

            var bodyJson = JsonSerializer.Serialize(new { body.Sender, body.To, body.Subject }, _logOptions);

            var response = await _client.PostAsJsonAsync("", body);
            var responseBody = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[EmailsService] {Method} - exception sending to {To}", method, to);
        }
    }

    private static string LoadTemplate(string name, string link)
    {
        var raw = Templates.GetOrAdd(name, static n =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var resourcePath = resourceNames.SingleOrDefault(str => str.EndsWith(n));

            using var stream = assembly.GetManifestResourceStream(resourcePath)!;
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        });

        return raw.Replace("{{link}}", link);
    }
}
