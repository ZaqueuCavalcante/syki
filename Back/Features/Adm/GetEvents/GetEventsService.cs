namespace Syki.Back.Features.Adm.GetEvents;

public class GetEventsService(SykiDbContext ctx) : IAdmService
{
    public async Task<List<EventOut>> Get()
    {
        var events = await ctx.DomainEvents
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return events.ConvertAll(e => e.ToOut());
    }
}
