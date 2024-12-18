namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordEmail : ISykiTask
{
    public Guid UserId { get; set; }
}
