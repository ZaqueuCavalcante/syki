namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class UserRegister : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Email { get; set; }
    public UserRegisterStatus Status { get; set; }

    private UserRegister() {}

    public UserRegister(string email)
    {
        Id = Guid.NewGuid();
        Email = email.ToLower();
        InstitutionId = Guid.NewGuid();
        AddDomainEvent(new PendingUserRegisterCreatedDomainEvent(Id));
    }

    public OneOf<SykiSuccess, SykiError> Finish()
    {
        if (Status == UserRegisterStatus.Finished)
            return new UserAlreadyRegistered();

        Status = UserRegisterStatus.Finished;

        return new SykiSuccess();
    }
}
