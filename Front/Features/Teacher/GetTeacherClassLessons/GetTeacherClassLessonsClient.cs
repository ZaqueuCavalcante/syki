namespace Syki.Front.Features.Teacher.GetTeacherClassLessons;

public class GetTeacherClassLessonsClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<List<ClassLessonOut>, ErrorOut>> Get(Guid classId)
    {
        var response = await http.GetAsync($"teacher/classes/{classId}/lessons");

        return await response.Resolve<List<ClassLessonOut>>();
    }
}
