using Exato.Web;
using Exato.Web.Domain;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.VincularEmpresaUsuario;

namespace Exato.Back.Features.Office.VincularEmpresaUsuario;

public class VincularEmpresaUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<VincularEmpresaUsuarioOut, ExatoError>> Vincular(VincularEmpresaUsuarioIn data)
    {
        var user = await ctx.PublicUsers.AsNoTracking()
            .Where(x => x.Id == data.UserId)
            .Select(x => new User
            {
                Id = x.Id,
                ExternalId = x.ExternalId,
            })
            .FirstOrDefaultAsync();
        if (user == null || user.ExternalId == null) return UserNotFound.I;

        var clienteExternalId = await ctx.PublicCliente
            .Where(x => x.ClienteId == data.ClienteId)
            .Select(x => x.ExternalId).FirstOrDefaultAsync();
        if (clienteExternalId == Guid.Empty) return EmpresaNaoEncontrada.I;

        var userExternalId = user.ExternalId!.Value.ToString();
        var webUserId = await webCtx.WebUserCompanies.AsNoTracking()
            .Where(x => x.UserExternalId == userExternalId)
            .Select(x => x.UserId).FirstOrDefaultAsync();

        var orgUserAlreadyExists = await ctx.PublicOrganizationUser
            .AnyAsync(x => x.LeavedAt == null && x.ClienteId == data.ClienteId && x.UserId == user.Id);
        if (orgUserAlreadyExists) return UsuarioJaVinculadoAEmpresaNoIntelligence.I;

        var orgUserTokenAlreadyExists = await ctx.PublicTokenAcesso
            .AnyAsync(x => x.ExcluidoEm == null && x.KeyType == TokenAcessoKeyType.Type3.ToShort() && x.ClienteId == data.ClienteId && x.UsuarioId == user.Id);
        if (orgUserTokenAlreadyExists) return TokenDeUsuarioJaVinculadoAEmpresaNoIntelligence.I;

        var userCompanyAlreadyExists = await webCtx.WebUserCompanies
            .AnyAsync(x => x.OrganizationExternalId == clienteExternalId.ToString() && x.UserId == webUserId);
        if (userCompanyAlreadyExists) return UsuarioJaVinculadoAEmpresaNoExatoWeb.I;

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var token = new TokenAcesso(data.ClienteId, user.Id);
        var orgUser = new OrganizationUser(data.ClienteId, user.Id, false);

        ctx.AddRange(token, orgUser);
        await ctx.SaveChangesAsync();

        await using var webTransaction = await webCtx.Database.BeginTransactionAsync();

        var companyId = await webCtx.Companies.Where(x => x.ExternalId == clienteExternalId).Select(x => x.Id).FirstOrDefaultAsync();
        var userCompany = new WebUserCompany(webUserId, user.ExternalId!.Value, clienteExternalId, token.Token, companyId);

        await webCtx.SaveChangesAsync(userCompany);

        await transaction.CommitAsync();
        await webTransaction.CommitAsync();

        return new VincularEmpresaUsuarioOut();
    }
}
