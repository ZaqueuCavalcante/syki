namespace Syki.Shared;

public class ForbiddenErrorOut : ErrorOut
{
    public new string Code { get; set; } = "403";
    public new string Message { get; set; } = "Forbidden";
}
