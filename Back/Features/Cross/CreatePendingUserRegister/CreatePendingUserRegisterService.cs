namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterService(SykiDbContext ctx)
{
    public async Task Create(CreatePendingUserRegisterIn data)
    {
        var email = data.Email.ToLower();

        var registerExists = await ctx.UserRegisters.AnyAsync(d => d.Email == email);
        if (registerExists)
            Throw.DE017.Now();

        var register = new UserRegister(email);
        ctx.Add(register);

        ctx.Add(SykiTask.SendUserRegisterEmailConfirmation(email));

        await ctx.SaveChangesAsync();
    }
}
