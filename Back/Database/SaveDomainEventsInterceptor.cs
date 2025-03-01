using Microsoft.EntityFrameworkCore.Diagnostics;
using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Back.Database;

public class SaveDomainEventsInterceptor(IHttpContextAccessor HttpContextAccessor) : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
        var ctx = HttpContextAccessor.HttpContext;
        Guid? institutionId = (ctx != null && ctx.User.IsAuthenticated()) ? ctx.User.InstitutionId() : null;

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry =>
            {
                var entity = entry.Entity;
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                var entityId = entry.Property<Guid>("Id").CurrentValue;
                institutionId ??= (entry.Entity is Institution) ? entityId : entry.Property<Guid>("InstitutionId").CurrentValue;

                return domainEvents.Select(x => new DomainEvent(institutionId.Value, entityId, x));
            })
            .ToList();

        foreach (var evt in domainEvents)
        {
            eventData.Context.Add(evt);
        }

		return await Task.Run(() => result);
	}
}
