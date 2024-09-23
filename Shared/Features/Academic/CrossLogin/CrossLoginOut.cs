namespace Syki.Shared;

public class CrossLoginOut
{
    public string AccessToken { get; set; }

    public static implicit operator CrossLoginOut(OneOf<CrossLoginOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
