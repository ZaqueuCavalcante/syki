namespace Syki.Back.Features.Student.GetStudentClassFrequency;

public class GetStudentClassFrequencyService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<GetStudentClassFrequencyOut, SykiError>> Get(Guid studentId, Guid classId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == studentId);
        if (!classOk) return new ClassNotFound();

        var attendances = await ctx.Attendances.Where(x => x.ClassId == classId && x.StudentId == studentId).CountAsync();
        var presences = await ctx.Attendances.Where(x => x.ClassId == classId && x.StudentId == studentId && x.Present).CountAsync();

        if (attendances == 0) return new GetStudentClassFrequencyOut();

        var frequency = Math.Round(100M*(1M * presences / (1M * attendances)), 2);

        return new GetStudentClassFrequencyOut() { Frequency = frequency, Presences = presences, Attendances = attendances };
    }
}
