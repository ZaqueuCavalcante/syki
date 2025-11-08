using MudBlazor;
using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

namespace Exato.Front.Features.Office.BuscarEmpresasDoUsuario;

public static class BuscarEmpresasDoUsuarioMapper
{
    extension(BuscarEmpresasDoUsuarioItemOut item)
    {
        public (Color color, string icon, string status) GetIsActive()
        {
            if (item.Ativa) return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativa");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativa");
        }
    }
}
