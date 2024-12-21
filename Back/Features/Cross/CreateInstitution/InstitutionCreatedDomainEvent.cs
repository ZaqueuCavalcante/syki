namespace Syki.Back.Features.Cross.CreateInstitution;

public record InstitutionCreatedDomainEvent(Guid Id) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, InstitutionCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(eventId, new SeedInstitutionDataTask(evt.Id));
    }
}
