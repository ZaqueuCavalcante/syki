namespace Estud.Back.Features.Teachers.GetTeachers;

public class GetTeachersService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetTeachersOut> Get(GetTeachersIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var institutionId = ctx.RequestUser.InstitutionId;

        var teachersQuery = ctx.Teachers.AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Campi)
            .Include(t => t.Disciplines)
            .Where(t => t.InstitutionId == institutionId);

        var filter = query.Filter;
        if (filter.HasValue())
            teachersQuery = teachersQuery.Where(t =>
                t.Name.ToLower().Contains(filter.ToLower()) ||
                t.User!.Email!.ToLower().Contains(filter.ToLower()));

        var total = await teachersQuery.CountAsync();

        var teachers = await teachersQuery
            .OrderBy(t => t.Name)
            .ThenBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new GetTeachersOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = teachers.ConvertAll(t => t.ToGetTeachersItemOut()),
        };
    }
}
