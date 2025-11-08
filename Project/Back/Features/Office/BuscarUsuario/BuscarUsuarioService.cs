using Exato.Web;
using Exato.Web.Domain;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarUsuario;

namespace Exato.Back.Features.Office.BuscarUsuario;

public class BuscarUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<BuscarUsuarioOut, ExatoError>> Get(int id)
    {
        var user = await ctx.PublicUsers.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new User
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Cpf = x.Cpf,
                CreatedAt = x.CreatedAt,
                Active = x.Active,
                ExternalId = x.ExternalId,
                DeletedAt = x.DeletedAt,
            })
            .FirstOrDefaultAsync();

        if (user == null) return UserNotFound.I;

        var userClienteId = await ctx.PublicOrganizationUser.AsNoTracking()
            .Where(x => x.UserId == id && x.ItsHisOwn)
            .Select(x => x.ClienteId)
            .FirstOrDefaultAsync();

        var userCliente = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == userClienteId)
            .Select(x => new Cliente
            {
                Saldo = x.Saldo,
                ClienteId = x.ClienteId,
                BalanceType = x.BalanceType,
                BalanceInBrl = x.BalanceInBrl,
                FaturamentoTipoId = x.FaturamentoTipoId,
            })
            .FirstOrDefaultAsync() ?? new();

        var userPhone = await ctx.PublicUserPhoneNumbers.AsNoTracking()
            .Where(x => x.UserId == id).FirstOrDefaultAsync();

        var dexterClienteId = userPhone?.ClienteId ?? userClienteId;
        var dexterCliente = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == dexterClienteId)
            .Select(x => new Cliente
            {
                ClienteId = x.ClienteId, Nome = x.Nome,
            })
            .FirstOrDefaultAsync();

        if (user.ExternalId == null) return user.ToBuscarUsuarioOut(userCliente, dexterCliente, null, null);

        var userExternalId = user.ExternalId.ToString();
        var webUserId = await webCtx.WebUserCompanies.AsNoTracking()
            .Where(x => x.UserExternalId == userExternalId)
            .Select(x => x.UserId).FirstOrDefaultAsync();

        var phone = await webCtx.UserPhoneNumbers
            .Where(x => x.UserId == webUserId && x.Main)
            .Select(x => new { x.Ddd, x.Number }).FirstOrDefaultAsync();

        var webUser = await webCtx.Users.AsNoTracking()
            .Where(x => x.Id == webUserId)
            .Select(x => new WebUser
            {
                Id = x.Id,
                OnboardStatus = x.OnboardStatus,
                ExtraClaims = x.ExtraClaims,
            })
            .FirstOrDefaultAsync();

        return user.ToBuscarUsuarioOut(userCliente, dexterCliente, webUser, phone?.Ddd + phone?.Number);
    }
}
