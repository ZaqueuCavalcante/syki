namespace Syki.Back.Features.Cross.CreateInstitution;

[DomainEventDescription("Instituição criada")]
public record InstitutionCreatedDomainEvent(Guid Id) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, InstitutionCreatedDomainEvent evt)
    {
        ctx.AddCommands(institutionId, eventId, new SeedInstitutionDataCommand(evt.Id));
        await Task.CompletedTask;
    }
}
