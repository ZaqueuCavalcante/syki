namespace Estud.Back.Features.Teachers.GetTeacherPotentialDisciplines;

public class GetTeacherPotentialDisciplinesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherPotentialDisciplinesOut, EstudError>> Get(int id, string? name)
    {
        var teacher = await ctx.Teachers.AsNoTracking().Include(t => t.Disciplines)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var assignedDisciplineIds = teacher.Disciplines.Select(d => d.Id).ToHashSet();

        var query = ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId && !assignedDisciplineIds.Contains(d.Id));

        if (name.HasValue()) query = query.Where(d => d.Name.ToLower().Contains(name.ToLower()));

        var items = await query
            .OrderBy(d => d.Name)
            .Select(d => new GetTeacherPotentialDisciplineItemOut { Id = d.Id, Name = d.Name })
            .ToListAsync();

        return new GetTeacherPotentialDisciplinesOut { Items = items };
    }
}
