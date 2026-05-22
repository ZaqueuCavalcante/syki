using System.Net.Http.Json;
using Syki.Back.Features.Periods.CreateAcademicPeriod;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateAcademicPeriodOut, ErrorOut>> CreateAcademicPeriod(string name = "2024.1")
    {
        var data = new CreateAcademicPeriodIn(name);
        var response = await http.PostAsJsonAsync("/periods/academic", data);
        return await response.Resolve<CreateAcademicPeriodOut>();
    }
}
