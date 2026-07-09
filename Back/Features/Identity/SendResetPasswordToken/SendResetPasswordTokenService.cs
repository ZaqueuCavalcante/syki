using Estud.Back.Domain.Identity;

namespace Estud.Back.Features.Identity.SendResetPasswordToken;

public class SendResetPasswordTokenService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return EstudSuccess.I; // Don't reveal if the email is registered or not

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, user.InstitutionId, token);
        ctx.AddCommand(user.InstitutionId, new SendResetPasswordTokenEmailCommand(user.Email, reset.Id), maxRetries: 1);
        await ctx.SaveChangesAsync(reset);

        return EstudSuccess.I;
    }
}
