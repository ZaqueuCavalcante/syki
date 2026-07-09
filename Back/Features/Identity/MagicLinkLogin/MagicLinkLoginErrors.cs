namespace Estud.Back.Features.Identity.MagicLinkLogin;

public class InvalidMagicLink : EstudError
{
    public static readonly InvalidMagicLink I = new();
    public override string Code { get; set; } = nameof(InvalidMagicLink);
    public override string Message { get; set; } = "Link de acesso inválido ou expirado.";
}
