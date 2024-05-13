namespace Syki.Front.Features.Academic.CreateCourse;

public class CreateCourseClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, CourseType type)
    {
        var data = new CreateCourseIn { Name = name, Type = type };
        return await http.PostAsJsonAsync("/courses", data);
    }
}
