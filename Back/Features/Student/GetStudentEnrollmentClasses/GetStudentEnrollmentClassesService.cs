namespace Syki.Back.Features.Student.GetStudentEnrollmentClasses;

public class GetStudentEnrollmentClassesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<EnrollmentClassOut>> Get(Guid institutionId, Guid userId, Guid courseCurriculumId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var enrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.StartAt <= today && p.EndAt >= today)
            .FirstOrDefaultAsync();

        if (enrollmentPeriod == null)
            return [];

        var courseCurriculum = await ctx.CourseCurriculums.Where(g => g.Id == courseCurriculumId).AsNoTracking()
            .Include(g => g.Links)
            .FirstAsync();
        var ids = courseCurriculum.Links.ConvertAll(d => d.DisciplineId);

        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Include(t => t.Teacher)
            .Where(t => t.InstitutionId == institutionId && t.PeriodId == enrollmentPeriod.Id && t.Status == ClassStatus.OnEnrollment && ids.Contains(t.DisciplineId))
            .ToListAsync();

        var selecteds = await ctx.ClassesStudents.Where(x => x.SykiStudentId == userId).Select(x => x.ClassId).ToListAsync();

        var response = classes.ConvertAll(t =>
        {
            var link = courseCurriculum.Links.First(v => v.DisciplineId == t.DisciplineId);
            return new EnrollmentClassOut
            {
                Id = t.Id,
                Discipline = t.Discipline.Name,
                Period = link.Period,
                Credits = link.Credits,
                Workload = link.Workload,
                Teacher = t.Teacher.Name,
                Schedules = t.Schedules.ConvertAll(h => h.ToOut()),
                IsSelected = selecteds.Contains(t.Id),
            };
        });

        return response.OrderBy(t => t.Period).ToList();
    }
}
