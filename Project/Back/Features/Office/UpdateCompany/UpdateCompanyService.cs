using Exato.Web;
using Exato.Shared.Features.Office.UpdateCompany;

namespace Exato.Back.Features.Office.UpdateCompany;

public class UpdateCompanyService(WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<UpdateCompanyOut, ExatoError>> Update(Guid id, UpdateCompanyIn data)
    {
        var company = await webCtx.Companies.FirstOrDefaultAsync(x => x.ExternalId == id);

        if (company == null) return WebCompanyNotFound.I;

        company.UpdateOnboardStatus(data.OnboardStatus);

        await webCtx.SaveChangesAsync();

        return new UpdateCompanyOut();
    }
}
