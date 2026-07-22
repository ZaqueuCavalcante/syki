namespace Estud.Back.Features.Identity.TwoFactorSetupLogin;

public class TwoFactorSetupNotCompleted : EstudError
{
    public static readonly TwoFactorSetupNotCompleted I = new();
    public override string Code { get; set; } = nameof(TwoFactorSetupNotCompleted);
    public override string Message { get; set; } = "É necessário concluir a configuração do 2FA antes de finalizar o login.";
}
