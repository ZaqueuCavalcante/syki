namespace Syki.Back.Features.Identity.SignIn;

public class SignInOut
{
    public int UserId { get; set; }
    public int InstitutionId { get; set; }
    public List<int> Permissions { get; set; } = [];
}
