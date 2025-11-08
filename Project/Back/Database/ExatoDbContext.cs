using Exato.Back.Audit;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Back.Features.Cross.SendResetPasswordToken;

namespace Exato.Back.Database;

public partial class BackDbContext
{
    public DbSet<ResetPasswordToken> ExatoResetPasswordTokens { get; set; }

    public DbSet<DomainEvent> ExatoDomainEvents { get; set; }
    public DbSet<Command> ExatoCommands { get; set; }

    public DbSet<AuditTrail> ExatoAuditTrails { get; set; }

    private static void ConfigureExatoSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExatoRoleDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoRoleClaimDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoUserDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoUserRoleDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoUserClaimDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoUserTokenDbConfig());
        modelBuilder.ApplyConfiguration(new ExatoUserLoginDbConfig());

        modelBuilder.ApplyConfiguration(new ResetPasswordTokenDbConfig());

        modelBuilder.ApplyConfiguration(new DomainEventDbConfig());
        modelBuilder.ApplyConfiguration(new CommandDbConfig());
        modelBuilder.ApplyConfiguration(new CommandBatchDbConfig());

        modelBuilder.ApplyConfiguration(new AuditTrailDbConfig());
    }

    public Command AddCommand(
        int organizationId,
        ICommand command,
        Guid? eventId = null,
        Guid? parentId = null,
        Guid? originalId = null,
        Guid? batchId = null,
        int? delaySeconds = null)
    {
        return Add(
            new Command(
                organizationId,
                command,
                eventId: eventId,
                parentId: parentId,
                originalId: originalId,
                batchId: batchId,
                delaySeconds: delaySeconds,
                activityId: ActivityId
            )
        ).Entity;
    }
}
