namespace Syki.Shared;

public class CampusOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public BrazilState State { get; set; }
    public string City { get; set; }
    public int Capacity { get; set; }
    public int Students { get; set; }
    public decimal FillRate { get; set; }

    public static implicit operator CampusOut(OneOf<CampusOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
