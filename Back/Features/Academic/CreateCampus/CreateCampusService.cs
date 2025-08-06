namespace Syki.Back.Features.Academic.CreateCampus;

public class CreateCampusService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<CampusOut, SykiError>> Create(Guid institutionId, CreateCampusIn data)
    {
        var result = Campus.New(institutionId, data.Name, data.State, data.City, data.Capacity);
        if (result.IsError) return result.Error;

        var campus = result.Success;

        await ctx.SaveChangesAsync(campus);

        return campus.ToOut();
    }
}
