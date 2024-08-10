namespace Syki.Front.Features.Academic.CreateCourse;

public class CreateCourseClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CourseOut, ErrorOut>> Create(string name, CourseType type)
    {
        var data = new CreateCourseIn { Name = name, Type = type };

        var response = await http.PostAsJsonAsync("/academic/courses", data);

        return await response.Resolve<CourseOut>();
    }
}
