namespace Syki.Shared;

public class GetTeacherLessonAttendanceOut
{
    public Guid LessonId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public bool Present { get; set; }
}
