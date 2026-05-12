using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourse;

public static class CreateCourseMapper
{
    extension(Course course)
    {
        public CreateCourseOut ToCreateCourseOut()
        {
            return new()
            {
                Id = course.Id,
                Name = course.Name,
                Type = course.Type,
                Disciplines = course.Disciplines
                    .Select(x => x.ToCreateCourseDisciplineOut())
                    .OrderBy(x => x.Name)
                    .ToList()
            };
        }
    }

    extension(Discipline discipline)
    {
        public CreateCourseDisciplineOut ToCreateCourseDisciplineOut()
        {
            return new()
            {
                Id = discipline.Id,
                Name = discipline.Name,
                Code = discipline.Code,
            };
        }
    }
}
