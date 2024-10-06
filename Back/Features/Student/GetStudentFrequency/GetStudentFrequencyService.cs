namespace Syki.Back.Features.Student.GetStudentFrequency;

public class GetStudentFrequencyService(SykiDbContext ctx) : IStudentService
{
    public async Task<GetStudentFrequencyOut> Get(Guid userId)
    {
        var attendances = await ctx.Attendances.Where(x => x.StudentId == userId).CountAsync();
        var presences = await ctx.Attendances.Where(x => x.StudentId == userId && x.Present).CountAsync();

        if (attendances == 0) return new() { Frequency = 0, Presences = presences, Attendances = attendances };
        var frequency = Math.Round(100M*(1M * presences / (1M * attendances)), 2);

        return new() { Frequency = frequency, Presences = presences, Attendances = attendances };
    }
}
