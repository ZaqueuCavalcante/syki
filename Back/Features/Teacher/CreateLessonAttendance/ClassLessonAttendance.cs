namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

/// <summary>
/// Representa se um Aluno estava presente ou não em uma Aula
/// </summary>
public class ClassLessonAttendance
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid LessonId { get; set; }
    public Guid StudentId { get; set; }
    public bool Present { get; set; }

    private ClassLessonAttendance() {}

    public ClassLessonAttendance(
        Guid classId,
        Guid lessonId,
        Guid studentId,
        bool present
    ) {
        Id = Guid.CreateVersion7();
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
