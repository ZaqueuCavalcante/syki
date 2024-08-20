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
		if (to.EndsWith("@syki.seed.com")) return;

        var link = $"{_settings.FrontUrl}/reset-password?token={token}";
        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki - Redefinição de senha",
            content: GetContent("Redefinição de senha", "Para redefinir sua senha, clique no botão abaixo e siga as instruções.", "Redefinir", link)
        );

        await _client.PostAsJsonAsync("", body);
    }

    public async Task SendUserRegisterEmailConfirmation(string to, string token)
    {
		if (to.EndsWith("@syki.seed.com")) return;

        var link = $"{_settings.FrontUrl}/register-setup?token={token}";
        var body = new BrevoEmailMessage(
            sender: "syki@zaqbit.com",
            to: to,
            subject: "Syki - Cadastro",
            content: GetContent("Cadastro", "Para finalizar seu cadastro, clique no botão abaixo e siga as instruções.", "Finalizar", link)
        );

        await _client.PostAsJsonAsync("", body);
    }

	private string GetContent(string title, string description, string button, string link)
	{
		return """
			<!doctype html>
			<html lang="pt-BR">
			<head>
				<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
			</head>
			<body marginheight="0" topmargin="0" marginwidth="0" style="margin: 0px; background-color: #f2f3f8;" leftmargin="0">
				<table cellspacing="0" border="0" cellpadding="0" width="100%" bgcolor="#f2f3f8"
					style="@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;">
					<tr>
						<td>
							<table style="background-color: #f2f3f8; max-width:670px;  margin:0 auto;" width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
								<tr>
									<td style="height:80px;">&nbsp;</td>
								</tr>
								<tr>
									<td>
										<table width="95%" border="0" align="center" cellpadding="0" cellspacing="0"
											style="max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);">
											<tr>
												<td style="height:40px;">&nbsp;</td>
											</tr>
											<tr>
												<td style="padding:0 35px;">
													<h1 style="color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;">
														SykiTitle</h1>
													<span
														style="display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;"></span>
													<p style="color:#455056; font-size:15px;line-height:24px; margin:0;">
														SykiDescription
													</p>
													<a href="SykiFrontendLink"
														style="background:#4caf50;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;">
														SykiButton
													</a>
												</td>
											</tr>
											<tr>
												<td style="height:40px;">&nbsp;</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="height:80px;">&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</body>
			</html>
		"""
        .Replace("SykiTitle", title)
        .Replace("SykiDescription", description)
        .Replace("SykiButton", button)
        .Replace("SykiFrontendLink", link);
	}
}
