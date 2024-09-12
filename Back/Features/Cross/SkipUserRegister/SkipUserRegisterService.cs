using Syki.Back.Features.Cross.Login;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Features.Cross.SkipUserRegister;

public class SkipUserRegisterService(
    SykiDbContext ctx,
    LoginService loginService,
    FinishUserRegisterService finishService,
    CreatePendingUserRegisterService createService
) : ICrossService
{
    public async Task<OneOf<SkipRegisterLoginOut, SykiError>> Skip(SkipRegisterLoginIn data)
    {
        if (data.InstitutionId == Guid.Empty) return await RegisterAndLogin();

        return await Login(data.InstitutionId);
    }

    public async Task<SkipRegisterLoginOut> RegisterAndLogin()
    {
        var email = $"academico.157{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.seed.com";
        var password = "Test@123";

        await createService.Create(new(email));

        var register = await ctx.UserRegisters.FirstAsync(d => d.Email == email);

        var user = await finishService.Finish(new(register.Id.ToString(), password));

        var result = await loginService.Login(new(email, password));

        return new() { InstitutionId = user.GetSuccess().InstitutionId, AccessToken = result.AccessToken };
    }

    public async Task<SkipRegisterLoginOut> Login(Guid institutionId)
    {
        var user = await ctx.Users.Where(x => x.InstitutionId == institutionId && x.Email.StartsWith("academico.157")).FirstOrDefaultAsync();
        if (user == null) return await RegisterAndLogin();

        var result = await loginService.Login(new(user.Email, "Test@123"));

        return new() { InstitutionId = institutionId, AccessToken = result.AccessToken };
    }
}
