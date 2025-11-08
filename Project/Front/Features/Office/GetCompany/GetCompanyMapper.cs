using MudBlazor;
using Exato.Shared.Features.Office.GetCompany;

namespace Exato.Front.Features.Office.GetCompany;

public static class GetCompanyMapper
{
    extension(GetCompanyOut company)
    {
        public (Color color, string icon, string status) GetIsActive()
        {
            if (company.Active) return (Color.Success, Icons.Material.Filled.CheckCircle, "Ativa");

            return (Color.Default, Icons.Material.Filled.RemoveCircleOutline, "Inativa");
        }

        public (Color color, string icon, string status) GetOnboardStatus()
        {
            var text = company.OnboardStatus.GetDescription();

            if (company.OnboardStatus == ExatoWebOnboardStatus.Waiting) return (Color.Warning, Icons.Material.Filled.AccessTime, text);

            if (company.OnboardStatus == ExatoWebOnboardStatus.Completed) return (Color.Success, Icons.Material.Filled.CheckCircleOutline, text);

            return (Color.Error, Icons.Material.Filled.Error, text);
        }

        public (Color color, string method) GetPaymentMode()
        {
            if (company.PaymentMode == CompanyPaymentMode.PrePago)
                return (Color.Tertiary, company.PaymentMode.GetDescription());

            return (Color.Info, company.PaymentMode.GetDescription());
        }
    }
}
