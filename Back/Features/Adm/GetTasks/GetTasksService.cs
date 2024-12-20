namespace Syki.Back.Features.Adm.GetTasks;

public class GetTasksService(SykiDbContext ctx) : IAdmService
{
    public async Task<List<TaskOut>> Get()
    {
        var tasks = await ctx.Tasks
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return tasks.ConvertAll(e => e.ToOut());
    }
}
