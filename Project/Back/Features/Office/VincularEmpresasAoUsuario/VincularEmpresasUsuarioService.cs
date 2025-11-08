using Exato.Back.Features.Office.VincularEmpresaUsuario;
using Exato.Shared.Features.Office.VincularEmpresasAoUsuario;

namespace Exato.Back.Features.Office.VincularEmpresasAoUsuario;

public class VincularEmpresasAoUsuarioService(VincularEmpresaUsuarioService service) : IOfficeService
{
    public async Task<OneOf<VincularEmpresasAoUsuarioOut, ExatoError>> Vincular(int id, VincularEmpresasAoUsuarioIn data)
    {
        if (!data.Empresas.IsAllDistinct()) return ListaDeEmpresasInvalida.I;

        foreach (var empresa in data.Empresas)
        {
            var result = await service.Vincular(new() { UserId = id, ClienteId = empresa });
            if (result.IsError) return result.Error;
        }

        return new VincularEmpresasAoUsuarioOut();
    }
}
