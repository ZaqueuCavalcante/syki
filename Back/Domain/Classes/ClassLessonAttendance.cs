namespace Syki.Back.Commands.Domain.Classes;

/// <summary>
/// Representa se um Aluno estava presente ou não em uma Aula
/// </summary>
public class ClassLessonAttendance
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int LessonId { get; set; }
    public int StudentId { get; set; }
    public bool Present { get; set; }

    private ClassLessonAttendance() {}

    public ClassLessonAttendance(
        int classId,
        int lessonId,
        int studentId,
        bool present
    ) {
        ClassId = classId;
        LessonId = lessonId;
        StudentId = studentId;
        Present = present;
    }

    public void Update(bool present)
    {
        Present = present;
    }
}
