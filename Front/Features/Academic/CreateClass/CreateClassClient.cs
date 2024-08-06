namespace Syki.Front.Features.Academic.CreateClass;

public class CreateClassClient(HttpClient http)
{
    public async Task<OneOf<ClassOut, ErrorOut>> Create(Guid disciplineId, Guid teacherId, string period, int vacancies, List<ScheduleIn> schedules)
    {
        var data = new CreateClassIn(disciplineId, teacherId, period, vacancies, schedules);

        var response = await http.PostAsJsonAsync("/academic/classes", data);

        return await response.Resolve<ClassOut>();
    }
}
