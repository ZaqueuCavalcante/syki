using Newtonsoft.Json;

namespace Syki.Back.Features.Adm.ReprocessTask;

public class ReprocessTaskService(SykiDbContext ctx) : IAdmService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Reprocess(Guid id)
    {
        var task = await ctx.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (task is null) return new SykiTaskNotFound();

        if (task.ParentId != null) return new OnlyRootTasksCanBeReprocessed();

        var type = typeof(DomainEvent).Assembly.GetType(task.Type)!;
        dynamic data = JsonConvert.DeserializeObject(task.Data, type)!;
   
        var newTask = new SykiTask(null, task.InstitutionId, data)
        {
            Type = task.Type,
            ParentId = task.Id
        };

        ctx.Tasks.Add(newTask);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
