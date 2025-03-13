namespace Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

public class ReleaseClassesForEnrollmentService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Release(Guid institutionId, ReleaseClassesForEnrollmentIn data)
    {
        var classes = await ctx.Classes
            .Where(c => c.InstitutionId == institutionId && data.Classes.Contains(c.Id))
            .ToListAsync();

        var today = DateTime.Now.ToDateOnly();
        var periods = await ctx.EnrollmentPeriods.AsNoTracking().Where(x => x.InstitutionId == institutionId).ToListAsync();
        foreach (var @class in classes)
        {
            var period = periods.FirstOrDefault(x => x.Id == @class.PeriodId);
            if (period == null) return new EnrollmentPeriodNotFound();
            if (today < period.StartAt) return new EnrollmentPeriodNotStarted();
            if (today > period.EndAt) return new EnrollmentPeriodFinalized();
        }

        var statusOk = classes.All(x => x.Status == ClassStatus.OnPreEnrollment);
        if (!statusOk) return new AllClassesMustHaveOnPreEnrollmentStatus();

        classes.ForEach(c => c.Status = ClassStatus.OnEnrollment);

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }

    public async Task ReleaseWithThrowOnError(Guid institutionId, ReleaseClassesForEnrollmentIn data)
    {
        (await Release(institutionId, data)).ThrowOnError();
    }
}
