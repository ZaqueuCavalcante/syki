namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

/// <summary>
/// Representa se um Aluno estava presente ou não em uma Aula
/// </summary>
public class LessonAttendance
{
    public Guid Id { get; }
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

    public void Update(bool present)
    {
        Present = present;
    }

    public GetTeacherLessonAttendanceOut ToOut(string studentName)
    {
        return new()
        {
            LessonId = LessonId,
            StudentId = StudentId,
            StudentName = studentName,
            Present = Present,
        };
    }
}
