namespace Syki.Front.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CourseOfferingOut, ErrorOut>> Create(
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

        var response = await http.PostAsJsonAsync("/academic/course-offerings", data);

        return await response.Resolve<CourseOfferingOut>();
    }
}
