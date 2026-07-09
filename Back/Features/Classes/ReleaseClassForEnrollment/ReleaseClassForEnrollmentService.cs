namespace Estud.Back.Features.Classes.ReleaseClassForEnrollment;

public class ReleaseClassForEnrollmentService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Release(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes.FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var today = DateTime.UtcNow.ToDateOnly();
        var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);
        if (!hasCurrentEnrollmentPeriod) return NoCurrentEnrollmentPeriod.I;

        if (@class.Status != ClassStatus.OnPreEnrollment) return ClassMustBeOnPreEnrollment.I;

        @class.Status = ClassStatus.OnEnrollment;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
