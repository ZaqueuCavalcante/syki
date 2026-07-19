using System.Net.Http.Json;
using Estud.Back.Features.Classes.GetClass;
using Estud.Back.Features.Classes.GetClasses;
using Estud.Back.Features.Classes.CreateClass;
using Estud.Back.Features.Classes.UpdateClassSchedules;
using Estud.Back.Features.Classes.AssignTeachersToClass;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateClassOut, ErrorOut>> CreateClass(
        int disciplineId,
        int periodId,
        int vacancies = 40,
        int? campusId = null
    ) {
        var data = new CreateClassIn
        {
            DisciplineId = disciplineId,
            CampusId = campusId,
            PeriodId = periodId,
            Vacancies = vacancies,
        };
        var response = await http.PostAsJsonAsync("/classes", data);
        return await response.Resolve<CreateClassOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignTeachersToClass(
        int classId,
        List<int> teachers
    ) {
        var data = new AssignTeachersToClassIn { Teachers = teachers };
        var response = await http.PostAsJsonAsync($"/classes/{classId}/teachers", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> UpdateClassSchedules(
        int classId,
        List<(Day Day, Hour Start, Hour End, int? TeacherId)> schedules
    ) {
        var data = new UpdateClassSchedulesIn
        {
            Schedules = schedules.ConvertAll(x => new UpdateClassScheduleIn { Day = x.Day, Start = x.Start, End = x.End, TeacherId = x.TeacherId }),
        };
        var response = await http.PutAsJsonAsync($"/classes/{classId}/schedules", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<GetClassesOut, ErrorOut>> GetClasses(
        ClassStatus? status = null,
        int? page = null,
        int? pageSize = null
    ) {
        var data = new GetClassesIn
        {
            Status = status,
            Page = page ?? 1,
            PageSize = pageSize ?? 10,
        };

        var response = await http.GetAsync("/classes".AddQueryString(data));
        return await response.Resolve<GetClassesOut>();
    }

    public async Task<OneOf<GetClassOut, ErrorOut>> GetClass(int id)
    {
        var response = await http.GetAsync($"/classes/{id}");
        return await response.Resolve<GetClassOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> ReleaseClassForEnrollment(int id)
    {
        var response = await http.PutAsync($"/classes/{id}/release-for-enrollment", null);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> StartClass(int id)
    {
        var response = await http.PutAsync($"/classes/{id}/start", null);
        return await response.Resolve<SuccessOut>();
    }
}
