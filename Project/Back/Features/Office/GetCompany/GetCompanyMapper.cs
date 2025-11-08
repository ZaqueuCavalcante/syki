using Exato.Web.Domain;
using Exato.Shared.Features.Office.GetCompany;

namespace Exato.Back.Features.Office.GetCompany;

public static class GetCompanyMapper
{
    extension(Company company)
    {
        public GetCompanyOut ToGetCompanyOut()
        {
            return new()
            {
                Id = company.Id,
                Name = company.Name,
                Active = company.Active,
                OnboardDate = company.OnboardDate,
                Cnpj = company.Cnpj.PadLeft(14, '0'),
                ExternalId = company.ExternalId!.Value,
                PaymentMode = company.PaymentMode.IntToEnum<CompanyPaymentMode>(),
                OnboardStatus = company.OnboardStatus.IntToEnum<ExatoWebOnboardStatus>(),
            };
        }
    }
}
