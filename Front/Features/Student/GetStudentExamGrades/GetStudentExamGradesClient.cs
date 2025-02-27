namespace Syki.Front.Features.Student.GetStudentExamGrades;

public class GetStudentExamGradesClient(HttpClient http) : IStudentClient
{
    public async Task<List<StudentExamGradeOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentExamGradeOut>>("/student/exam-grades", HttpConfigs.JsonOptions) ?? [];
    }
}
