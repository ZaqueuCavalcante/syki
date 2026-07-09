using Estud.Back.Domain.Courses;

namespace Estud.Back.Features.Courses.UpdateCourse;

public static class UpdateCourseMapper
{
    extension(Course course)
    {
        public UpdateCourseOut ToUpdateCourseOut()
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
