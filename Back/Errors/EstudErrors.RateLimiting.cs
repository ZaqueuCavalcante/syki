namespace Estud.Back.Errors;

public class TooManyRequests : EstudError
{
    public static readonly TooManyRequests I = new();
    public override string Code { get; set; } = nameof(TooManyRequests);
    public override string Message { get; set; } = "Muitas requisições. Tente novamente em alguns instantes.";
}
