namespace Syki.Shared;

public class CreateUserOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid InstitutionId { get; set; }
    public string Institution { get; set; }
    public string Role { get; set; }
}
