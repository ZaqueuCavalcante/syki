using Exato.Shared.Features.Office.CreateCompany;

namespace Exato.Front.Features.Office.CreateCompany;

public class CreateCompanyClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CreateCompanyOut, ErrorOut>> Create(Guid externalId)
    {
        var body = new CreateCompanyIn { ExternalId = externalId };
        var response = await http.PostAsJsonAsync("office/companies", body);

        return await response.Resolve<CreateCompanyOut>();
    }
}
