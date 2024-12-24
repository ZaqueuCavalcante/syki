namespace Syki.Back.Features.Cross.CreateInstitution;

[DomainEventDescription("Instituição criada")]
public record InstitutionCreatedDomainEvent(Guid Id) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, Guid institutionId, InstitutionCreatedDomainEvent evt)
    {
        await ctx.SaveTasksAsync(eventId, institutionId, new SeedInstitutionDataTask(evt.Id));
    }
}
