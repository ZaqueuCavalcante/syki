using MudBlazor;
using Exato.Shared.Features.Office.BuscarTokensDeAcesso;

namespace Exato.Front.Features.Office.BuscarTokensDeAcesso;

public static class BuscarTokensDeAcessoMapper
{
    extension(BuscarTokensDeAcessoItemOut token)
    {
        public (Color color, string icon, string status) GetStatus()
        {
            if (token.ExcluidoEm != null) return (Color.Error, Icons.Material.Filled.RemoveCircleOutline, "Excluído");

            if (token.ValidoAte < DateTime.Now) return (Color.Default, Icons.Material.Filled.HourglassEmpty, "Expirado");

            return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativo");
        }
    }
}
