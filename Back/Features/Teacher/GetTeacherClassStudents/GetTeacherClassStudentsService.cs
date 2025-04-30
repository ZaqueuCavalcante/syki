namespace Syki.Back.Features.Teacher.GetTeacherClassStudents;

public class GetTeacherClassStudentsService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<TeacherClassStudentOut>, SykiError>> Get(Guid teacherId, Guid classId)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        var students = @class.Students.OrderBy(x => x.Name)
            .Select(x => new TeacherClassStudentOut { Id = x.Id, Name = x.Name })
            .ToList();
        var classAttendances = await ctx.Attendances.AsNoTracking().Where(x => x.ClassId == classId).ToListAsync();

        var classActivities = await ctx.ClassActivities
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.ClassId == classId)
            .Include(x => x.Works)
            .ToListAsync();
        var activities = classActivities
            .Select(x => new ClassActivityDto(x.Id, x.Weight, x.Note))
            .ToList();

        foreach (var student in students)
        {
            var attendances = classAttendances.Count(x => x.StudentId == student.Id);
            var presences = classAttendances.Count(x => x.StudentId == student.Id && x.Present);
            student.Frequency = attendances > 0 ? Math.Round(100M*(1M * presences / (1M * attendances)), 2) : 0;
  
            var works = classActivities.SelectMany(x => x.Works.Where(w => w.SykiStudentId == student.Id))
                .Select(x => new ClassActivityWorkDto(x.ClassActivityId, x.Note))
                .ToList();

            var n1 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N1).ToList(), works);
            var n2 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N2).ToList(), works);
            var n3 = GetAverage(activities.Where(x => x.Note == ClassNoteType.N3).ToList(), works);

            student.Notes.Add(new() { Type = ClassNoteType.N1, Note = n1 });
            student.Notes.Add(new() { Type = ClassNoteType.N2, Note = n2 });
            student.Notes.Add(new() { Type = ClassNoteType.N3, Note = n3 });
            student.AverageNote = GetAverageNote(n1, n2, n3);
        }

        return students;
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
