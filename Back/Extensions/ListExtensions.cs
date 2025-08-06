using Syki.Back.Features.Teacher.AddClassActivityNote;

namespace Syki.Back.Extensions;

public static class ListExtensions
{
    extension(List<StudentClassNote> notes)
    {
        public decimal GetAverageNote()
        {
            if (notes.Count <= 2) return 0;
            var average = notes.Select(x => x.Note).OrderDescending().Take(2).Average();
            return Math.Round(average, 2);
        }
    }
}
