using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterService(SykiDbContext ctx, CreateUserService service)
{
    public async Task<OneOf<UserOut, SykiError>> Finish(FinishUserRegisterIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Id == id);
        if (register == null) return new InvalidRegistrationToken();

        var finish = register.Finish();
        if (finish.IsError()) return finish.GetError();

        var institution = new Institution($"Institution - {register.Email}");
        ctx.Add(institution);

        if (!Env.IsTesting()) ctx.Add(SykiTask.SeedInstitutionData(institution.Id));

        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademic(institution.Id, register.Email, data.Password);
        var result = await service.Create(userIn);

        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();

        await ctx.SaveChangesAsync();
        await transaction.CommitAsync();

        return user;
    }
}
