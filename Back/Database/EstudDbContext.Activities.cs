using Estud.Back.Domain.Activities;
using Estud.Back.Database.Activities;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<UserActivity> UserActivities { get; set; }

    private static void ConfigureActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserActivityDbConfig());
    }

    public void RecordSuccess(UserActivityType type, int? userId = null, int? institutionId = null, object? metadata = null)
    {
        Record(UserActivitySeverity.Success, type, userId, institutionId, metadata);
    }

    public void RecordInfo(UserActivityType type, int? userId = null, int? institutionId = null, object? metadata = null)
    {
        Record(UserActivitySeverity.Info, type, userId, institutionId, metadata);
    }

    public void RecordError(UserActivityType type, int? userId = null, int? institutionId = null, object? metadata = null)
    {
        Record(UserActivitySeverity.Error, type, userId, institutionId, metadata);
    }

    private void Record(UserActivitySeverity severity, UserActivityType type, int? userId = null, int? institutionId = null, object? metadata = null)
    {
        var activity = new UserActivity(
            severity,
            type,
            userId ?? (RequestUser.Id > 0 ? RequestUser.Id : null),
            institutionId ?? (RequestUser.InstitutionId > 0 ? RequestUser.InstitutionId : null),
            metadata
        );
        UserActivities.Add(activity);
    }
}
