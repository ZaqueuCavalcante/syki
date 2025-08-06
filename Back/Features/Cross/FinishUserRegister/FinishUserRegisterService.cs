using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterService(SykiDbContext ctx, CreateUserService service, HybridCache cache) : ICrossService
{
    public async Task<OneOf<UserOut, SykiError>> Finish(FinishUserRegisterIn data)
    {
        _ = Guid.TryParse(data.Token, out var id);
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Id == id);
        if (register == null) return new InvalidRegistrationToken();

        var finish = register.Finish();
        if (finish.IsError) return finish.Error;

        var institution = new Institution(register.InstitutionId, BrazilianInstitutions.PickRandom());

        ctx.Add(institution);
        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademic(institution.Id, register.Email, data.Password);
        var result = await service.Create(userIn);

        if (result.IsError) return result.Error;

        var user = result.Success;

        await ctx.SaveChangesAsync();

        await cache.RemoveAsync("users");
        await cache.RemoveAsync("institutions");

        return user;
    }
}
