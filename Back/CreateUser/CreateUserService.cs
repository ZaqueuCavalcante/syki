using Syki.Shared;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Shared.CreateUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.CreateUser;

public class CreateUserService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task<CreateUserOut> Create(CreateUserIn body)
    {
        if (!(body.Role is Academico or Professor or Aluno))
            Throw.DE013.Now();

        var institutionOk = await ctx.Institutions.AnyAsync(c => c.Id == body.InstitutionId);
        if (!institutionOk)
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
