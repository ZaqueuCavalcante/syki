using Syki.Shared;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.CreateUser;

public class CreateUserService
{
    private readonly SykiDbContext _ctx;
    private readonly UserManager<SykiUser> _userManager;
    public CreateUserService(
        SykiDbContext ctx,
        UserManager<SykiUser> userManager
    ) {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<UserOut> Create(UserIn body)
    {
        if (!(body.Role is Academico or Professor or Aluno))
            Throw.DE013.Now();

        var faculdadeOk = await _ctx.Faculdades.AnyAsync(c => c.Id == body.InstitutionId);
        if (!faculdadeOk)
            Throw.DE014.Now();

        if (!body.Email.IsValidEmail())
            Throw.DE016.Now();

        var emailUsed = await _ctx.Users.AnyAsync(u => u.Email == body.Email);
        if (emailUsed)
            Throw.DE017.Now();

        var user = new SykiUser(body.InstitutionId, body.Name, body.Email);

        var result = await _userManager.CreateAsync(user, body.Password);
        if (!result.Succeeded)
            Throw.DE015.Now();

        await _userManager.AddToRoleAsync(user, body.Role);

        return user.ToOut();
    }
}
