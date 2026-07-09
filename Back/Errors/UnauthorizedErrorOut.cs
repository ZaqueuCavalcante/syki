namespace Estud.Back.Errors;

public class UnauthorizedErrorOut : ErrorOut
{
    public static readonly UnauthorizedErrorOut I = new();

    public UnauthorizedErrorOut()
    {
        Code = "401";
        Message = "Unauthorized";
    }
}
