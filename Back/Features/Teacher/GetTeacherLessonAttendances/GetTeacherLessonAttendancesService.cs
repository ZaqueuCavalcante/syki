namespace Syki.Back.Features.Teacher.GetTeacherLessonAttendances;

public class GetTeacherLessonAttendancesService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<GetTeacherLessonAttendanceOut>, SykiError>> Get(Guid institutionId, Guid userId, Guid lessonId)
    {
        var lesson = await ctx.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
        if (lesson == null) return new LessonNotFound();

        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Students)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == lesson.ClassId)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        var attendances = await ctx.Attendances.Where(x => x.LessonId == lessonId).ToListAsync();
        return @class.Students.ConvertAll(s =>
        {
            var value = attendances.FirstOrDefault(a => a.StudentId == s.Id);
            if (value != null) return value.ToOut(s.Name);
            return new GetTeacherLessonAttendanceOut() { LessonId = lessonId, StudentId = s.Id, StudentName = s.Name };
        });
    }
}
