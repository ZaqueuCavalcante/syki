namespace Syki.Front.Features.Academic.CreateStudent;

public class CreateStudentClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, string email, Guid courseOfferingId)
    {
        var data = new CreateStudentIn
        {
            Name = name,
            Email = email,
            CourseOfferingId = courseOfferingId,
        };
        return await http.PostAsJsonAsync("/academic/students", data);
    }
}
