namespace Estud.Back.Features.Disciplines.GetDisciplineTeachers;

public class GetDisciplineTeachersService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetDisciplineTeachersOut, EstudError>> Get(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var disciplineOk = await ctx.Disciplines.AsNoTracking()
            .AnyAsync(d => d.Id == id && d.InstitutionId == institutionId);
        if (!disciplineOk) return DisciplineNotFound.I;

        var items = await ctx.Teachers.AsNoTracking()
            .Where(t => t.InstitutionId == institutionId && t.Disciplines.Any(d => d.Id == id))
            .OrderBy(t => t.Name)
            .Select(t => new GetDisciplineTeacherItemOut { Id = t.Id, Name = t.Name })
            .ToListAsync();

        return new GetDisciplineTeachersOut { Items = items };
    }
}
