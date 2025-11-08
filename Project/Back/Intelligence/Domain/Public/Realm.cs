namespace Exato.Back.Intelligence.Domain.Public;

public class Realm
{
    public short Id { get; set; }

    public Guid Uid { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
