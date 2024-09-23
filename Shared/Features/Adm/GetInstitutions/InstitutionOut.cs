namespace Syki.Shared;

public class InstitutionOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal NoteLimit { get; set; }
    public decimal FrequencyLimit { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
