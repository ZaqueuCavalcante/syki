using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarVinculoEmpresaUsuario;

namespace Exato.Back.Features.Office.BuscarVinculoEmpresaUsuario;

public class BuscarVinculoEmpresaUsuarioService(BackDbContext ctx) : IOfficeService
{
    public async Task<OneOf<BuscarVinculoEmpresaUsuarioOut, ExatoError>> Buscar(BuscarVinculoEmpresaUsuarioIn data)
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

        var clienteExternalId = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == data.ClienteId)
            .Select(x => x.ExternalId).FirstOrDefaultAsync();
        if (clienteExternalId == Guid.Empty) return EmpresaNaoEncontrada.I;

        var userClienteId = await ctx.PublicOrganizationUser.AsNoTracking()
            .Where(x => x.UserId == data.UserId && x.ItsHisOwn)
            .Select(x => x.ClienteId)
            .FirstOrDefaultAsync();

        var userPhone = await ctx.PublicUserPhoneNumbers.AsNoTracking()
            .Where(x => x.UserId == data.UserId).FirstOrDefaultAsync();

        var dexterClienteId = userPhone?.ClienteId ?? userClienteId;

        return new BuscarVinculoEmpresaUsuarioOut()
        {
            UsedByDexter = dexterClienteId == data.ClienteId,
        };
    }
}
