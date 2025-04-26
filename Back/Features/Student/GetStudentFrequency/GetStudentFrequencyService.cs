namespace Syki.Back.Features.Student.GetStudentFrequency;

public class GetStudentFrequencyService(SykiDbContext ctx) : IStudentService
{
    public async Task<GetStudentFrequencyOut> Get(Guid userId)
    {
        var result = new GetStudentFrequencyOut();

        result.Attendances = await ctx.Attendances.Where(x => x.StudentId == userId).CountAsync();
        result.Presences = await ctx.Attendances.Where(x => x.StudentId == userId && x.Present).CountAsync();
        result.Absences = result.Attendances - result.Presences;

        var classesIds = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId)
            .Select(x => x.ClassId).ToListAsync();
        result.TotalLessons = await ctx.Lessons.AsNoTracking()
            .Where(x => classesIds.Contains(x.ClassId))
            .CountAsync();

        if (result.Attendances == 0) return result;

        result.Frequency = Math.Round(100M*(1M * result.Presences / (1M * result.Attendances)), 2);

        return result;
    }
}
