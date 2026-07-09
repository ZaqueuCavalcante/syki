namespace Estud.Back.Features.Teachers.GetTeacher;

public class GetTeacherService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherOut, EstudError>> Get(int id)
    {
        var teacher = await ctx.Teachers.AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Campi)
            .Include(t => t.Disciplines)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        return new GetTeacherOut
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Email = teacher.User!.Email!,
            Campi = teacher.Campi.OrderBy(c => c.Name).Select(c => new GetTeacherCampusOut { Id = c.Id, Name = c.Name }).ToList(),
            Disciplines = teacher.Disciplines.OrderBy(d => d.Name).Select(d => new GetTeacherDisciplineOut { Id = d.Id, Name = d.Name }).ToList(),
        };
    }
}
