namespace Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

public class ReleaseClassesForEnrollmentService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Release(Guid institutionId, ReleaseClassesForEnrollmentIn data)
    {
        var enrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(x => x.InstitutionId == institutionId && x.Id == data.PeriodId).FirstOrDefaultAsync();
        if (enrollmentPeriod == null) return new EnrollmentPeriodNotFound();

        var today = DateTime.Now.ToDateOnly();
        if (today < enrollmentPeriod.StartAt) return new EnrollmentPeriodNotStarted();
        if (today > enrollmentPeriod.EndAt) return new EnrollmentPeriodFinalized();

        var classes = await ctx.Classes
            .Where(c => c.InstitutionId == institutionId && data.Classes.Contains(c.Id))
            .ToListAsync();

        var statusOk = classes.All(x => x.Status == ClassStatus.OnPreEnrollment);
        if (!statusOk) return new AllClassesMustHaveOnPreEnrollmentStatus();

        classes.ForEach(c => c.Status = ClassStatus.OnEnrollment);

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
