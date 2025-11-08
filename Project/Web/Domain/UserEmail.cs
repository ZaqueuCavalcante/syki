namespace Exato.Web.Domain;

public class UserEmail
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool Main { get; set; }

    public string Email { get; set; }

    public bool Verified { get; set; }

    public DateTime? VerificationDate { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public UserEmail() { }

    public UserEmail(int userId, string email)
    {
        UserId = userId;
        Email = email;
        Main = true;
        Verified = true;
    }

    public void Editar(string email)
    {
        Email = email;
    }
}
