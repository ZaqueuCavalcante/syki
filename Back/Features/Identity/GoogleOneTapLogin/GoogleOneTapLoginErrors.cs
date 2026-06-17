namespace Syki.Back.Features.Identity.GoogleOneTapLogin;

public class GoogleOneTapLoginInvalidToken : SykiError
{
    public static readonly GoogleOneTapLoginInvalidToken I = new();
    public override string Code { get; set; } = nameof(GoogleOneTapLoginInvalidToken);
    public override string Message { get; set; } = "Token do Google inválido.";
}

public class GoogleOneTapLoginDisabled : SykiError
{
    public static readonly GoogleOneTapLoginDisabled I = new();
    public override string Code { get; set; } = nameof(GoogleOneTapLoginDisabled);
    public override string Message { get; set; } = "Login com Google One Tap não está disponível.";
}
