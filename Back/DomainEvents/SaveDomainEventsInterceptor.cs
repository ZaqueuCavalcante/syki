using Estud.Back.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estud.Back.DomainEvents;

public sealed class SaveDomainEventsInterceptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
        var ctx = eventData.Context as EstudDbContext;

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<DomainEntity>()
            .SelectMany(entry =>
            {
                var entity = entry.Entity;
                var domainEvents = entity.GetDomainEvents();
                if (domainEvents.Count == 0) return [];

                entity.ClearDomainEvents();

                return domainEvents.Select(evt => new DomainEvent(ctx.RequestUser.InstitutionId, entity.Uid, evt, ctx.ActivityId));
            })
            .ToList();

        foreach (var evt in domainEvents)
        {
            eventData.Context.Add(evt);
            ctx.HasPendingDomainEvents = true;
        }

		return ValueTask.FromResult(result);
	}
}
