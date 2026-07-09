using Estud.Back.Features.Academic.CreateCourse;

namespace Estud.Back.Features.Academic.GetCoursesWithCurriculums;

public static class GetCoursesWithCurriculumsMapper
{
    extension(Course course)
    {
        public GetCoursesWithCurriculumsItemOut ToGetCoursesWithCurriculumsItemOut()
        {
            return new()
            {
                Id = course.Id,
                Name = course.Name,
                Type = course.Type,
            };
        }
    }
}
