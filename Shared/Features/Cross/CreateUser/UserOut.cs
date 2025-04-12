namespace Syki.Shared;

public class UserOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid InstitutionId { get; set; }
    public string Institution { get; set; }
    public string Role { get; set; }
    public bool Online { get; set; }
    public int Connections { get; set; }
}
