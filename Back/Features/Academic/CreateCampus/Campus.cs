namespace Syki.Back.Features.Academic.CreateCampus;

public class Campus
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    private Campus() { }

    public Campus(Guid institutionId, string name, string city)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        City = city;
    }

    public void Update(string name, string city)
    {
        Name = name;
        City = city;
    }

    public CampusOut ToOut()
    {
        return new CampusOut
        {
            Id = Id,
            Name = Name,
            City = City,
        };
    }
}
