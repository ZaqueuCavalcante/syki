namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class UserRegister
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public DateOnly? TrialStart { get; set; }
    public DateOnly? TrialEnd { get; set; }

    public UserRegister(string email)
    {
        Id = Guid.NewGuid();
        if (!email.IsValidEmail())
            Throw.DE016.Now();
        Email = email.ToLower();
    }

    public void Finish()
    {
        if (TrialStart != null)
            Throw.DE025.Now();

        TrialStart = DateOnly.FromDateTime(DateTime.Now);
        TrialEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
    }
}
