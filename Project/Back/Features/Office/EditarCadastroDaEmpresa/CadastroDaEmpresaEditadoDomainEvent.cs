using Exato.Back.Features.Office.CriarEmpresa;

namespace Exato.Back.Features.Office.EditarCadastroDaEmpresa;

public record CadastroDaEmpresaEditadoDomainEvent(Guid ExternalId) : IDomainEvent;

public class CadastroDaEmpresaEditadoDomainEventHandler(BackDbContext ctx) : IDomainEventHandler<CadastroDaEmpresaEditadoDomainEvent>
{
    public async Task Handle(int organizationId, Guid eventId, CadastroDaEmpresaEditadoDomainEvent evt)
    {
        ctx.AddCommand(organizationId, new BuscarDadosDaEmpresaNaReceitaFederalCommand(evt.ExternalId), eventId: eventId);

        await Task.CompletedTask;
    }
}
