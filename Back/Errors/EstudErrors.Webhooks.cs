namespace Estud.Back.Errors;

public class WebhookSubscriptionNotFound : EstudError
{
    public static readonly WebhookSubscriptionNotFound I = new();
    public override string Code { get; set; } = nameof(WebhookSubscriptionNotFound);
    public override string Message { get; set; } = "Inscrição de webhook não encontrada.";
}

public class InvalidWebhookName : EstudError
{
    public static readonly InvalidWebhookName I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookName);
    public override string Message { get; set; } = "Nome de webhook inválido.";
}

public class InvalidWebhookUrl : EstudError
{
    public static readonly InvalidWebhookUrl I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookUrl);
    public override string Message { get; set; } = "URL de webhook inválida.";
}

public class InvalidWebhookEvents : EstudError
{
    public static readonly InvalidWebhookEvents I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookEvents);
    public override string Message { get; set; } = "Lista de eventos de webhook inválida.";
}

public class InvalidWebhookCustomHeaders : EstudError
{
    public static readonly InvalidWebhookCustomHeaders I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookCustomHeaders);
    public override string Message { get; set; } = "Headers customizados de webhook inválidos.";
}
