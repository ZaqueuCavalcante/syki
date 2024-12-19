namespace Syki.Back.Features.Cross.CreateInstitution;

public record InstitutionCreatedDomainEvent(Guid Id) : IDomainEvent;

public class InstitutionCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<InstitutionCreatedDomainEvent>
{
    public async Task Handle(InstitutionCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(new SeedInstitutionDataTask(evt.Id));
    }
}
