namespace Syki.Back.Features.Cross.CreateUser;

public class CreateUserService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<OneOf<UserOut, SykiError>> Create(CreateUserIn data)
    {
        var institutionExists = await ctx.Institutions.AnyAsync(c => c.Id == data.InstitutionId);
        if (!institutionExists) return new InstitutionNotFound();

        if (!data.Email.IsValidEmail()) return new InvalidEmail();

        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == data.Email);
        if (emailUsed) return new EmailAlreadyUsed();

        var user = new SykiUser(data.InstitutionId, data.Name, data.Email);

        // TODO: Levar em conta mais o q pode dar problema aqui
        // Caractere invalido no email
        // Lockout apos mais de 3 tentativas de login com falha
        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded) return new WeakPassword();

        await userManager.AddToRoleAsync(user, data.Role.ToString());

        return user.ToOut();
    }
}
