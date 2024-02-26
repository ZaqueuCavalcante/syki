namespace Syki.Back.CreatePendingUserRegister;

public class UserRegister
{
    public Guid Id { get; }
    public string Email { get; }
    public DateOnly? TrialStart { get; private set; }
    public DateOnly? TrialEnd { get; private set; }

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
