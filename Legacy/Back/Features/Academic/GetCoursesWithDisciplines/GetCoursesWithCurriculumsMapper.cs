using Estud.Back.Features.Academic.CreateCourse;

namespace Estud.Back.Features.Academic.GetCoursesWithDisciplines;

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
