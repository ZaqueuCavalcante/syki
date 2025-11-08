namespace Exato.Front.Components.Users;

public class UserSelectData
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid? GetId()
    {
        return Id == Guid.Empty ? null : Id;
    }

    public override string ToString()
    {
        return Name;
    }
}
