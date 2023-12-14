namespace Syki.Shared;

public class UserOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Faculdade { get; set; }
    public string Role { get; set; }
}
