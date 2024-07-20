namespace Syki.Back.Features.Cross.CreateUser;

public class CreateUserService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task<OneOf<UserOut, SykiError>> Create(CreateUserIn data)
    {
        var institutionOk = await ctx.Institutions.AnyAsync(c => c.Id == data.InstitutionId);
        if (!institutionOk) return new InstitutionNotFound();

        if (!data.Email.IsValidEmail()) return new InvalidEmail();

        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == data.Email);
        if (emailUsed) return new EmailAlreadyUsed();

        var user = new SykiUser(data.InstitutionId, data.Name, data.Email);

        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded) return new WeakPassword();

        await userManager.AddToRoleAsync(user, data.Role.ToString());

        return user.ToOut();
    }
}
