using Syki.Back.Domain;
using Syki.Back.CreateUser;
using Syki.Shared.CreateUser;
using Syki.Shared.FinishUserRegister;

namespace Syki.Back.FinishUserRegister;

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

        var institution = new Faculdade($"Instituição - {register.Email}");
        ctx.Add(institution);
        ctx.Add(SykiTask.SeedInstitutionData(institution.Id));
        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademico(institution.Id, register.Email, data.Password);
        await service.Create(userIn);

        await ctx.SaveChangesAsync();
        transaction.Commit();
    }
}
