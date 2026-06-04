namespace Syki.Back.Features.CourseCurriculums.GetCourseCurriculum;

public class GetCourseCurriculumService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetCourseCurriculumOut, SykiError>> Get(int id)
    {
        var curriculum = await ctx.CourseCurriculums.AsNoTracking()
            .Include(c => c.Course)
            .Include(c => c.Links)
            .FirstOrDefaultAsync(c => c.InstitutionId == ctx.RequestUser.InstitutionId && c.Id == id);
        if (curriculum == null) return CourseCurriculumNotFound.I;

        var disciplineIds = curriculum.Links.Select(l => l.DisciplineId).ToList();

        var disciplines = await ctx.Disciplines.AsNoTracking()
            .Where(d => disciplineIds.Contains(d.Id))
            .Select(d => new { d.Id, d.Name })
            .ToListAsync();

        var disciplineMap = disciplines.ToDictionary(d => d.Id, d => d.Name);

        var disciplineItems = curriculum.Links
            .OrderBy(l => l.Period)
            .ThenBy(l => disciplineMap.GetValueOrDefault(l.DisciplineId, string.Empty))
            .Select(l => new GetCourseCurriculumDisciplineOut
            {
                Id = l.DisciplineId,
                Name = disciplineMap.GetValueOrDefault(l.DisciplineId, string.Empty),
                Period = l.Period,
                Credits = l.Credits,
                Workload = l.Workload,
            })
            .ToList();

        return new GetCourseCurriculumOut
        {
            Id = curriculum.Id,
            Name = curriculum.Name,
            CourseId = curriculum.CourseId,
            Course = curriculum.Course!.Name,
            Disciplines = disciplineItems,
        };
    }
}
