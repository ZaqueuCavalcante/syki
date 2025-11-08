namespace Exato.Shared.Errors;

public class ForbiddenErrorOut : ErrorOut
{
    public static readonly ForbiddenErrorOut I = new();
    public new string Code { get; set; } = "403";
    public new string Message { get; set; } = "Forbidden";
}
