using Exato.Web;
using Exato.Web.Domain;
using Exato.Shared.Features.Office.GetCompany;

namespace Exato.Back.Features.Office.GetCompany;

public class GetCompanyService(WebDbContext webContext) : IOfficeService
{
    public async Task<GetCompanyOut> Get(Guid id)
    {
        var company = await webContext.Companies.AsNoTracking()
            .Where(x => x.ExternalId == id)
            .Select(x => new Company
            {
                Id = x.Id,
                Name = x.Name,
                Cnpj = x.Cnpj,
                Active = x.Active,
                ExternalId = x.ExternalId,
                PaymentMode = x.PaymentMode,
                OnboardDate = x.OnboardDate,
                OnboardStatus = x.OnboardStatus,
            })
            .FirstOrDefaultAsync();

        if (company == null) return new();

        return company.ToGetCompanyOut();
    }
}
