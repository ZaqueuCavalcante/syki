namespace Syki.Back.Features.Academic.GetAcademicClass;

public class GetAcademicClassService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<GetAcademicClassOut, SykiError>> Get(Guid institutionId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .Include(t => t.Lessons)
                .ThenInclude(l => l.Attendances)
            .Include(t => t.Students)
            .Include(t => t.Notes)
            .Where(c => c.InstitutionId == institutionId && c.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        var count = await ctx.ClassesStudents.Where(x => x.ClassId == id).CountAsync();
        @class.SetFillRatio(count);

        var period = await ctx.EnrollmentPeriods.AsNoTracking().Where(x => x.InstitutionId == institutionId && x.Id == @class.PeriodId).FirstOrDefaultAsync();
        if (@class.Status == ClassStatus.OnEnrollment && period?.EndAt < DateTime.UtcNow.ToDateOnly())
        {
            @class.Status = ClassStatus.AwaitingStart;
        }

        return @class.ToGetAcademicClassOut();
    }
}
