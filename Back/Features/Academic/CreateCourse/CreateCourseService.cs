namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx)
{
    public async Task<OneOf<CourseOut, SykiError>> Create(Guid institutionId, CreateCourseIn data)
    {
        if (!data.Type.IsValid()) return new InvalidCourseType();
        
        var course = new Course(institutionId, data.Name, data.Type);

        ctx.Add(course);
        await ctx.SaveChangesAsync();

        return course.ToOut();
    }
}
