namespace Estud.Back.Features.Identity.GetTwoFactorEnforcement;

public class GetTwoFactorEnforcementService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetTwoFactorEnforcementOut> Get()
    {
        var roles = await ctx.Roles.AsNoTracking()
            .Where(r => r.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(r => r.Name)
            .Select(r => new GetTwoFactorEnforcementItemOut
            {
                RoleId = r.Id,
                Name = r.Name!,
                BaseType = r.BaseType,
                TwoFactorRequired = r.TwoFactorRequired,
            })
            .ToListAsync();

        return new GetTwoFactorEnforcementOut
        {
            Total = roles.Count,
            Items = roles,
        };
    }
}
