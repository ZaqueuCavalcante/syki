namespace Syki.Back.Features.Notifications.MarkNotificationsAsViewed;

public class InvalidNotificationId : SykiError
{
    public static readonly InvalidNotificationId I = new();
    public override string Code { get; set; } = nameof(InvalidNotificationId);
    public override string Message { get; set; } = "ID de notificação inválido.";
}
