namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumService(SykiDbContext ctx)
{
    public async Task<CourseCurriculumOut> Create(Guid institutionId, CreateCourseCurriculumIn data)
    {
        var courseOk = await ctx.Courses
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CourseId);

        if (!courseOk)
            Throw.DE002.Now();

        var courseCurriculum = new CourseCurriculum(
            institutionId,
            data.CourseId,
            data.Name
        );

        var disciplines = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(x => x.CourseId == data.CourseId)
            .Select(x => x.DisciplineId)
            .ToListAsync();

        if (!data.Disciplines.ConvertAll(d => d.Id).IsSubsetOf(disciplines))
            Throw.DE003.Now();

        data.Disciplines.ForEach(d => courseCurriculum.Links.Add(
            new CourseCurriculumDiscipline(d.Id, d.Period, d.Credits, d.Workload)));

        ctx.CourseCurriculums.Add(courseCurriculum);
        await ctx.SaveChangesAsync();

        courseCurriculum = await ctx.CourseCurriculums.AsNoTracking()
            .Include(g => g.Course)
            .Include(x => x.Disciplines)
            .Include(g => g.Links)
            .FirstAsync(x => x.Id == courseCurriculum.Id);

        return courseCurriculum.ToOut();
    }
}
