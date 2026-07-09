namespace Estud.Back.Features.Teachers.AssignCampiToTeacher;

public class AssignCampiToTeacherService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Assign(int id, AssignCampiToTeacherIn data)
    {
        var teacher = await ctx.Teachers.Include(t => t.Campi)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var campi = await ctx.Campi
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId && data.Campi.Contains(c.Id))
            .ToListAsync();

        if (campi.Count != data.Campi.Count) return InvalidCampusList.I;

        teacher.Campi = campi;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
