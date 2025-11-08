using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Exato.Back.Database.Interceptors;

public sealed class SaveDomainEventsInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
        var domainEvents = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry =>
            {
                var entity = entry.Entity;
                var domainEvents = entity.GetDomainEvents();
                if (domainEvents.Count == 0) return [];

                entity.ClearDomainEvents();

                var ctx = eventData.Context as BackDbContext;
                return domainEvents.Select(evt => new DomainEvent(ctx.OrganizationId, evt, ctx.ActivityId));
            })
            .ToList();

        foreach (var evt in domainEvents)
        {
            eventData.Context.Add(evt);
        }

		return await Task.Run(() => result);
	}
}
