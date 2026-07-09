using Estud.Back.Domain.CourseOfferings;

namespace Estud.Back.Features.CourseOfferings.GetCourseOfferings;

public static class GetCourseOfferingsMapper
{
    extension(CourseOffering offering)
    {
        public GetCourseOfferingsItemOut ToGetCourseOfferingsItemOut()
        {
            return new()
            {
                Id = offering.Id,
                Campus = offering.Campus!.Name,
                Course = offering.Course!.Name,
                CourseCurriculum = offering.CourseCurriculum!.Name,
                Period = offering.AcademicPeriod!.Name,
                Session = offering.Session,
            };
        }
    }
}
