namespace Syki.Back.Errors;

public class InvalidNotificationTitle : SykiError
{
    public static readonly InvalidNotificationTitle I = new();
    public override string Code { get; set; } = nameof(InvalidNotificationTitle);
    public override string Message { get; set; } = "Título de notificação inválido.";
}

public class InvalidNotificationDescription : SykiError
{
    public static readonly InvalidNotificationDescription I = new();
    public override string Code { get; set; } = nameof(InvalidNotificationDescription);
    public override string Message { get; set; } = "Descrição de notificação inválida.";
}
