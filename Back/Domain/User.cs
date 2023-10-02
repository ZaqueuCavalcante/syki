namespace Syki.Domain;

public class User
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
}
