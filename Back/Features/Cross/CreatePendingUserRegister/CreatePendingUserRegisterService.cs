namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterService(SykiDbContext ctx) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(CreatePendingUserRegisterIn data)
    {
        var email = data.Email.ToLower();

        var registerExists = await ctx.UserRegisters.AnyAsync(d => d.Email == email);
        if (registerExists) return new EmailAlreadyUsed();

        if (!email.IsValidEmail()) return new InvalidEmail();

        ctx.Add(new UserRegister(email));

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
