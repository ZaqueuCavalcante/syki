using Exato.Web;
using Exato.Shared.Features.Office.ExcluirUsuario;

namespace Exato.Back.Features.Office.ExcluirUsuario;

public class ExcluirUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<ExcluirUsuarioOut, ExatoError>> Excluir(int id)
    {
        var user = await ctx.PublicUsers.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null || user.ExternalId == null) return UserNotFound.I;

        if (user.DeletedAt != null) return UserAlreadyDeleted.I;

        var clienteId = await ctx.PublicOrganizationUser
            .Where(x => x.UserId == id && x.ItsHisOwn)
            .Select(x => x.ClienteId)
            .SingleOrDefaultAsync();
        if (clienteId == 0) return EmpresaNaoEncontrada.I;

        var userExternalId = user.ExternalId!.Value.ToString();
        var webUserId = await webCtx.WebUserCompanies.AsNoTracking()
            .Where(x => x.UserExternalId == userExternalId)
            .Select(x => x.UserId).FirstOrDefaultAsync();
        if (webUserId == 0) return WebUserCompanyNotFound.I;

        var webUser = await webCtx.Users.FirstOrDefaultAsync(x => x.Id == webUserId);
        if (webUser == null) return WebUserNotFound.I;

        var cliente = await ctx.PublicCliente.FirstAsync(x => x.ClienteId == clienteId);
        var tokens = await ctx.PublicTokenAcesso.Where(x => x.UsuarioId == id && x.ExcluidoEm == null).ToListAsync();
        var orgs = await ctx.PublicOrganizationUser.Where(x => x.UserId == id && x.LeavedAt == null).ToListAsync();

        user.SoftDelete();
        cliente.SoftDelete();
        tokens.ForEach(x => x.SoftDelete());
        orgs.ForEach(x => x.SoftDelete());
        await ctx.SaveChangesAsync();

        webUser.DoSoftDelete();
        await webCtx.SaveChangesAsync();

        return new ExcluirUsuarioOut();
    }
}
