namespace Estud.Back.Features.Campi.GetCampi;

public class GetCampiService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetCampiOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var ids = campi.Select(c => c.Id).ToHashSet();

        var courseOfferings = await ctx.CourseOfferings.AsNoTracking()
            .Where(x => ids.Count == 0 || ids.Contains(x.CampusId))
            .ToListAsync();
        var courseOfferingsIds = courseOfferings.Select(co => co.Id).ToHashSet();

        var students = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(x => x.LeftAt == null && courseOfferingsIds.Contains(x.CourseOfferingId))
            .ToListAsync();

        var teachers = await ctx.TeachersCampi.AsNoTracking()
            .Where(x => ids.Count == 0 || ids.Contains(x.CampusId))
            .ToListAsync();

        var items = campi.ConvertAll(x =>
        {
            var teachersCount = teachers.Count(t => t.CampusId == x.Id);
            var campusCourseOfferings = courseOfferings.Where(co => co.CampusId == x.Id).Select(x => x.Id).ToList();
            var studentsCount = students.Count(s => campusCourseOfferings.Contains(s.CourseOfferingId));
            return x.ToGetCampiItemOut(studentsCount, teachersCount);
        });

        return new GetCampiOut() { Total = items.Count, Items = items };
    }
}
