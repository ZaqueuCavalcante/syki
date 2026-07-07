namespace Syki.Back.Features.Classrooms.GetClassrooms;

public class GetClassroomsService(SykiDbContext ctx) : ISykiService
{
    public async Task<List<GetClassroomsOut>> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var classrooms = await ctx.Classrooms
            .Include(x => x.Campus)
            .Where(x => x.InstitutionId == institutionId).ToListAsync();

        return classrooms.ConvertAll(c => new GetClassroomsOut
        {
            Id = c.Id,
            Name = c.Name,
            CampusId = c.CampusId,
            Capacity = c.Capacity,
            Campus = c.Campus.Name,
        });
    }
}
