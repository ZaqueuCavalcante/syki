using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Back.Extensions;

public static class ListExtensions
{
    public static decimal GetAverageNote(this IEnumerable<ExamGrade> examGrades)
    {
        if (examGrades.Count() <= 2) return 0;
        var average = examGrades.Select(x => x.Note).OrderDescending().Take(2).Average();
        return Math.Round(average, 2);
    }
}
