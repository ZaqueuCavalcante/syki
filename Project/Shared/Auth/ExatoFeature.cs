namespace Exato.Shared.Auth;

/// <summary>
/// Representa uma funcionalidade dentro do sistema.
/// </summary>
public class ExatoFeature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }

    public ExatoFeature() { }

    public ExatoFeature(
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
        return Id == ((ExatoFeature)obj).Id;
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
