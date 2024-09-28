namespace Syki.Back.Features.Student.GetStudentFrequency;

public class GetStudentFrequencyService(SykiDbContext ctx) : IStudentService
{
    public async Task<decimal> Get(Guid userId)
    {
        var attendances = await ctx.Attendances.Where(x => x.StudentId == userId).CountAsync();
        var presences = await ctx.Attendances.Where(x => x.StudentId == userId && x.Present).CountAsync();

        if (attendances == 0) return 0.00M;
        return 100M*(1M * presences / (1M * attendances));
    }
}
