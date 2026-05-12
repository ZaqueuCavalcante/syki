using System.Diagnostics;
using Syki.Back.Database.Commands;
using Syki.Back.Commands.Domain.Enums;
using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public bool HasPendingCommands { get; set; }
    public List<string> CommandLogs { get; set; } = [];

    public DbSet<Command> Commands { get; set; }

    private static void ConfigureCommands(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommandDbConfig());
        modelBuilder.ApplyConfiguration(new CommandBatchDbConfig());
    }

    public Command AddCommand(
        int institutionId,
        ICommand command,
        int? parentId = null,
        int? originalId = null,
        int? batchId = null,
        int? delaySeconds = null,
        int maxRetries = 0,
        int baseDelaySeconds = 5,
        BackoffStrategy backoffStrategy = BackoffStrategy.None)
    {
        var activityId = Activity.Current?.Id;

        HasPendingCommands = true;

        return Add(
            new Command(
                institutionId,
                command,
                parentId: parentId,
                originalId: originalId,
                batchId: batchId,
                delaySeconds: delaySeconds,
                activityId: activityId,
                maxRetries: maxRetries,
                backoffStrategy: backoffStrategy,
                baseDelaySeconds: baseDelaySeconds
            )
        ).Entity;
    }
}
