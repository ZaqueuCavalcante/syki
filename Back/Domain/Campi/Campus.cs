namespace Syki.Back.Domain.Campi;

public class Campus
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public BrazilState State { get; set; }
    public string City { get; set; }
    public int Capacity { get; set; }

    private Campus() { }

    public Campus(int institutionId, string name, BrazilState state, string city, int capacity)
    {
        InstitutionId = institutionId;
        Name = name;
        State = state;
        City = city;
        Capacity = capacity;
    }

    public void Update(string name, BrazilState state, string city, int capacity)
    {
        Name = name;
        State = state;
        City = city;
        Capacity = capacity;
    }
}
