namespace Exato.Shared.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenIn : IApiDto<SendResetPasswordTokenIn>
{
    public string Email { get; set; }

    public SendResetPasswordTokenIn() {}

    public static IEnumerable<(string, SendResetPasswordTokenIn)> GetExamples() =>
    [
        ("Exemplo", new() { Email = "user@exato.com" }),
    ];
}
