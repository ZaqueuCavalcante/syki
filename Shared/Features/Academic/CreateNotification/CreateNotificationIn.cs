namespace Syki.Shared;

public record CreateNotificationIn(string Title, string Description, UsersGroup TargetUsers, bool Timeless);
