using Syki.Back.Events;

namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class UserRegister : IDomainEventsPublisher
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public DateOnly? TrialStart { get; set; }
    public DateOnly? TrialEnd { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = [];

    public UserRegister(string email)
    {
        Id = Guid.NewGuid();
        Email = email.ToLower();
        DomainEvents.Publish(Id, new PendingUserRegisterCreatedDomainEvent(Email));
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
