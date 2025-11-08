using MudBlazor;
using Exato.Shared.Features.Office.BuscarUsuarios;

namespace Exato.Front.Features.Office.BuscarUsuarios;

public static class BuscarUsuariosMapper
{
    extension(BuscarUsuariosItemOut item)
    {
        public (Color color, string icon, string text) GetIsActive()
        {
            if (item.Ativo)
                return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativo");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativo");
        }
    }
}
