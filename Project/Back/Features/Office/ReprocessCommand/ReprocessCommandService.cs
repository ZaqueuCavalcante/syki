using Newtonsoft.Json;

namespace Exato.Back.Features.Office.ReprocessCommand;

public class ReprocessCommandService(BackDbContext ctx) : IOfficeService
{
    public async Task<OneOf<ExatoSuccess, ExatoError>> Reprocess(Guid id)
    {
        var command = await ctx.ExatoCommands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (command is null) return new CommandNotFound();

        if (command.OriginalId != null) return new OnlyRootCommandsCanBeReprocessed();

        var type = typeof(Command).Assembly.GetType(command.Type)!;
        dynamic data = JsonConvert.DeserializeObject(command.Data, type)!;
   
        var newCommand = new Command(command.OrganizationId, data, originalId: command.Id)
        {
            Type = command.Type,
        };

        await ctx.SaveChangesAsync(newCommand);

        return new ExatoSuccess();
    }
}
