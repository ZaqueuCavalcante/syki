using MudBlazor;
using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Front.Features.Office.BuscarEmpresa;

public static class BuscarEmpresaMapper
{
    extension(BuscarEmpresaOut empresa)
    {
        public (Color color, string icon, string status) GetIsActive()
        {
            if (empresa.Ativa) return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativa");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativa");
        }

        public (Color color, string icon, string status) GetIsBillingCustomer()
        {
            if (empresa.IsBillingCustomer)
                return (Color.Success, Icons.Material.Filled.AttachMoney, "Habilitado");

            return (Color.Default, Icons.Material.Filled.MoneyOff, "Inabilitado");
        }

        public (Color color, string method) GetPaymentMethod()
        {
            if (empresa.MetodoDePagamento == MetodoDePagamento.PrePago)
                return (Color.Tertiary, empresa.MetodoDePagamento.GetDescription());

            return (Color.Info, empresa.MetodoDePagamento.GetDescription());
        }

        public (string icon, string type) GetBalanceType()
        {
            if (empresa.BalanceType == BalanceType.Reais)
                return (Icons.Material.Filled.AttachMoney, "Saldo");

            return (Icons.Material.Filled.CreditCard, "Créditos");
        }

        public string GetBalance()
        {
            return ((int)(empresa.BalanceInBrl * 100)).ToBrl();
        }

        public string GetCreditos()
        {
            return empresa.Creditos.ToIntFormat();
        }
    }
}
