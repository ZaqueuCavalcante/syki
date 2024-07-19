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
        if (finish.IsT1) return finish.AsT1;

        var institution = new Institution($"Institution - {register.Email}");
        ctx.Add(institution);

        if (Env.IsDevelopment()) ctx.Add(SykiTask.SeedInstitutionData(institution.Id));

        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademic(institution.Id, register.Email, data.Password);
        var result = await service.Create(userIn);

        return await result.Match<Task<OneOf<UserOut, SykiError>>>(
            async user =>
            {
                await ctx.SaveChangesAsync();
                transaction.Commit();
                return user;
            },
            error => Task.FromResult<OneOf<UserOut, SykiError>>(error)
        );
    }
}
