using Syki.Back.Features.Cross.SeedInstitutionData;

namespace Syki.Back.Features.Cross.CreateInstitution;

[DomainEvent(nameof(Institution), "Instituição criada")]
public record InstitutionCreatedDomainEvent(Guid InstitutionId) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, DomainEventId eventId, InstitutionCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new SeedInstitutionBasicDataCommand(evt.InstitutionId), eventId: eventId);
        await Task.CompletedTask;
    }
}
