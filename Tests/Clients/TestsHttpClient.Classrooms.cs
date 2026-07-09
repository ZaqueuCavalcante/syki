using System.Net.Http.Json;
using Estud.Back.Features.Classrooms.GetClassrooms;
using Estud.Back.Features.Classrooms.CreateClassroom;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateClassroomOut, ErrorOut>> CreateClassroom(
        int campusId,
        string name = "Sala 05",
        int capacity = 40
    ) {
        var data = new CreateClassroomIn { CampusId = campusId, Name = name, Capacity = capacity };
        var response = await http.PostAsJsonAsync("classrooms", data);
        return await response.Resolve<CreateClassroomOut>();
    }

    public async Task<OneOf<List<GetClassroomsOut>, ErrorOut>> GetClassrooms()
    {
        var response = await http.GetAsync("classrooms");
        return await response.Resolve<List<GetClassroomsOut>>();
    }
}
