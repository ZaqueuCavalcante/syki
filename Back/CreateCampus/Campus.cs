using Syki.Shared.GetCampi;
using Syki.Shared.CreateCampus;
using Syki.Shared.UpdateCampus;

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

    public CreateCampusOut ToCreateCampusOut()
    {
        return new CreateCampusOut
        {
            Id = Id,
            Name = Name,
            City = City,
        };
    }

    public UpdateCampusOut ToUpdateCampusOut()
    {
        return new UpdateCampusOut
        {
            Id = Id,
            Name = Name,
            City = City,
        };
    }

    public GetCampusOut ToGetCampusOut()
    {
        return new GetCampusOut
        {
            Id = Id,
            Name = Name,
            City = City,
        };
    }
}
