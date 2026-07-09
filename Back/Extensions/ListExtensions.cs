namespace Estud.Back.Extensions;

public static class ListExtensions
{
    extension(List<decimal> notes)
    {
        public decimal GetAverageNote()
        {
            if (notes.Count <= 2) return 0;
            var average = notes.Select(x => x).OrderDescending().Take(2).Average();
            return Math.Round(average, 2);
        }
    }
}
