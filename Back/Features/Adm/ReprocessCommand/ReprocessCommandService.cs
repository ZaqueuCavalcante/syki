using Newtonsoft.Json;

namespace Syki.Back.Features.Adm.ReprocessCommand;

public class ReprocessCommandService(SykiDbContext ctx) : IAdmService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Reprocess(Guid id)
    {
        var command = await ctx.Commands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (command is null) return new CommandNotFound();

        if (command.OriginalId != null) return new OnlyRootCommandsCanBeReprocessed();

        var type = typeof(DomainEvent).Assembly.GetType(command.Type)!;
        dynamic data = JsonConvert.DeserializeObject(command.Data, type)!;
   
        var newCommand = new Command(command.InstitutionId, data, originalId: command.Id)
        {
            Type = command.Type,
        };

        ctx.Commands.Add(newCommand);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
