using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.CreateUser;

public class CreateUserService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task<CreateUserOut> Create(CreateUserIn data)
    {
        if (!(data.Role is Academico or Professor or Aluno))
            Throw.DE013.Now();

        var institutionOk = await ctx.Institutions.AnyAsync(c => c.Id == data.InstitutionId);
        if (!institutionOk)
            Throw.DE014.Now();

        if (!data.Email.IsValidEmail())
            Throw.DE016.Now();

        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == data.Email);
        if (emailUsed)
            Throw.DE017.Now();

        var user = new SykiUser(data.InstitutionId, data.Name, data.Email);

        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
            Throw.DE015.Now();

        await userManager.AddToRoleAsync(user, data.Role);

        return user.ToOut();
    }
}
