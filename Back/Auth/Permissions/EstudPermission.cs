namespace Estud.Back.Auth.Permissions;

/// <summary>
/// Representa uma permissão dentro do sistema.
/// </summary>
public class EstudPermission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PermissionGroup Group { get; set; }
    public List<UserType> AllowedTypes { get; set; }

    public EstudPermission() { }

    public EstudPermission(
        PermissionGroup group,
        int id,
        string name,
        string description,
        List<UserType> allowedTypes)
    {
        Group = group;
        Id = id;
        Name = name;
        Description = description;
        AllowedTypes = allowedTypes;
    }
}
