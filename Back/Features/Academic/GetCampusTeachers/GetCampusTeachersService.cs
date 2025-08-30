namespace Syki.Back.Features.Academic.GetCampusTeachers;

public class GetCampusTeachersService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetCampusTeachersOut>> Get(Guid campusId)
    {
        var links = await ctx.TeachersCampi.AsNoTracking()
            .Where(x => x.CampusId == campusId).ToListAsync();

        var ids = links.ConvertAll(x => x.SykiTeacherId);
        var teachers = await ctx.Teachers.AsNoTracking()
            .Where(x => x.InstitutionId == ctx.InstitutionId && ids.Contains(x.Id))
            .Include(x => x.Disciplines)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return teachers.ConvertAll(x => new GetCampusTeachersOut
        {
            Id = x.Id,
            Name = x.Name,
            Disciplines = x.Disciplines.ConvertAll(d => d.Id),
        });
    }
}
