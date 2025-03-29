using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetDomainEvent;

public class GetDomainEventService(DatabaseSettings settings) : IAdmService
{
    public async Task<DomainEventOut> Get(Guid id)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT *
            FROM syki.domain_events
            WHERE id = @Id
        ";
        var evt = await connection.QueryFirstOrDefaultAsync<DomainEventOut>(sql, new{ id }) ?? new();

        evt.EntityName = evt.Type.ToDomainEventEntityName();
        evt.Type = evt.Type.ToDomainEventDescription();

        const string commandsSql = @"
            SELECT *
            FROM syki.commands
            WHERE event_id = @Id
        ";
        evt.Commands = (await connection.QueryAsync<DomainEventCommandOut>(commandsSql, new{ id })).ToList();
        evt.Commands.ForEach(x => x.Type = x.Type.ToCommandDescription());

        return evt;
    }
}
