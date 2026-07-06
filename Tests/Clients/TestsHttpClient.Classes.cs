using System.Net.Http.Json;
using Syki.Back.Features.Classes.GetClasses;
using Syki.Back.Features.Classes.CreateClass;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateClassOut, ErrorOut>> CreateClass(
        int disciplineId,
        int periodId,
        int? campusId = null,
        int? teacherId = null,
        int vacancies = 40,
        List<CreateClassScheduleIn>? schedules = null
    ) {
        var data = new CreateClassIn
        {
            DisciplineId = disciplineId,
            CampusId = campusId,
            TeacherId = teacherId,
            PeriodId = periodId,
            Vacancies = vacancies,
            Schedules = schedules ?? [new CreateClassScheduleIn { Day = Day.Monday, Start = Hour.H07_00, End = Hour.H09_00 }],
        };
        var response = await http.PostAsJsonAsync("/classes", data);
        return await response.Resolve<CreateClassOut>();
    }

    public async Task<OneOf<GetClassesOut, ErrorOut>> GetClasses(ClassStatus? status = null)
    {
        var url = status == null ? "/classes" : $"/classes?status={status}";
        var response = await http.GetAsync(url);
        return await response.Resolve<GetClassesOut>();
    }
}
