namespace Syki.Back.Errors;

public class SykiSuccess { }

public abstract class SykiError
{
    public abstract string Code { get; set; }
    public abstract string Message { get; set; }

    public SwaggerExample<ErrorOut> ToSwaggerExampleErrorOut()
    {
        return SwaggerExample.Create(Message, new ErrorOut { Message = Message });
    }
}
