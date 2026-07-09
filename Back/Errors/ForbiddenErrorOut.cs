namespace Estud.Back.Errors;

public class ForbiddenErrorOut : ErrorOut
{
    public static readonly ForbiddenErrorOut I = new();

    public ForbiddenErrorOut()
    {
        Code = "403";
        Message = "Forbidden";
    }
}
