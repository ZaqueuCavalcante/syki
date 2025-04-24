namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<CourseOut, SykiError>> Create(Guid institutionId, CreateCourseIn data)
    {
        if (!data.Type.IsValid()) return new InvalidCourseType();
        
        var course = new Course(institutionId, data.Name, data.Type);

        data.Disciplines.ForEach(d => course.Disciplines.Add(new(institutionId, d)));

        await ctx.SaveChangesAsync(course);

        await cache.RemoveAsync($"courses:{institutionId}");

        return course.ToOut();
    }
}
