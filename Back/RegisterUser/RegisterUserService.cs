using Syki.Back.CreateUser;
using Syki.Back.SendResetPasswordToken;

namespace Syki.Back.RegisterUser;

public class RegisterUserService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task<CreateUserOut> Register(CreateUserIn body)
    {
        var userOut = await RegisterUser(body);

        var user = await userManager.FindByIdAsync(userOut.Id.ToString());

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, token);
        ctx.Add(reset);

        await ctx.SaveChangesAsync();

        return userOut;
    }

    private async Task<CreateUserOut> RegisterUser(CreateUserIn body)
    {
        if (!(body.Role is AuthorizationConfigs.Academico or AuthorizationConfigs.Professor or AuthorizationConfigs.Aluno))
            Throw.DE013.Now();

        var faculdadeOk = await ctx.Institutions.AnyAsync(c => c.Id == body.InstitutionId);
        if (!faculdadeOk)
            Throw.DE014.Now();

        if (!body.Email.IsValidEmail())
            Throw.DE016.Now();

        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == body.Email);
        if (emailUsed)
            Throw.DE017.Now();

        var user = new SykiUser(body.InstitutionId, body.Name, body.Email);

        var result = await userManager.CreateAsync(user, body.Password);
        if (!result.Succeeded)
            Throw.DE015.Now();

        await userManager.AddToRoleAsync(user, body.Role);

        return user.ToOut();
    }
}
