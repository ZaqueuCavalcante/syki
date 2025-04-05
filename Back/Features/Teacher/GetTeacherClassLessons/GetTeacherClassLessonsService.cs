namespace Syki.Back.Features.Teacher.GetTeacherClassLessons;

public class GetTeacherClassLessonsService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<LessonOut>, SykiError>> Get(Guid teacherId, Guid classId)
    {
        var classOk = await ctx.Classes.AnyAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var lessons = await ctx.Lessons.AsNoTracking()
            .Include(x => x.Attendances)
            .Where(t => t.ClassId == classId)
            .OrderByDescending(x => x.Number)
            .ToListAsync();

        return lessons.ConvertAll(x => x.ToOut());
    }
}
