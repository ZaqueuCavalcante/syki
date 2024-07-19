namespace Syki.Back.Features.Teacher.GetTeacherClass;

public class GetTeacherClassService(SykiDbContext ctx)
{
    public async Task<OneOf<TeacherClassOut, SykiError>> Get(Guid institutionId, Guid userId, string classId)
    {
        _ = Guid.TryParse(classId, out var id);
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.ToTeacherClassOut();
    }
}
