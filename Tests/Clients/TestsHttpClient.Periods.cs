using System.Net.Http.Json;
using Syki.Back.Features.Periods.GetAcademicPeriods;
using Syki.Back.Features.Periods.CreateAcademicPeriod;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateAcademicPeriodOut, ErrorOut>> CreateAcademicPeriod(
        string name = "2024.1",
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        var data = new CreateAcademicPeriodIn(name);
        if (startAt.HasValue) data.StartAt = startAt.Value;
        if (endAt.HasValue) data.EndAt = endAt.Value;
        var response = await http.PostAsJsonAsync("/periods/academic", data);
        return await response.Resolve<CreateAcademicPeriodOut>();
    }

    public async Task<GetAcademicPeriodsOut> GetAcademicPeriods()
    {
        var response = await http.GetAsync("/periods/academic");
        return await response.DeserializeTo<GetAcademicPeriodsOut>();
    }
}
