using Exato.Web;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.DesvincularEmpresaUsuario;

namespace Exato.Back.Features.Office.DesvincularEmpresaUsuario;

public class DesvincularEmpresaUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<DesvincularEmpresaUsuarioOut, ExatoError>> Desvincular(DesvincularEmpresaUsuarioIn data)
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
        if (webUserId == 0) return WebUserCompanyNotFound.I;

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var token = await ctx.PublicTokenAcesso
            .FirstOrDefaultAsync(x => x.ExcluidoEm == null && x.KeyType == TokenAcessoKeyType.Type3.ToShort() && x.ClienteId == data.ClienteId && x.UsuarioId == user.Id);
        var orgUser = await ctx.PublicOrganizationUser
            .FirstOrDefaultAsync(x => x.LeavedAt == null && x.ItsHisOwn == false && x.ClienteId == data.ClienteId && x.UserId == user.Id);

        if (token == null || orgUser == null) return VinculoEmpresaUsuarioNaoExiste.I;

        token.SoftDelete();
        orgUser.SoftDelete();

        var userClienteId = await ctx.PublicOrganizationUser
            .Where(x => x.UserId == data.UserId && x.ItsHisOwn)
            .Select(x => x.ClienteId).FirstAsync();
        var userPhones = await ctx.PublicUserPhoneNumbers
            .Where(x => x.UserId == data.UserId).ToListAsync();
        userPhones?.ForEach(x => x.ClienteId = userClienteId);

        await ctx.SaveChangesAsync();

        await using var webTransaction = await webCtx.Database.BeginTransactionAsync();

        var userCompany = await webCtx.WebUserCompanies.FirstOrDefaultAsync(
            x => x.UserId == webUserId && x.UserExternalId == userExternalId && x.OrganizationExternalId == clienteExternalId.ToString());

        if (userCompany == null) return new DesvincularEmpresaUsuarioOut();

        webCtx.Remove(userCompany);
        await webCtx.SaveChangesAsync();
        
        await transaction.CommitAsync();
        await webTransaction.CommitAsync();

        return new DesvincularEmpresaUsuarioOut();
    }
}
