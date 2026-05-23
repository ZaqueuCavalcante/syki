using System.Net.Http.Json;
using Syki.Back.Features.Campi.GetCampi;
using Syki.Back.Features.Campi.CreateCampus;
using Syki.Back.Features.Campi.UpdateCampus;

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

    public async Task<OneOf<UpdateCampusOut, ErrorOut>> UpdateCampus(
        int id,
        string name = "Agreste II",
        BrazilState? state = BrazilState.PE,
        string city = "Bonito",
        int capacity = 200
    ) {
        var data = new UpdateCampusIn { Id = id, Name = name, State = state, City = city, Capacity = capacity };
        var response = await http.PutAsJsonAsync("/campi", data);
        return await response.Resolve<UpdateCampusOut>();
    }

    public async Task<GetCampiOut> GetCampi()
    {
        var response = await http.GetAsync("/campi");
        return await response.DeserializeTo<GetCampiOut>();
    }
}
