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
        Email = email.ToLower();
    }

    public OneOf<SykiSuccess, SykiError> Finish()
    {
        if (TrialStart != null)
            return new UserAlreadyRegistered();

        TrialStart = DateOnly.FromDateTime(DateTime.Now);
        TrialEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7));

        return new SykiSuccess();
    }
}
