using Exato.Shared.Features.Office.UpdateRole;

namespace Exato.Front.Features.Office.UpdateRole;

public class UpdateRoleClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<UpdateRoleOut, ErrorOut>> Update(
        Guid id,
        string name,
        string description,
        List<int> features)
    {
        var body = new UpdateRoleIn { Name = name, Description = description, Features = features };
        var response = await http.PutAsJsonAsync($"office/roles/{id}", body);

        return await response.Resolve<UpdateRoleOut>();
    }
}
