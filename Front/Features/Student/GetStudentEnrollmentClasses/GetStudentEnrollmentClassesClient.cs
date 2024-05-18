namespace Syki.Front.Features.Student.GetStudentEnrollmentClasses;

public class GetStudentEnrollmentClassesClient(HttpClient http)
{
    public async Task<List<EnrollmentClassOut>> Get()
    {
        return await http.GetFromJsonAsync<List<EnrollmentClassOut>>("/student/enrollment-classes") ?? [];
    }
}
