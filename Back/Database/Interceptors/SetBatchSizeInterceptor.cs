using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Syki.Back.Database.Interceptors;

public class SetBatchSizeInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
        var batches = eventData.Context.ChangeTracker
            .Entries<CommandBatch>()
            .Select(x => x.Entity)
            .ToList();

        foreach (var batch in batches)
        {
            batch.Size = eventData.Context.ChangeTracker
                .Entries<Command>()
                .Where(x => x.Entity.BatchId == batch.Id)
                .Count();
        }

		return await Task.Run(() => result);
	}
}
