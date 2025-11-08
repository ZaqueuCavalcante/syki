using Exato.Shared.Auth;
using Exato.Shared.Features.Office.CreateRole;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.CreateRole;

public class CreateRoleService(BackDbContext ctx) : IOfficeService
{
    private class Validator : AbstractValidator<CreateRoleIn>
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

    public async Task<OneOf<CreateRoleOut, ExatoError>> Create(CreateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;

        var orgExists = await ctx.PublicCliente.AnyAsync(c => c.ClienteId == data.OrganizationId);
        if (!orgExists) return EmpresaNaoEncontrada.I;

        var roleNameExists = await ctx.Roles.AnyAsync(x => x.NormalizedName == data.Name.ToUpper() && x.OrganizationId == data.OrganizationId);
        if (roleNameExists) return RoleNameAlreadyExists.I;

        var role = new ExatoRole(data.Name, data.Description, data.OrganizationId, data.Features);

        await ctx.SaveChangesAsync(role);

        return new CreateRoleOut { Id = role.Id };
    }
}
