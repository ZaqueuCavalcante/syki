using Syki.Back.Features.Cross.GenerateJWT;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Features.Cross.SkipUserRegister;

public class SkipUserRegisterService(
    SykiDbContext ctx,
    GenerateJWTService generateJWTService,
    FinishUserRegisterService finishService,
    CreatePendingUserRegisterService createService
) : ICrossService
{
    public async Task<OneOf<SkipUserRegisterLoginOut, SykiError>> Skip(SkipUserRegisterLoginIn data)
    {
        var user = await ctx.Users.AsNoTracking().Where(x => x.Id == data.UserId).FirstOrDefaultAsync();

        if (user == null) data.UserId = await Register();

        return await Login(data.UserId);
    }

    public async Task<Guid> Register()
    {
        var email = $"academico.{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.skip.com";
        var password = $"Academic@{Guid.NewGuid().ToString().OnlyNumbers()}";

        await createService.Create(new(email));

        var register = await ctx.UserRegisters.FirstAsync(d => d.Email == email);

        var user = await finishService.Finish(new(register.Id.ToString(), password));
        return user.GetSuccess().Id;
    }

    public async Task<SkipUserRegisterLoginOut> Login(Guid userId)
    {
        var user = await ctx.Users.AsNoTracking().Where(x => x.Id == userId).FirstAsync();

        var jwt = await generateJWTService.Generate(user.Email);

        return new() { UserId = userId, AccessToken = jwt };
    }
}
