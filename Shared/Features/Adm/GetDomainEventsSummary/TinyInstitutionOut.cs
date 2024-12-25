namespace Syki.Shared;

public class TinyInstitutionOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((TinyInstitutionOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
