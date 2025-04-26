namespace Syki.Back.Features.Student.GetStudentClassFrequency;

public class GetStudentClassFrequencyService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<GetStudentClassFrequencyOut, SykiError>> Get(Guid studentId, Guid classId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == studentId);
        if (!classOk) return new ClassNotFound();

        var lessons = await ctx.Lessons.AsNoTracking()
            .Where(x => x.ClassId == classId)
            .OrderBy(x => x.Number)
            .ToListAsync();

        var attendances = await ctx.Attendances.AsNoTracking().Where(x => x.ClassId == classId && x.StudentId == studentId).ToListAsync();
        var presences = attendances.Count(x => x.Present);
        var absences = attendances.Count - presences;

        var lessonsOut = new List<GetStudentClassLessonFrequencyOut>();
        foreach (var lesson in lessons)
        {
            var attendance = attendances.FirstOrDefault(x => x.LessonId == lesson.Id);
            lessonsOut.Add(new()
            {
                Lesson = lesson.Number,
                Frequency = attendance == null ? 0.50M : attendance.Present ? 1 : 0,
                LessonDate = $"{lesson.Date} {lesson.StartAt.GetDescription()}-{lesson.EndAt.GetDescription()}"
            });
        }

        if (attendances.Count == 0) return new GetStudentClassFrequencyOut { TotalLessons = lessons.Count, Lessons = lessonsOut };

        var frequency = Math.Round(100M*(1M * presences / (1M * attendances.Count)), 2);

        return new GetStudentClassFrequencyOut
        {
            Frequency = frequency,
            Presences = presences,
            Attendances = attendances.Count,
            Absences = absences,
            TotalLessons = lessons.Count,
            Lessons = lessonsOut,
        };
    }
}
