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
        var hash = faculdadeId.ToString().OnlyNumbers();
        return new UserIn
        {
            Faculdade = faculdadeId,
            Name = $"{role} - {hash}",
            Email = $"{role.ToLower()}@{hash}.com",
            Password = $"{role}@123",
            Role = role,
        };
    }
}
