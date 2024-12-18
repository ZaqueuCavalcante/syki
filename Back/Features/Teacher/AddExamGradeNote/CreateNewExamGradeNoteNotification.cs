namespace Syki.Back.Features.Teacher.AddExamGradeNote;

public class CreateNewExamGradeNoteNotification : ISykiTask
{
    public Guid UserId { get; set; }
    public Guid ClassId { get; set; }
}
