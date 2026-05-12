using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

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
