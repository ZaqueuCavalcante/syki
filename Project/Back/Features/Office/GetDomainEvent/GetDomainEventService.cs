using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetDomainEvent;

namespace Exato.Back.Features.Office.GetDomainEvent;

public class GetDomainEventService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<OneOf<GetDomainEventOut, ExatoError>> Get(Guid id)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string eventSql = @"
            SELECT *
            FROM exato.domain_events
            WHERE id = @Id
        ";

        var parameters = new { id };

        var evt = await connection.QueryFirstOrDefaultAsync<GetDomainEventOut>(eventSql, parameters);

        if (evt == null) return DomainEventNotFound.I;

        const string commandsSql = @"
            SELECT *
            FROM exato.commands
            WHERE event_id = @Id
        ";
        evt.Commands = (await connection.QueryAsync<GetDomainEventOutCommandOut>(commandsSql, parameters)).ToList();

        return evt;
    }
}
