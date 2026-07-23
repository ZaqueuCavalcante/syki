namespace Estud.Back.Features.Identity.SetTwoFactorEnforcement;

public class SetTwoFactorEnforcementService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<SetTwoFactorEnforcementOut, EstudError>> Set(SetTwoFactorEnforcementIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var role = await ctx.Roles.FirstOrDefaultAsync(r => r.InstitutionId == institutionId && r.Id == data.RoleId);
        if (role == null) return RoleNotFound.I;

        role.SetTwoFactorRequired(data.Required);

        await ctx.SaveChangesAsync();

        return new SetTwoFactorEnforcementOut
        {
            RoleId = role.Id, TwoFactorRequired = role.TwoFactorRequired,
        };
    }
}
