using Syki.Back.CreateInstitution;

namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterService(SykiDbContext ctx, CreateUserService service)
{
    public async Task Finish(FinishUserRegisterIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Id == id);
        if (register == null)
            Throw.DE024.Now();

        register.Finish();

        var institution = new Institution($"Institution - {register.Email}");
        ctx.Add(institution);

        if (Env.IsNotProduction())
        {
            ctx.Add(SykiTask.SeedInstitutionData(institution.Id));
        }

        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademico(institution.Id, register.Email, data.Password);
        await service.Create(userIn);

        await ctx.SaveChangesAsync();
        transaction.Commit();
    }
}
