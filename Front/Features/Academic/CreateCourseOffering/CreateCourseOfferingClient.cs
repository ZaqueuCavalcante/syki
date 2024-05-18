namespace Syki.Front.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var data = new CreateCourseOfferingIn
        {
            CampusId = campusId,
            CourseId = courseId,
            CourseCurriculumId = courseCurriculumId,
            Period = period,
            Shift = shift,
        };

        return await http.PostAsJsonAsync("/academic/course-offerings", data);
    }
}
