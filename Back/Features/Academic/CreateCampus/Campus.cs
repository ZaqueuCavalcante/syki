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
    public int Capacity { get; set; }

    private Campus() { }

    public Campus(Guid institutionId, string name, BrazilState state, string city, int capacity)
    {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        Name = name;
        State = state;
        City = city;
        Capacity = capacity;
    }

    public static OneOf<Campus, SykiError> New(
        Guid institutionId,
        string name,
        BrazilState state,
        string city,
        int capacity
    ) {
        if (capacity <= 0) return new InvalidCampusCapacity();

        return new Campus(institutionId, name, state, city, capacity);
    }

    public void Update(string name, BrazilState state, string city, int capacity)
    {
        Name = name;
        State = state;
        City = city;
        Capacity = capacity;
    }

    public CampusOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            State = State,
            City = City,
            Capacity = Capacity,
        };
    }
}
