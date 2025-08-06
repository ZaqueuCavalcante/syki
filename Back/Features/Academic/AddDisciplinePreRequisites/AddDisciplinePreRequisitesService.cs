namespace Syki.Back.Features.Academic.AddDisciplinePreRequisites;

public class AddDisciplinePreRequisitesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid institutionId, Guid courseCurriculumId, AddDisciplinePreRequisitesIn data)
    {
        var courseCurriculum = await ctx.CourseCurriculums
            .Where(x => x.InstitutionId == institutionId && x.Id == courseCurriculumId)
            .Include(x => x.Links)
            .FirstOrDefaultAsync();

        if (courseCurriculum == null) return new CourseCurriculumNotFound();

        var result = courseCurriculum.AddDisciplinePreRequisites(data.DisciplineId, data.PreRequisites);

        if (result.IsError) return result;

        await ctx.SaveChangesAsync();

        return result;
    }
}
