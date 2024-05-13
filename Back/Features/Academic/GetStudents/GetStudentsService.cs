namespace Syki.Back.Features.Academic.GetStudents;

public class GetStudentsService(SykiDbContext ctx)
{
    public async Task<List<StudentOut>> Get(Guid institutionId)
    {
        var students = await ctx.Students
            .AsNoTracking().AsSplitQuery()
            .Include(a => a.User)
            .Include(a => a.CourseOffering)
                .ThenInclude(o => o.Course)
            .Where(a => a.InstitutionId == institutionId)
            .ToListAsync();
        
        return students.ConvertAll(p => p.ToOut());
    }
}
