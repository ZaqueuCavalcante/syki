namespace Syki.Back.Auth.Permissions;

/// <summary>
/// Representa uma permissão dentro do sistema.
/// </summary>
public class SykiPermission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PermissionGroup Group { get; set; }

    public SykiPermission() { }

    public SykiPermission(
        PermissionGroup group,
        int id,
        string name,
        string description)
    {
        Group = group;
        Id = id;
        Name = name;
        Description = description;
    }
}
