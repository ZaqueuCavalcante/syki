namespace Syki.Front.Features.Academic.CreateClass;

public class CreateClassClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(Guid disciplineId, Guid teacherId, string period, List<ScheduleIn> schedules)
    {
        var data = new CreateClassIn(
            disciplineId,
            teacherId,
            period,
            schedules
        );
        return await http.PostAsJsonAsync("/academic/classes", data);
    }
}
