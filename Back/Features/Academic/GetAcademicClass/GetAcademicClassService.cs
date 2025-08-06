using Syki.Back.Features.Teacher.GetTeacherClassStudents;

namespace Syki.Back.Features.Academic.GetAcademicClass;

public class GetAcademicClassService(SykiDbContext ctx, GetTeacherClassStudentsService service) : IAcademicService
{
    public async Task<OneOf<GetAcademicClassOut, SykiError>> Get(Guid institutionId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .Include(t => t.Lessons)
                .ThenInclude(l => l.Attendances)
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

        var result = @class.ToGetAcademicClassOut();

        var classStudents = (await service.Get(@class.TeacherId!.Value, @class.Id)).Success;

        result.Students = classStudents.ConvertAll(x => new AcademicClassStudentOut
        {
            Id = x.Id,
            Name = x.Name,
            Frequency = x.Frequency,
            Notes = x.Notes
        });

        result.Frequency = result.Students.Count > 0 ? result.Students.Average(s => s.Frequency) : 0.00M;

        return result;
    }
}
