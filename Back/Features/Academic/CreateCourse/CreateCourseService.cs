namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx)
{
    public async Task<CourseOut> Create(Guid institutionId, CreateCourseIn data)
    {
        var course = new Course(institutionId, data.Name, data.Type);

        ctx.Add(course);
        await ctx.SaveChangesAsync();

        return course.ToOut();
    }
}
