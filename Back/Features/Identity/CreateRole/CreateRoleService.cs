using Syki.Back.Domain.Identity;
using Syki.Back.Auth.Permissions;

namespace Syki.Back.Features.Identity.CreateRole;

public class CreateRoleService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CreateRoleIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidRoleName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidRoleName.I);

            RuleFor(x => x.Description).NotEmpty().WithError(InvalidRoleDescription.I);
            RuleFor(x => x.Description).MaximumLength(200).WithError(InvalidRoleDescription.I);

            RuleFor(x => x.Permissions)
                .Must(x => x != null && x.IsAllDistinct() && x.IsSubsetOf(SykiPermissions.Permissions.ConvertAll(p => p.Id)))
                .WithError(InvalidPermissionsList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateRoleOut, SykiError>> Create(CreateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;

        var orgId = ctx.RequestUser.InstitutionId;
        var upperCaseName = data.Name.Normalize().ToUpperInvariant();
        var roleAlreadyExists = await ctx.Roles.AnyAsync(x => x.OwnerId == orgId && x.NormalizedName == upperCaseName);
        if (roleAlreadyExists) return RoleNameAlreadyExists.I;

        var role = new SykiRole(orgId, data.Name, data.Description, data.Permissions);
        var rolePermissionsOk = role.IsSubsetOf(ctx.RequestUser.Permissions);
        if (!rolePermissionsOk) return InvalidRolePermissions.I;

        var orgRole = new InstitutionRole(orgId, role);
        ctx.AddRange(role, orgRole);
        await ctx.SaveChangesAsync();

        return new CreateRoleOut { Id = role.Id };
    }
}
