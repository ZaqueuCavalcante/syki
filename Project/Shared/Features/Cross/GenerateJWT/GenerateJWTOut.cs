namespace Exato.Shared.Features.Cross.GenerateJWT;

public class GenerateJWTOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public List<int> Features { get; set; } = [];

    public string JWT { get; set; }
}
