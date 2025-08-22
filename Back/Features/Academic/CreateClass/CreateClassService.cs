namespace Syki.Back.Features.Academic.CreateClass;

public class CreateClassService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<ClassOut, SykiError>> Create(Guid institutionId, CreateClassIn data)
    {
        var campusOk = await ctx.Campi.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return CampusNotFound.I;

        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.DisciplineId);
        if (!disciplineOk) return new DisciplineNotFound();

        var teacherOk = await ctx.Teachers.AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.TeacherId);
        if (!teacherOk) return new TeacherNotFound();

        var teacherCampusOk = await ctx.TeachersCampi.AnyAsync(x => x.SykiTeacherId == data.TeacherId && x.CampusId == data.CampusId);
        if (!teacherCampusOk) return new TeacherNotAssignedToCampus();

        var teacherDisciplineOk = await ctx.TeachersDisciplines.AnyAsync(x => x.SykiTeacherId == data.TeacherId && x.DisciplineId == data.DisciplineId);
        if (!teacherDisciplineOk) return new TeacherNotAssignedToDiscipline();

        if (await ctx.AcademicPeriodNotFound(data.Period)) return AcademicPeriodNotFound.I;

        var schedulesResult = data.Schedules.ToSchedules();
        if (schedulesResult.IsError) return schedulesResult.Error;
        var schedules = schedulesResult.Success;

        var result = Class.New(
            institutionId,
            data.DisciplineId,
            data.CampusId,
            data.TeacherId,
            data.Period,
            data.Vacancies,
            schedules
        );

        if (result.IsError) return result.Error;

        var @class = result.Success;

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
