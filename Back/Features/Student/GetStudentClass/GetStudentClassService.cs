namespace Syki.Back.Features.Student.GetStudentClass;

public class GetStudentClassService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<StudentClassOut, SykiError>> Get(Guid userId, Guid classId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == userId);
        if (!classOk) return new ClassNotFound();

        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(x => x.Activities)
            .Where(t => t.Id == classId)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.ToStudentClassOut();
    }
}
