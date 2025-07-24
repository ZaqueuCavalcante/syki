namespace Syki.Back.Features.Academic.GetClassrooms;

public class GetClassroomsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetClassroomsOut>> Get(Guid institutionId)
    {
        var classrooms = await ctx.Classrooms.Where(x => x.InstitutionId == institutionId).ToListAsync();
        return classrooms.ConvertAll(c => c.ToGetClassroomsOut());
    }
}
