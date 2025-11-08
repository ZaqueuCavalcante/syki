using Exato.Shared.Auth;
using Exato.Shared.Features.Office.UpdateRole;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.UpdateRole;

public class UpdateRoleService(BackDbContext ctx) : IOfficeService
{
    private class Validator : AbstractValidator<UpdateRoleIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidRoleName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidRoleName.I);

            RuleFor(x => x.Description).NotEmpty().WithError(InvalidRoleDescription.I);
            RuleFor(x => x.Description).MaximumLength(50).WithError(InvalidRoleDescription.I);

            RuleFor(x => x.Features)
                .Must(x => x != null && x.IsSubsetOf(ExatoFeaturesStore.Features.ConvertAll(f => f.Id)))
                .WithError(InvalidFeaturesList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateRoleOut, ExatoError>> Update(Guid id, UpdateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;

        var role = await ctx.Roles.FirstOrDefaultAsync(x => x.Id == id);
        if (role == null) return RoleNotFound.I;

        var roleNameExists = await ctx.Roles.AnyAsync(x => x.Id != id && x.NormalizedName == data.Name.ToUpper() && x.OrganizationId == role.OrganizationId);
        if (roleNameExists) return RoleNameAlreadyExists.I;

        role.Update(data.Name, data.Description, data.Features);

        await ctx.SaveChangesAsync();

        return new UpdateRoleOut { Id = role.Id };
    }
}
