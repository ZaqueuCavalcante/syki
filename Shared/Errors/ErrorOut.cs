namespace Syki.Shared;

public class ErrorOut
{
    public string Code { get; set; }
    public string Message { get; set; }

    public override string ToString() => $"{Code}: {Message}";
}
