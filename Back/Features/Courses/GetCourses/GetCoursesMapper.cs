using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.GetCourses;

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
                Type = course.CourseType,
            };
        }
    }
}
