namespace Syki.Back.Errors;

public class WebhookSubscriptionNotFound : SykiError
{
    public static readonly WebhookSubscriptionNotFound I = new();
    public override string Code { get; set; } = nameof(WebhookSubscriptionNotFound);
    public override string Message { get; set; } = "Inscrição de webhook não encontrada.";
}


public class InvalidWebhookName : SykiError
{
    public static readonly InvalidWebhookName I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookName);
    public override string Message { get; set; } = "Nome de webhook inválido.";
}

public class InvalidWebhookUrl : SykiError
{
    public static readonly InvalidWebhookUrl I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookUrl);
    public override string Message { get; set; } = "URL de webhook inválida.";
}

public class InvalidWebhookEvents : SykiError
{
    public static readonly InvalidWebhookEvents I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookEvents);
    public override string Message { get; set; } = "Lista de eventos de webhook inválida.";
}
