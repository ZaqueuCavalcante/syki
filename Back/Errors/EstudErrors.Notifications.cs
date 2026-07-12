namespace Estud.Back.Errors;

public class NotificationNotFound : EstudError
{
    public static readonly NotificationNotFound I = new();
    public override string Code { get; set; } = nameof(NotificationNotFound);
    public override string Message { get; set; } = "Notificação não encontrada.";
}

public class InvalidNotificationTitle : EstudError
{
    public static readonly InvalidNotificationTitle I = new();
    public override string Code { get; set; } = nameof(InvalidNotificationTitle);
    public override string Message { get; set; } = "Título de notificação inválido.";
}

public class InvalidNotificationDescription : EstudError
{
    public static readonly InvalidNotificationDescription I = new();
    public override string Code { get; set; } = nameof(InvalidNotificationDescription);
    public override string Message { get; set; } = "Descrição de notificação inválida.";
}
