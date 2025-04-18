namespace Syki.Back.Features.Student.GetStudentClass;

public class GetStudentClassService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<StudentClassOut, SykiError>> Get(Guid userId, Guid classId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == userId);
        if (!classOk) return new ClassNotFound();

        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Where(t => t.Id == classId)
            .FirstOrDefaultAsync();

        var activities = await ctx.ClassActivities.Where(x => x.ClassId == classId)
            .Select(x => new ClassActivityDto(x.Id, x.Weight, x.Note))
            .ToListAsync();
        var works = await ctx.ClassActivityWorks.Where(x => x.SykiStudentId == userId)
            .Select(x => new ClassActivityWorkDto(x.ClassActivityId, x.Note))
            .ToListAsync();

        var n1 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N1).ToList(), works);
        var n2 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N2).ToList(), works);
        var n3 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N3).ToList(), works);
        var average = GetAverageNote(n1, n2, n3);

        return @class.ToStudentClassOut(n1, n2, n3, average);
    }

    private static decimal GetAverage(List<ClassActivityDto> activities, List<ClassActivityWorkDto> works)
    {
        var average = 0.00M;
        foreach (var activity in activities)
        {
            var work = works.First(x => x.ActivityId == activity.Id);
            average += activity.Weight * work.Note;
        }
        average /= 100.00M;
        return average;
    }

    private static decimal GetAverageNote(params decimal[] notes)
    {
        if (notes.Length <= 2) return 0;
        var average = notes.OrderDescending().Take(2).Average();
        return Math.Round(average, 2);
    }
}

internal record ClassActivityDto(Guid Id, int Weight, ClassNoteType Note);
internal record ClassActivityWorkDto(Guid ActivityId, decimal Note);
