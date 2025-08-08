namespace Syki.Back.Features.Academic.GetClassrooms;

public class GetClassroomsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetClassroomsOut>> Get(Guid institutionId)
    {
        var classrooms = await ctx.Classrooms
            .Include(x => x.Campus)
            .Where(x => x.InstitutionId == institutionId).ToListAsync();

        return classrooms.ConvertAll(c => c.ToGetClassroomsOut());
    }
}
