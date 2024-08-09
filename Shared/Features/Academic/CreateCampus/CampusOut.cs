namespace Syki.Shared;

public class CampusOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    public static implicit operator CampusOut(OneOf<CampusOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
