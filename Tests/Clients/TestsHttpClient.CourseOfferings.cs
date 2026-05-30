using System.Net.Http.Json;
using Syki.Back.Features.CourseOfferings.GetCourseOfferings;
using Syki.Back.Features.CourseOfferings.CreateCourseOffering;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateCourseOfferingOut, ErrorOut>> CreateCourseOffering(
        int campusId,
        int courseId,
        int courseCurriculumId,
        int academicPeriodId,
        CourseSession session = CourseSession.Evening
    ) {
        var data = new CreateCourseOfferingIn
        {
            CampusId = campusId,
            CourseId = courseId,
            CourseCurriculumId = courseCurriculumId,
            AcademicPeriodId = academicPeriodId,
            CourseSession = session,
        };
        var response = await http.PostAsJsonAsync("/course-offerings", data);
        return await response.Resolve<CreateCourseOfferingOut>();
    }

    public async Task<GetCourseOfferingsOut> GetCourseOfferings()
    {
        var response = await http.GetAsync("/course-offerings");
        return await response.DeserializeTo<GetCourseOfferingsOut>();
    }
}
