using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Syki.Back.Database;

public class SaveDomainEventsInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		var entries = eventData.Context.ChangeTracker
			.Entries<Entity>()
            .ToList();

        foreach (var entry in entries)
        {
            var entityId = (Guid) entry.Property("Id").CurrentValue!;
            var events = entry.Entity.GetDomainEvents();
            foreach (var evt in events)
            {
                eventData.Context.Add(new DomainEvent(entityId, evt));
            }
        }

		return await Task.Run(() => result);
	}
}
