using Syki.Back.Events;
using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Back.Extensions;

public static class ListExtensions
{
    public static decimal GetAverageNote(this IEnumerable<ExamGrade> examGrades)
    {
        var average = examGrades.Select(x => x.Note).OrderDescending().Take(2).Average();
        return Math.Round(average, 2);
    }

    public static void Publish(this List<DomainEvent> domainEvents, Guid entityId, object data)
    {
        domainEvents.Add(new DomainEvent(entityId, data));
    }
}
