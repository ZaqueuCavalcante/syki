using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<CourseOut, SykiError>> Create(Guid institutionId, CreateCourseIn data)
    {
        if (!data.Type.IsValid()) return new InvalidCourseType();
        
        var course = new Course(institutionId, data.Name, data.Type);

        data.Disciplines.ForEach(d => course.Disciplines.Add(new Discipline(institutionId, d)));

        ctx.Add(course);
        await ctx.SaveChangesAsync();

        return course.ToOut();
    }
}
