namespace Syki.Shared;

public class UserIn
{
    public Guid Faculdade { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public static UserIn New(Guid faculdadeId, string role)
    {
        return new UserIn
        {
            Faculdade = faculdadeId,
            Name = $"{role} - Nova Roma",
            Email = $"{role.ToLower()}@novaroma.com",
            Password = $"{role}@123",
            Role = role,
        };
    }
}
