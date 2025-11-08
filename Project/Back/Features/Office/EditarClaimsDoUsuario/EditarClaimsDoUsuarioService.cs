using Exato.Web;
using Exato.Web.Extensions;
using Exato.Shared.Features.Office.EditarClaimsDoUsuario;

namespace Exato.Back.Features.Office.EditarClaimsDoUsuario;

public class EditarClaimsDoUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<EditarClaimsDoUsuarioOut, ExatoError>> Editar(int id, EditarClaimsDoUsuarioIn data)
    {
        var userExternalIdGuid = await ctx.PublicUsers.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.ExternalId)
            .FirstOrDefaultAsync();

        if (userExternalIdGuid == null) return UserNotFound.I;

        var userExternalId = userExternalIdGuid.ToString();

        var webUserId = await webCtx.WebUserCompanies.AsNoTracking()
            .Where(x => x.UserExternalId == userExternalId)
            .Select(x => x.UserId).FirstOrDefaultAsync();

        var webUser = await webCtx.Users
            .Where(x => x.Id == webUserId)
            .FirstOrDefaultAsync();

        webUser.ExtraClaims = data.Claims.ToPermissions();

        await webCtx.SaveChangesAsync();

        return new EditarClaimsDoUsuarioOut() { Id = id };
    }
}
