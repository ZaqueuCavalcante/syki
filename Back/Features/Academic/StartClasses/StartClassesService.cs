namespace Syki.Back.Features.Academic.StartClasses;

public class StartClassesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Start(Guid institutionId, StartClassesIn data)
    {
        var classes = await ctx.Classes
            .Include(x => x.Students)
            .Where(c => c.InstitutionId == institutionId && data.Classes.Contains(c.Id))
            .ToListAsync();

        if (classes.Count == 0) return new InvalidClassesList();
        
        var statusOk = classes.All(x => x.Status == ClassStatus.OnEnrollment);
        if (!statusOk) return new ClassMustHaveOnEnrollmentStatus();
        
        var today = DateTime.Now.ToDateOnly();
        var periods = await ctx.EnrollmentPeriods.AsNoTracking().Where(x => x.InstitutionId == institutionId).ToListAsync();
        foreach (var @class in classes)
        {
            var period = periods.FirstOrDefault(x => x.Id == @class.PeriodId);
            if (period == null) return new EnrollmentPeriodNotFound();
            if (today <= period.EndAt) return new EnrollmentPeriodMustBeFinalized();
        }

        classes.ForEach(c => c.Start());

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
