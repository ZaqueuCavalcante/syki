using System.Net.Http.Json;
using Estud.Back.Features.Periods.GetAcademicPeriods;
using Estud.Back.Features.Periods.CreateAcademicPeriod;
using Estud.Back.Features.Periods.GetEnrollmentPeriods;
using Estud.Back.Features.Periods.CreateEnrollmentPeriod;
using Estud.Back.Features.Periods.UpdateEnrollmentPeriod;

namespace Estud.Tests.Integration.Clients;

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

    public async Task<OneOf<GetAcademicPeriodsOut, ErrorOut>> GetAcademicPeriods()
    {
        var response = await http.GetAsync("/periods/academic");
        return await response.Resolve<GetAcademicPeriodsOut>();
    }

    public async Task<OneOf<CreateEnrollmentPeriodOut, ErrorOut>> CreateEnrollmentPeriod(
        string name = "Matrículas 2024.1",
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        var data = new CreateEnrollmentPeriodIn
        {
            Name = name,
            StartAt = startAt ?? new DateOnly(2024, 01, 15),
            EndAt = endAt ?? new DateOnly(2024, 02, 01),
        };
        var response = await http.PostAsJsonAsync("/periods/enrollment", data);
        return await response.Resolve<CreateEnrollmentPeriodOut>();
    }

    public async Task<OneOf<UpdateEnrollmentPeriodOut, ErrorOut>> UpdateEnrollmentPeriod(
        int id,
        string name = "Matrículas 2024.1",
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        var data = new UpdateEnrollmentPeriodIn
        {
            Id = id,
            Name = name,
            StartAt = startAt ?? new DateOnly(2024, 01, 15),
            EndAt = endAt ?? new DateOnly(2024, 02, 01),
        };
        var response = await http.PutAsJsonAsync("/periods/enrollment", data);
        return await response.Resolve<UpdateEnrollmentPeriodOut>();
    }

    public async Task<OneOf<GetEnrollmentPeriodsOut, ErrorOut>> GetEnrollmentPeriods()
    {
        var response = await http.GetAsync("/periods/enrollment");
        return await response.Resolve<GetEnrollmentPeriodsOut>();
    }
}
