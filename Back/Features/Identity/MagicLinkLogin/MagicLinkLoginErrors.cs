namespace Syki.Back.Features.Identity.MagicLinkLogin;

public class InvalidMagicLinkToken : SykiError
{
    public static readonly InvalidMagicLinkToken I = new();
    public override string Code { get; set; } = nameof(InvalidMagicLinkToken);
    public override string Message { get; set; } = "Link de acesso inválido ou expirado.";
}
