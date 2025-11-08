namespace Exato.Back.Features.Web;

public class ExatoWebResponseWrapper
{
    public object? Data { get; set; }
    public bool Error { get; set; }
    public string? Message { get; set; }

    public static ExatoWebResponseWrapper NewSuccess(object? data)
    {
        return new()
        {
            Data = data,
            Error = false,
        };
    }

    public static ExatoWebResponseWrapper NewError(string message)
    {
        return new()
        {
            Error = true,
            Message = message,
        };
    }
}
