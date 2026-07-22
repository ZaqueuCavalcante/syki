using Estud.Back.Auth.Schemes;
using Estud.Back.Features.Cross.SignIn;
using Microsoft.AspNetCore.Authentication;

namespace Estud.Back.Features.Identity.TwoFactorSetupLogin;

public class TwoFactorSetupLoginService(
    EstudDbContext ctx,
    SignInService signInService,
    IHttpContextAccessor httpCtx) : IEstudService
{
    public async Task<OneOf<TwoFactorSetupLoginOut, EstudError>> Login()
    {
        var user = await ctx.Users
            .Where(u => u.Id == ctx.RequestUser.Id)
            .Select(u => new { u.Id, u.Email, u.TwoFactorEnabled })
            .FirstAsync();

        // Gate de segurança: a credencial de setup foi emitida só com email+senha.
        // Sem esta checagem, quem tem apenas a senha trocaria o cookie de setup por
        // um JWT full, furando o 2FA. Só emitimos o JWT depois que o setup foi de fato
        // concluído (2FA habilitado no passo anterior, provando posse do segundo fator).
        if (!user.TwoFactorEnabled) return TwoFactorSetupNotCompleted.I;

        var signInResult = await signInService.SignIn(user.Email!);
        await httpCtx.HttpContext!.SignOutAsync(TwoFactorSetupScheme.Name);

        return signInResult.ToTwoFactorSetupLoginOut();
    }
}
