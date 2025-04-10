namespace Syki.Front.Features.Student.GetStudentClass;

public class GetStudentClassClient(HttpClient http) : IStudentClient
{
    public async Task<StudentClassOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<StudentClassOut>($"/student/classes/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
