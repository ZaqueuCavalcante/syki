namespace Syki.Shared;

public class CreatePendingUserRegisterIn
{
    public string Email { get; set; }

    public CreatePendingUserRegisterIn(string email)
    {
        Email = email;
    }
}
