namespace Syki.Shared;

public class InstitutionOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
