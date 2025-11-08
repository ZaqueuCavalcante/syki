using SendGrid;
using SendGrid.Helpers.Mail;

namespace Exato.Back.Emails;

public class SendGridEmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendResetPasswordEmail(string to, string token)
    {
		var settings = configuration.Email;

        var client = new SendGridClient(settings.ApiKey);

        var from = new EmailAddress("suporte@exato.digital", "Admin");

        var link = $"{settings.FrontUrl}/reset-password?token={token}";

        var msg = new SendGridMessage
        {
            From = from,
            Subject = "Exato Admin - Redefinição de senha",
            HtmlContent =
			$"""
				<p>Para redefinir sua senha, clique no link abaixo e siga as instruções.</p>
				<p>{link}</p>
			"""
        };

        msg.AddTo(to);

        var response = await client.SendEmailAsync(msg);

        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine(await response.Body.ReadAsStringAsync());
    }
}
