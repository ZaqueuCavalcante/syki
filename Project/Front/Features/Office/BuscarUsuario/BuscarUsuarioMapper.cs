using MudBlazor;
using Exato.Shared.Features.Office.BuscarUsuario;

namespace Exato.Front.Features.Office.BuscarUsuario;

public static class BuscarUsuarioMapper
{
    extension(BuscarUsuarioOut usuario)
    {
        public (Color color, string icon, string status) GetIsActive()
        {
            if (usuario.Ativo) return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativo");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativo");
        }

        public (Color color, string icon, string status) GetOnboardStatus()
        {
            if (usuario.OnboardStatus == null) return (Color.Transparent, Icons.Material.Filled.QuestionMark, "-");

            var text = usuario.OnboardStatus.GetDescription();

            if (usuario.OnboardStatus == ExatoWebOnboardStatus.Waiting) return (Color.Warning, Icons.Material.Filled.AccessTime, text);

            if (usuario.OnboardStatus == ExatoWebOnboardStatus.Completed) return (Color.Success, Icons.Material.Filled.CheckCircleOutline, text);

            return (Color.Error, Icons.Material.Filled.Error, text);
        }

        public (Color color, string method) GetPaymentMethod()
        {
            if (usuario.MetodoDePagamento == MetodoDePagamento.PrePago)
                return (Color.Tertiary, usuario.MetodoDePagamento.GetDescription());

            return (Color.Info, usuario.MetodoDePagamento.GetDescription());
        }

        public (string icon, string type) GetBalanceType()
        {
            if (usuario.BalanceType == BalanceType.Reais)
                return (Icons.Material.Filled.AttachMoney, "Saldo");

            return (Icons.Material.Filled.CreditCard, "Créditos");
        }

        public string GetBalance()
        {
            return ((int)(usuario.BalanceInBrl * 100)).ToBrl();
        }

        public string GetCreditos()
        {
            return usuario.Creditos.ToIntFormat();
        }
    }
}
