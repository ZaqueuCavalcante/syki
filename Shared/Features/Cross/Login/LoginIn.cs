namespace Syki.Shared;

public class LoginIn
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginIn(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
