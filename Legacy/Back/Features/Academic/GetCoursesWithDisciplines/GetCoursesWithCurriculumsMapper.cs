using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

public static class GetCoursesWithDisciplinesMapper
{
    extension(Course course)
    {
        public GetCoursesWithDisciplinesItemOut ToGetCoursesWithDisciplinesItemOut()
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
