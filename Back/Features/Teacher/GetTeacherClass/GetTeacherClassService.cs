namespace Syki.Back.Features.Teacher.GetTeacherClass;

public class GetTeacherClassService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<TeacherClassOut, SykiError>> Get(Guid institutionId, Guid userId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Students)
            .Include(t => t.Notes)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.ToTeacherClassOut();
    }
}
