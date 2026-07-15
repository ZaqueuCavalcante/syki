using Estud.Back.Auth.Permissions;

namespace Estud.Back.Features.Identity.UpdateRole;

public class UpdateRoleService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateRoleIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidRoleName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidRoleName.I);

            RuleFor(x => x.Description).NotEmpty().WithError(InvalidRoleDescription.I);
            RuleFor(x => x.Description).MaximumLength(200).WithError(InvalidRoleDescription.I);

            RuleFor(x => x.BaseType).IsInEnum().WithError(InvalidRoleBaseType.I);

            RuleFor(x => x.Permissions)
                .Must(x => x != null && x.IsAllDistinct() && x.IsSubsetOf(EstudPermissions.Permissions.ConvertAll(p => p.Id)))
                .WithError(InvalidPermissionsList.I);

            RuleFor(x => x)
                .Must(x => x.Permissions.All(id => EstudPermissions.IsAllowedFor(id, x.BaseType)))
                .WithError(InvalidPermissionsForUserType.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateRoleOut, EstudError>> Update(UpdateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var role = await ctx.Roles.FirstOrDefaultAsync(r => r.OwnerId == institutionId && r.Id == data.Id);
        if (role == null) return RoleNotFound.I;

        var upperCaseName = data.Name.Normalize().ToUpperInvariant();
        var nameConflict = await ctx.Roles.AnyAsync(r => r.OwnerId == institutionId && r.NormalizedName == upperCaseName && r.Id != data.Id);
        if (nameConflict) return RoleNameAlreadyExists.I;

        var rolePermissionsOk = role.IsSubsetOf(ctx.RequestUser.Permissions) && data.Permissions.IsSubsetOf(ctx.RequestUser.Permissions);
        if (!rolePermissionsOk) return InvalidRolePermissions.I;

        role.Update(data.Name, upperCaseName, data.Description, data.BaseType, data.Permissions);

        await ctx.SaveChangesAsync();

        return new UpdateRoleOut { Id = role.Id };
    }
}
