using MudBlazor;

namespace Exato.Front.Features.Office.EditarSaldoDaEmpresa;

public static class EditarSaldoDaEmpresaMapper
{
    extension(EditarSaldoDaEmpresaFormData data)
    {
        public (string icon, string type) GetBalanceType()
        {
            if (data.BalanceType == BalanceType.Reais)
                return (Icons.Material.Filled.AttachMoney, "Saldo");

            return (Icons.Material.Filled.CreditCard, "Créditos");
        }
    }
}
