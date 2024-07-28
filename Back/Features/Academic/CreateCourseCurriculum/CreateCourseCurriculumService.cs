namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumService(SykiDbContext ctx)
{
    public async Task<OneOf<CourseCurriculumOut, SykiError>> Create(Guid institutionId, CreateCourseCurriculumIn data)
    {
        var courseOk = await ctx.Courses
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CourseId);
        if (!courseOk) return new CourseNotFound();

        var disciplines = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(x => x.CourseId == data.CourseId)
            .Select(x => x.DisciplineId)
            .ToListAsync();

        if (!data.Disciplines.ConvertAll(d => d.Id).IsSubsetOf(disciplines))
            return new InvalidDisciplinesList();

        var courseCurriculum = new CourseCurriculum(institutionId, data.CourseId, data.Name);

        data.Disciplines.ForEach(d =>
            courseCurriculum.Links.Add(new(d.Id, d.Period, d.Credits, d.Workload))
        );

        ctx.Add(courseCurriculum);
        await ctx.SaveChangesAsync();

        courseCurriculum = await ctx.CourseCurriculums.AsNoTracking()
            .Include(g => g.Course)
            .Include(x => x.Disciplines)
            .Include(g => g.Links)
            .FirstAsync(x => x.Id == courseCurriculum.Id);

        return courseCurriculum.ToOut();
    }
}
