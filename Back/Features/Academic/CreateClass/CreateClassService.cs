namespace Syki.Back.Features.Academic.CreateClass;

public class CreateClassService(SykiDbContext ctx)
{
    public async Task<OneOf<ClassOut, SykiError>> Create(Guid institutionId, CreateClassIn data)
    {
        var disciplineOk = await ctx.Disciplines
            .AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.DisciplineId);
        if (!disciplineOk) return new DisciplineNotFound();

        var teacherOk = await ctx.Teachers
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.TeacherId);
        if (!teacherOk) return new TeacherNotFound();

        var periodOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Period);
        if (!periodOk) return new AcademicPeriodNotFound();

        var schedules = data.Schedules.ConvertAll(h => new Schedule(h.Day, h.Start, h.End));
        var @class = new Class(
            institutionId,
            data.DisciplineId,
            data.TeacherId,
            data.Period,
            data.Vacancies,
            schedules
        );

        ctx.Classes.Add(@class);
        await ctx.SaveChangesAsync();

        @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .FirstAsync(x => x.Id == @class.Id);

        return @class.ToOut();
    }
}
