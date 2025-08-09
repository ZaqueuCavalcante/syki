namespace Syki.Back.Features.Academic.AssignCampiToTeacher;

public class AssignCampiToTeacherService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Assign(Guid institutionId, Guid teacherId, AssignCampiToTeacherIn data)
    {
        var teacher = await ctx.Teachers.Include(x => x.Campi).FirstOrDefaultAsync(p => p.InstitutionId == institutionId && p.Id == teacherId);
        if (teacher == null) return new TeacherNotFound();

        var campi = await ctx.Campi
            .Where(x => data.Campi.Contains(x.Id))
            .ToListAsync();

        if (campi.Count != data.Campi.Count)
            return new InvalidCampusList();

        teacher.Campi = campi;

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
