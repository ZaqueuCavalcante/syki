namespace Estud.Back.Features.Classes.StartClass;

public class StartClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Start(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes
            .Include(c => c.Period)
            .Include(c => c.Teachers)
            .Include(c => c.Schedules)
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.Status != ClassStatus.OnEnrollment) return ClassMustBeOnEnrollment.I;

        var today = DateTime.UtcNow.ToDateOnly();
        var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);
        if (hasCurrentEnrollmentPeriod) return EnrollmentPeriodMustBeFinalized.I;

        // Checkpoint de montagem: a turma só inicia com o conjunto completo (professores + horários),
        // pois as aulas derivam dos horários e ficam congeladas a partir daqui.
        if (@class.Teachers.Count == 0) return ClassWithoutTeachers.I;
        if (@class.Schedules.Count == 0) return ClassWithoutSchedules.I;

        @class.CreateLessons();
        @class.Status = ClassStatus.Started;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
