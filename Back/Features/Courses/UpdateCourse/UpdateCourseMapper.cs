using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.UpdateCourse;

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
