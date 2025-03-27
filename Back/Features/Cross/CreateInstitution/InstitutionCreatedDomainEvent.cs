using Syki.Back.Features.Cross.SeedInstitutionData;

namespace Syki.Back.Features.Cross.CreateInstitution;

[DomainEventDescription("Instituição criada")]
public record InstitutionCreatedDomainEvent(Guid InstitutionId) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, InstitutionCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new SeedInstitutionBasicDataCommand(evt.InstitutionId), eventId: eventId);
        await Task.CompletedTask;
    }
}
