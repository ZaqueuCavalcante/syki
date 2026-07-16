namespace Estud.Back.Features.Parents.GetParentStudents;

public class GetParentStudentsService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetParentStudentsOut> Get()
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;

        var parentId = await ctx.GetParentId(institutionId, userId);

        var links = await ctx.ParentStudents.AsNoTracking()
            .Include(x => x.Student)
            .Where(x => x.ParentId == parentId && x.Status == ParentStudentStatus.Active && !x.RevokedByStudent)
            .OrderBy(x => x.Student!.Name)
            .ToListAsync();

        return new GetParentStudentsOut { Items = links.ConvertAll(x => x.ToGetParentStudentsItemOut()) };
    }
}
