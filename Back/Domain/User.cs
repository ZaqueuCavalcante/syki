namespace Syki.Domain;

public class User
{
    public long Id { get; set; }
    
    public string Email { get; set; }

    public string Role { get; set; }

    public long FaculdadeId { get; set; }
}
