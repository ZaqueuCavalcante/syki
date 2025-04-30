namespace Syki.Back.Features.Student.GetStudentAverageNote;

public class GetStudentAverageNoteService(SykiDbContext ctx) : IStudentService
{
    public async Task<decimal> Get(Guid userId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId)
            .ToListAsync();

        var classesIds = classesStudents.ConvertAll(x => x.ClassId);
        var activities = await ctx.ClassActivities
            .AsNoTrackingWithIdentityResolution()
            .Where(x => classesIds.Contains(x.ClassId))
            .ToListAsync();

        var works = await ctx.ClassActivityWorks
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.SykiStudentId == userId)
            .ToListAsync();

        var notes = new List<ClassAverageDto>();
        foreach (var classId in classesIds)
        {
            var classActivities = activities.Where(x => x.ClassId == classId)
                .Select(x => new ClassActivityDto(x.Id, x.Weight, x.Note))
                .ToList();
            var classWorks = works.Where(x => classActivities.Select(a => a.Id).Contains(x.ClassActivityId))
                .Select(x => new ClassActivityWorkDto(x.ClassActivityId, x.Note))
                .ToList();

            var n1 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N1).ToList(), classWorks);
            var n2 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N2).ToList(), classWorks);
            var n3 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N3).ToList(), classWorks);

            var average = GetAverageNote(n1, n2, n3);
            notes.Add(new ClassAverageDto(classId, average));
        }

        var values = notes.ConvertAll(x => x.Average);

        if (values.Count == 0) return 0;

        return Math.Round(values.Average(), 2);
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
internal record ClassAverageDto(Guid ClassId, decimal Average);
