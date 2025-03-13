namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class CreateStudentEnrollmentService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid institutionId, Guid userId, Guid courseCurriculumId, CreateStudentEnrollmentIn data)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var enrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.StartAt <= today && p.EndAt >= today)
            .FirstOrDefaultAsync();
        if (enrollmentPeriod == null) return new EnrollmentPeriodNotFound();

        var courseCurriculum = await ctx.CourseCurriculums.Where(g => g.Id == courseCurriculumId).AsNoTracking()
            .Include(g => g.Links)
            .FirstAsync();
        var disciplines = courseCurriculum.Links.ConvertAll(d => d.DisciplineId);
        var classes = await ctx.Classes.AsNoTracking()
            .Where(t => t.InstitutionId == institutionId && t.PeriodId == enrollmentPeriod.Id && t.Status == ClassStatus.OnEnrollment && disciplines.Contains(t.DisciplineId))
            .ToListAsync();
        var ids = classes.Where(t => data.Classes.Contains(t.Id)).Select(t => t.Id).ToList();

        var classesStudents = await ctx.ClassesStudents.Where(x => x.SykiStudentId == userId && x.StudentDisciplineStatus == StudentDisciplineStatus.Matriculado).ToListAsync();
        ctx.RemoveRange(classesStudents);
        ids.ForEach(id => ctx.Add(new ClassStudent(id, userId)));

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }

    public async Task CreateWithThrowOnError(Guid institutionId, Guid userId, Guid courseCurriculumId, CreateStudentEnrollmentIn data)
    {
        (await Create(institutionId, userId, courseCurriculumId, data)).ThrowOnError();
    }
}
