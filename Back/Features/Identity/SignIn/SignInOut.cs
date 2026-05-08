namespace Syki.Back.Features.Identity.SignIn;

public class SignInOut
{
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];
}
