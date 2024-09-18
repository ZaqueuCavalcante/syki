namespace Syki.Back.Features.Academic.StartClass;

public class StartClassService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Start(Guid institutionId, Guid classId)
    {
        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == classId);
        if (@class == null) return new ClassNotFound();

        if (@class.Status != ClassStatus.OnEnrollment) return new ClassMustHaveOnEnrollmentStatus();

        var today = DateOnly.FromDateTime(DateTime.Now);
        var enrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.Id == @class.PeriodId && p.EndAt < today)
            .FirstOrDefaultAsync();
        if (enrollmentPeriod == null) return new EnrollmentPeriodMustBeFinalized();

        @class.Start();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
