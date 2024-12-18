using Syki.Back.Features.Teacher.AddExamGradeNote;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.LinkOldNotifications;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.Tasks;

public class SykiTask
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
    public Guid? ProcessorId { get; set; }

    public SykiTask() { }

    public SykiTask(object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
    }

    public static SykiTask SendResetPasswordEmail(Guid userId)
    {
        return new SykiTask(new SendResetPasswordEmail { UserId = userId });
    }

    public static SykiTask SeedInstitutionData(Guid institutionId)
    {
        return new SykiTask(new SeedInstitutionData { InstitutionId = institutionId });
    }

    public static SykiTask LinkOldNotifications(Guid userId, Guid institutionId)
    {
        return new SykiTask(new LinkOldNotifications { UserId = userId, InstitutionId = institutionId });
    }

    public static SykiTask CreateNewExamGradeNoteNotification(Guid userId, Guid classId)
    {
        return new SykiTask(new CreateNewExamGradeNoteNotification { UserId = userId, ClassId = classId });
    }
}
