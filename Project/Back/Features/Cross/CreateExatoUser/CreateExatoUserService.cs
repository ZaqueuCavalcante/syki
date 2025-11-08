using Exato.Shared.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class CreateExatoUserService(BackDbContext ctx, UserManager<ExatoUser> userManager) : ICrossService
{
    public async Task<OneOf<CreateExatoUserOut, ExatoError>> Create(CreateExatoUserIn data)
    {
        var orgExists = await ctx.PublicCliente.AnyAsync(c => c.ClienteId == data.OrganizationId);
        if (!orgExists) return EmpresaNaoEncontrada.I;

        if (!data.Email.IsValidEmail()) return InvalidEmail.I;

        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == data.Email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var role = await ctx.Roles.FirstOrDefaultAsync(x => x.Id == data.RoleId && x.OrganizationId == data.OrganizationId);
        if (role == null) return RoleNotFound.I;

        var user = new ExatoUser(data.OrganizationId, data.Name, data.Email);

        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded) return WeakPassword.I;

        var userRole = new ExatoUserRole(user.Id, role.Id);
        await ctx.SaveChangesAsync(userRole);

        return user.ToCreateExatoUserOut(role.Name);
    }
}
