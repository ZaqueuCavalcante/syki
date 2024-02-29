using Syki.Back.FinishUserRegister;
using Syki.Back.SendResetPasswordToken;
using Syki.Back.CreatePendingUserRegister;

namespace Syki.Back.Tasks;

public class SykiTask
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }

    public SykiTask() { }

    public SykiTask(object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
    }

    public static SykiTask SendUserRegisterEmailConfirmation(string email)
    {
        return new SykiTask(new SendUserRegisterEmailConfirmation { Email = email });
    }

    public static SykiTask SendResetPasswordEmail(Guid userId)
    {
        return new SykiTask(new SendResetPasswordEmail { UserId = userId });
    }

    public static SykiTask SeedInstitutionData(Guid institutionId)
    {
        return new SykiTask(new SeedInstitutionData { InstitutionId = institutionId });
    }
}
