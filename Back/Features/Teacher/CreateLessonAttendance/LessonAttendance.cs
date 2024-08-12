namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

public class LessonAttendance
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid LessonId { get; set; }
    public Guid StudentId { get; set; }
    public bool Present { get; set; }

    public LessonAttendance(
        Guid classId,
        Guid lessonId,
        Guid studentId,
        bool present
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        LessonId = lessonId;
        StudentId = studentId;
        Present = present;
    }
}
