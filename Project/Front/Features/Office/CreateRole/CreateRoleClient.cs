using Exato.Shared.Features.Office.CreateRole;

namespace Exato.Front.Features.Office.CreateRole;

public class CreateRoleClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CreateRoleOut, ErrorOut>> Create(
        string name,
        string description,
        int organizationId,
        List<int> features)
    {
        var body = new CreateRoleIn { Name = name, Description = description, OrganizationId = organizationId, Features = features };
        var response = await http.PostAsJsonAsync("office/roles", body);

        return await response.Resolve<CreateRoleOut>();
    }
}
