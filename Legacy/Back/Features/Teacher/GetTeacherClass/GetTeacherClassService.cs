namespace Estud.Back.Features.Teacher.GetTeacherClass;

public class GetTeacherClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<TeacherClassOut, EstudError>> Get(Guid institutionId, Guid userId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.ToTeacherClassOut();
    }
}
