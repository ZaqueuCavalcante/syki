namespace Syki.Shared;

public class ErrorOut
{
    public string Code { get; set; }
    public string Message { get; set; }
}

public class ForbiddenErrorOut : ErrorOut
{
    public new string Code { get; set; } = "403";
    public new string Message { get; set; } = "Forbidden";
}
