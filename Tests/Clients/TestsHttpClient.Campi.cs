using System.Net.Http.Json;
using Syki.Back.Features.Campi.CreateCampus;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateCampusOut, ErrorOut>> CreateCampus(
        string name = "Agreste I",
        BrazilState? state = BrazilState.PE,
        string city = "Caruaru",
        int capacity = 100
    ) {
        var data = new CreateCampusIn { Name = name, State = state, City = city, Capacity = capacity };
        var response = await http.PostAsJsonAsync("/campi", data);
        return await response.Resolve<CreateCampusOut>();
    }
}
