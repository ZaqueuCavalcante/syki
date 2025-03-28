namespace Syki.Back.Features.Academic.CreateClass;

public class CreateClassService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<ClassOut, SykiError>> Create(Guid institutionId, CreateClassIn data)
    {
        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.DisciplineId);
        if (!disciplineOk) return new DisciplineNotFound();

        var teacherOk = await ctx.Teachers.AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.TeacherId);
        if (!teacherOk) return new TeacherNotFound();

        var periodExists = await ctx.AcademicPeriodExists(institutionId, data.Period);
        if (!periodExists) return new AcademicPeriodNotFound();

        var schedules = data.Schedules.ConvertAll(h => Schedule.New(h.Day, h.Start, h.End));
        foreach (var schedule in schedules)
        {
            if (schedule.IsError()) return schedule.GetError();
        }

        var result = Class.New(
            institutionId,
            data.DisciplineId,
            data.TeacherId,
            data.Period,
            data.Vacancies,
            schedules.ConvertAll(x => x.GetSuccess())
        );

        if (result.IsError()) return result.GetError();

        var @class = result.GetSuccess();

        ctx.Add(@class);
        await ctx.SaveChangesAsync();

        @class = await ctx.Classes
            .Include(c => c.Period)
            .Include(t => t.Lessons)
            .Include(t => t.Schedules)
            .FirstAsync(x => x.Id == @class.Id);

        @class.CreateLessons();
        await ctx.SaveChangesAsync();

        @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .Include(t => t.Lessons)
                .ThenInclude(l => l.Attendances)
            .FirstAsync(x => x.Id == @class.Id);

        return @class.ToOut();
    }

    public async Task CreateWithThrowOnError(Guid institutionId, CreateClassIn data)
    {
        (await Create(institutionId, data)).ThrowOnError();
    }
}
