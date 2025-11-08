using Exato.Shared.Features.Office.CreateUser;

namespace Exato.Front.Features.Office.CreateUser;

public class CreateUserClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CreateUserOut, ErrorOut>> Create(
        int organizationId,
        string name,
        string email,
        Guid roleId)
    {
        var body = new CreateUserIn { OrganizationId = organizationId, Name = name, Email = email, RoleId = roleId };
        var response = await http.PostAsJsonAsync("office/users", body);

        return await response.Resolve<CreateUserOut>();
    }
}
