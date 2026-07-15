using Estud.Back.Domain.Identity;
using Estud.Back.Auth.Permissions;

namespace Estud.Back.Features.Identity.CreateRole;

public class CreateRoleService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateRoleIn>
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

    public async Task<OneOf<CreateRoleOut, EstudError>> Create(CreateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;
        var institutionId = ctx.RequestUser.InstitutionId;

        var upperCaseName = data.Name.Normalize().ToUpperInvariant();
        var roleAlreadyExists = await ctx.Roles.AnyAsync(x => x.OwnerId == institutionId && x.NormalizedName == upperCaseName);
        if (roleAlreadyExists) return RoleNameAlreadyExists.I;

        var role = new EstudRole(institutionId, data.Name, data.Description, data.BaseType, data.Permissions);
        var rolePermissionsOk = role.IsSubsetOf(ctx.RequestUser.Permissions);
        if (!rolePermissionsOk) return InvalidRolePermissions.I;

        ctx.Add(role);
        ctx.RecordSuccess(UserActivityType.CreateRole_Success, metadata: new { role.Id });
        await ctx.SaveChangesAsync();

        return new CreateRoleOut { Id = role.Id };
    }
}
