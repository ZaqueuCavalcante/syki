using System.Text.Json;

namespace Syki.Back.Domain.Activities;

public class UserActivity
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? InstitutionId { get; set; }
    public FeatureGroup FeatureGroup { get; set; }
    public UserActivitySeverity Severity { get; set; }
    public UserActivityType ActivityType { get; set; }
    public JsonDocument? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserActivity() {}

    public UserActivity(
        UserActivitySeverity severity,
        UserActivityType type,
        int? userId = null,
        int? institutionId = null,
        object? metadata = null)
    {
        ActivityType = type;
        UserId = userId;
        InstitutionId = institutionId;
        CreatedAt = DateTime.UtcNow;
        Severity = severity;
        FeatureGroup = (type.ToInt() / 10_000).IntToEnum<FeatureGroup>();
        Metadata = metadata != null ? JsonDocument.Parse(metadata.Serialize()) : null;
    }
}
