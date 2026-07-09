namespace Estud.Back.Features.Identity.SendResetPasswordToken;

public class SendResetPasswordTokenIn : IApiDto<SendResetPasswordTokenIn>
{
    public string Email { get; set; }

    public SendResetPasswordTokenIn() {}

    public static IEnumerable<(string, SendResetPasswordTokenIn)> GetExamples() =>
    [
        ("Exemplo", new() { Email = "user.example@estud.com.br" }),
    ];
}
