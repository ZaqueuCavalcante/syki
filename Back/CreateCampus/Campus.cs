namespace Syki.Back.CreateCampus;

public class Campus
{
    public Guid Id { get; }
    public Guid InstitutionId { get; }
    public string Name { get; private set; }
    public string City { get; private set; }

    public Campus() { }

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
