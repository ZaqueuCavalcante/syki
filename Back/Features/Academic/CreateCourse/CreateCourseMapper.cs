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
                    .Select(x => new CreateCourseDisciplineOut() { Id = x.Id, Name = x.Name, Code = x.Code })
                    .OrderBy(x => x.Name).ToList()
            };
        }
    }
}
