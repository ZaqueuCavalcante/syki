namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterService(SykiDbContext ctx) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(CreatePendingUserRegisterIn data)
    {
        var email = data.Email.ToLower();

        var registerExists = await ctx.UserRegisters.AnyAsync(d => d.Email == email);
        if (registerExists) return new EmailAlreadyUsed();

        if (!email.IsValidEmail()) return new InvalidEmail();

        var register = new UserRegister(email);
        ctx.Add(register);

        ctx.Add(SykiTask.SendUserRegisterEmailConfirmation(email));

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
