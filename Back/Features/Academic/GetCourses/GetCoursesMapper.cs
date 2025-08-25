using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Back.Features.Academic.GetCourses;

public static class GetCoursesMapper
{
    extension(Course course)
    {
        public GetCoursesItemOut ToGetCoursesItemOut()
        {
            return new()
            {
                Id = course.Id,
                Name = course.Name,
                Type = course.Type
            };
        }
    }
}
