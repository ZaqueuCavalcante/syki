using MudBlazor;
using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Front.Features.Office.BuscarEmpresas;

public static class BuscarEmpresasMapper
{
    extension(BuscarEmpresasItemOut item)
    {
        public (Color color, string method) GetPaymentMethod()
        {
            if (item.MetodoDePagamento == MetodoDePagamento.PrePago)
                return (Color.Tertiary, item.MetodoDePagamento.GetDescription());

            return (Color.Info, item.MetodoDePagamento.GetDescription());
        }

        public (Color color, string icon, string status) GetIsActive()
        {
            if (item.Ativa) return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativa");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativa");
        }
    }
}
