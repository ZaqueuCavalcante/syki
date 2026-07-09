namespace Estud.Back.Features.Cross.GetHomeStats;

public class GetHomeStatsService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetHomeStatsOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var enrolledStudents = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => e.LeftAt == null && ctx.Students.Any(s => s.Id == e.StudentId && s.InstitutionId == institutionId))
            .CountAsync();

        var activeTeachers = await ctx.Teachers.AsNoTracking()
            .Where(t => t.InstitutionId == institutionId)
            .CountAsync();

        var offeredCourses = await ctx.CourseOfferings.AsNoTracking()
            .Where(o => o.InstitutionId == institutionId)
            .CountAsync();

        var registeredDisciplines = await ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == institutionId)
            .CountAsync();

        return new GetHomeStatsOut
        {
            EnrolledStudents = enrolledStudents,
            ActiveTeachers = activeTeachers,
            OfferedCourses = offeredCourses,
            RegisteredDisciplines = registeredDisciplines,
        };
    }
}
