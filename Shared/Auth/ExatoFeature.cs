namespace Syki.Shared.Auth;

/// <summary>
/// Representa uma funcionalidade do sistema.
/// </summary>
public class SykiFeature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }

    public SykiFeature() { }

    public SykiFeature(
        int groupId,
        int id,
        string name)
    {
        GroupId = groupId;
        Id = id;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((SykiFeature)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public override string ToString()
    {
        return Name;
    }
}
