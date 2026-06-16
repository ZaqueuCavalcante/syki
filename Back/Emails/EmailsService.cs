using System.Text.Json;
using System.Reflection;
using System.Collections.Concurrent;

namespace Syki.Back.Emails;

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

        var keyPreview = _settings.ApiKey.Length > 8
            ? _settings.ApiKey[..4] + "..." + _settings.ApiKey[^4..]
            : "***";
        _logger.LogInformation("[EmailsService] initialized - ApiUrl: {ApiUrl} | FrontUrl: {FrontUrl} | ApiKey: {KeyPreview}",
            _settings.ApiUrl, _settings.FrontUrl, keyPreview);
    }

    public async Task SendResetPasswordEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/reset-password?token={token}";
        await SendEmail("SendResetPasswordEmail", to, "Syki - Redefinição de senha", "ResetPassword.html", link);
    }

    public async Task SendFirstAccessMagicLinkEmail(string to, string token)
    {
        var link = $"{_settings.FrontUrl}/magic-link?token={token}";
        await SendEmail("SendFirstAccessMagicLinkEmail", to, "Syki - Acesse sua conta", "FirstAccessMagicLink.html", link);
    }

    private async Task SendEmail(string method, string to, string subject, string templateName, string link)
    {
        _logger.LogInformation("[EmailsService] {Method} - to: {To} | subject: {Subject} | link: {Link}",
            method, to, subject, link);
        try
        {
            var content = LoadTemplate(templateName, link);
            var body = new BrevoEmailMessage(sender: "suporte@estud.com.br", to: to, subject: subject, content: content);

            var bodyJson = JsonSerializer.Serialize(new { body.Sender, body.To, body.Subject }, _logOptions);
            _logger.LogInformation("[EmailsService] {Method} - request body (sem htmlContent): {Body}", method, bodyJson);

            var response = await _client.PostAsJsonAsync("", body);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("[EmailsService] {Method} - success ({StatusCode}): {Response}",
                    method, (int)response.StatusCode, responseBody);
            else
                _logger.LogError("[EmailsService] {Method} - failed ({StatusCode}): {Response}",
                    method, (int)response.StatusCode, responseBody);
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
            var resourcePath = resourceNames.SingleOrDefault(str => str.EndsWith(n))
                ?? throw new InvalidOperationException($"Email template '{n}' not found. Available: {string.Join(", ", resourceNames)}");

            using var stream = assembly.GetManifestResourceStream(resourcePath)!;
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        });

        return raw.Replace("{{link}}", link);
    }
}
