namespace Syki.Back.Features.Student.GetStudentCurrentClasses;

public class GetStudentCurrentClassesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<StudentCurrentClassOut>> Get(Guid institutionId, Guid userId)
    {
        var classesIds = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId)
            .Select(x => x.ClassId).ToListAsync();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Where(x => classesIds.Contains(x.Id))
            .ToListAsync();

        return classes.OrderBy(x => x.Discipline.Name).Select(t => t.ToStudentCurrentClassOut()).ToList();
    }
}
