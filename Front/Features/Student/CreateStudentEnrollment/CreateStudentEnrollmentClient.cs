namespace Syki.Front.Features.Student.CreateStudentEnrollment;

public class CreateStudentEnrollmentClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(List<Guid> classes)
    {
        var data = new CreateStudentEnrollmentIn { Classes = classes };
        return await http.PostAsJsonAsync("/student/enrollments", data);
    }
}
