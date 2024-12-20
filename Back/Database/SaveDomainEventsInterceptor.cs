using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Syki.Back.Database;

public class SaveDomainEventsInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
        var domainEvents = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry =>
            {
                var entity = entry.Entity;
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents.Select(x => new DomainEvent(x));
            })
            .ToList();

        Console.WriteLine(domainEvents.Count);

        foreach (var evt in domainEvents)
        {
            eventData.Context.Add(evt);
        }

		return await Task.Run(() => result);
	}
}
