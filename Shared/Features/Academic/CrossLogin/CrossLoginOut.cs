namespace Syki.Shared;

public class CrossLoginOut
{
    public string AccessToken { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

    public static implicit operator CrossLoginOut(OneOf<CrossLoginOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
