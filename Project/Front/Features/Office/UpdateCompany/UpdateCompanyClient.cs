using Exato.Shared.Features.Office.UpdateCompany;

namespace Exato.Front.Features.Office.UpdateCompany;

public class UpdateCompanyClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<UpdateCompanyOut, ErrorOut>> Update(Guid externalId, ExatoWebOnboardStatus onboardStatus)
    {
        var body = new UpdateCompanyIn { OnboardStatus = onboardStatus };

        var response = await http.PutAsJsonAsync($"office/companies/{externalId}", body);

        return await response.Resolve<UpdateCompanyOut>();
    }
}
