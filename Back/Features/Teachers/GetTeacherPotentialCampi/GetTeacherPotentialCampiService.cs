namespace Estud.Back.Features.Teachers.GetTeacherPotentialCampi;

public class GetTeacherPotentialCampiService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherPotentialCampiOut, EstudError>> Get(int id, string? name)
    {
        var teacher = await ctx.Teachers.AsNoTracking().Include(t => t.Campi)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var assignedCampusIds = teacher.Campi.Select(c => c.Id).ToHashSet();

        var query = ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId && !assignedCampusIds.Contains(c.Id));

        if (name.HasValue()) query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));

        var items = await query
            .OrderBy(c => c.Name)
            .Select(c => new GetTeacherPotentialCampusItemOut { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new GetTeacherPotentialCampiOut { Items = items };
    }
}
