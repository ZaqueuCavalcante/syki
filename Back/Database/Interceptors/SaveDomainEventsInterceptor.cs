using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Syki.Back.Features.Cross.CreateInstitution;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Syki.Back.Database.Interceptors;

public sealed class SaveDomainEventsInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
        var activityId = Activity.Current?.Id;

        var httpContextAccessor = eventData.Context.GetService<IHttpContextAccessor>();
        var ctx = httpContextAccessor?.HttpContext;
        Guid? institutionId = (ctx != null && ctx.User.IsAuthenticated) ? ctx.User.InstitutionId : null;

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry =>
            {
                var entity = entry.Entity;
                var domainEvents = entity.GetDomainEvents();
                if (domainEvents.Count == 0) return [];

                entity.ClearDomainEvents();

                var entityId = entry.Property<Guid>("Id").CurrentValue;
                institutionId ??= (entry.Entity is Institution) ? entityId : entry.Property<Guid>("InstitutionId").CurrentValue;

                return domainEvents.Select(evt => new DomainEvent(institutionId.Value, entityId, evt, activityId));
            })
            .ToList();

        foreach (var evt in domainEvents)
        {
            eventData.Context.Add(evt);
        }

		return await Task.Run(() => result);
	}
}
