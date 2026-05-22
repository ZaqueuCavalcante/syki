namespace Syki.Back.Features.Teachers.GetTeachers;

public class GetTeachersService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetTeachersOut> Get()
    {
        var teachers = await ctx.Teachers.AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Campi)
            .Include(t => t.Disciplines)
            .Where(t => t.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(t => t.Name)
            .ToListAsync();

        return new GetTeachersOut
        {
            Total = teachers.Count,
            Items = teachers.ConvertAll(t => t.ToGetTeachersItemOut()),
        };
    }
}
