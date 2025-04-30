namespace Syki.Back.Features.Student.GetStudentNotes;

public class GetStudentNotesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<StudentNoteOut>> Get(Guid userId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId).ToListAsync();

        var classesIds = classesStudents.ConvertAll(x => x.ClassId);
        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Select(x => new { x.Id, x.DisciplineId, Discipline = x.Discipline.Name })
            .Where(x => classesIds.Contains(x.Id))
            .ToListAsync();

        var activities = await ctx.ClassActivities
            .AsNoTrackingWithIdentityResolution()
            .Where(x => classesIds.Contains(x.ClassId))
            .ToListAsync();

        var works = await ctx.ClassActivityWorks
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.SykiStudentId == userId)
            .ToListAsync();

        var disciplinesIds = classes.ConvertAll(x => x.DisciplineId);
        var periods = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Select(x => new { x.DisciplineId, x.Period })
            .Where(x => disciplinesIds.Contains(x.DisciplineId))
            .ToListAsync();

        var notes = new List<StudentNoteOut>();
        foreach (var @class in classes)
        {
            var classActivities = activities.Where(x => x.ClassId == @class.Id)
                .Select(x => new ClassActivityDto(x.Id, x.Weight, x.Note))
                .ToList();
            var classWorks = works.Where(x => classActivities.Select(a => a.Id).Contains(x.ClassActivityId))
                .Select(x => new ClassActivityWorkDto(x.ClassActivityId, x.Note))
                .ToList();

            var n1 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N1).ToList(), classWorks);
            var n2 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N2).ToList(), classWorks);
            var n3 = GetAverage(classActivities.Where(x => x.Note == ClassNoteType.N3).ToList(), classWorks);

            var average = GetAverageNote(n1, n2, n3);
            notes.Add(new StudentNoteOut
            {
                ClassId = @class.Id,
                Discipline = @class.Discipline,
                AverageNote = average,
                Notes =
                [
                    new() { Type = ClassNoteType.N1, Note = n1 },
                    new() { Type = ClassNoteType.N2, Note = n2 },
                    new() { Type = ClassNoteType.N3, Note = n3 },
                ],
                Period = periods.First(p => p.DisciplineId == @class.DisciplineId).Period,
                StudentDisciplineStatus = classesStudents.First(s => s.ClassId == @class.Id).StudentDisciplineStatus,
            });
        }

        return notes.OrderBy(x => x.Period).ThenBy(x => x.Discipline).ToList();
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
