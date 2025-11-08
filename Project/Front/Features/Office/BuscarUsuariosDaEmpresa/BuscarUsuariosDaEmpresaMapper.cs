using MudBlazor;
using Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

namespace Exato.Front.Features.Office.BuscarUsuariosDaEmpresa;

public static class BuscarUsuariosDaEmpresaMapper
{
    extension(BuscarUsuariosDaEmpresaItemOut item)
    {
        public (Color color, string icon, string text) GetIsActive()
        {
            if (item.Ativo)
                return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativo");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativo");
        }
    }
}
