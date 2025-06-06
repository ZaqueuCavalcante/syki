namespace Syki.Back.Features.Academic.CreateCampus;

/// <summary>
/// Campus
/// </summary>
public class Campus
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public BrazilState State { get; set; }
    public string City { get; set; }

    private Campus() { }

    public Campus(Guid institutionId, string name, BrazilState state, string city)
    {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        Name = name;
        State = state;
        City = city;
    }

    public void Update(string name, BrazilState state, string city)
    {
        Name = name;
        State = state;
        City = city;
    }

    public CampusOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            State = State,
            City = City,
        };
    }
}
