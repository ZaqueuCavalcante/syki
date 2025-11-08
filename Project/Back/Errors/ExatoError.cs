namespace Exato.Back.Errors;

public class ExatoSuccess { }

public abstract class ExatoError
{
    public abstract string Code { get; set; }
    public abstract string Message { get; set; }

    public override string ToString() => $"{Code}: {Message}";

    public SwaggerExample<ErrorOut> ToSwaggerExampleErrorOut()
    {
        return SwaggerExample.Create(Message, new ErrorOut { Code = Code, Message = Message });
    }
}
