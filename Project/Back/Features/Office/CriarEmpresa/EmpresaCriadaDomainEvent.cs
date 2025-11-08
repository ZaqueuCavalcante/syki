namespace Exato.Back.Features.Office.CriarEmpresa;

public record EmpresaCriadaDomainEvent(Guid ExternalId) : IDomainEvent;

public class EmpresaCriadaDomainEventHandler(BackDbContext ctx) : IDomainEventHandler<EmpresaCriadaDomainEvent>
{
    public async Task Handle(int organizationId, Guid eventId, EmpresaCriadaDomainEvent evt)
    {
        ctx.AddCommand(organizationId, new BuscarDadosDaEmpresaNaReceitaFederalCommand(evt.ExternalId), eventId: eventId);

        await Task.CompletedTask;
    }
}
