namespace Syki.Shared;

public class GetStudentClassLessonFrequencyOut
{
    public int Lesson { get; set; }
    public string LessonDate { get; set; }
    public decimal Frequency { get; set; }

    public string GetLesson()
    {
        return $"Aula {Lesson} - {LessonDate}";
    }
}
