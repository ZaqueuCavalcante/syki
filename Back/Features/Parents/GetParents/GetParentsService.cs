namespace Estud.Back.Features.Parents.GetParents;

public class GetParentsService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetParentsOut> Get(GetParentsIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var institutionId = ctx.RequestUser.InstitutionId;

        var parentsQuery = ctx.Parents.AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.InstitutionId == institutionId);

        var filter = query.Filter;
        if (filter.HasValue())
            parentsQuery = parentsQuery.Where(p =>
                p.Name.ToLower().Contains(filter.ToLower()) ||
                p.User!.Email!.ToLower().Contains(filter.ToLower()));

        var total = await parentsQuery.CountAsync();

        var parents = await parentsQuery
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var parentIds = parents.Select(p => p.Id).ToHashSet();

        var links = await ctx.ParentStudents.AsNoTracking()
            .Include(x => x.Student)
            .Where(x => parentIds.Contains(x.ParentId))
            .OrderBy(x => x.Student!.Name)
            .ToListAsync();

        var result = parents.ConvertAll(p => p.ToGetParentsItemOut());
        foreach (var item in result)
        {
            item.Students = links.Where(l => l.ParentId == item.Id).Select(l => l.Student!.Name).ToList();
        }

        return new GetParentsOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = result,
        };
    }
}
