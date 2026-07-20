namespace Estud.Tests.Base;

public static class TestData
{
    public static IEnumerable<object[]> ClassActivityValidWeights()
    {
        foreach (var weight in new List<int>()
        {
            0, 1, 10, 60, 99, 100,
        })
        {
            yield return [weight];
        };
    }

    public static IEnumerable<object[]> ClassActivityInvalidWeights()
    {
        foreach (var weight in new List<int>()
        {
            -10, -1, 101, 110,
        })
        {
            yield return [weight];
        };
    }

    public static IEnumerable<int[]> ClassActivityValidWeightsLists()
    {
        yield return new[] { 0 };
        yield return new[] { 1 };
        yield return new[] { 60 };
        yield return new[] { 99 };
        yield return new[] { 100 };
        yield return new[] { 0, 0 };
        yield return new[] { 0, 50 };
        yield return new[] { 0, 100 };
        yield return new[] { 100, 0 };
        yield return new[] { 10, 20 };
        yield return new[] { 70, 30 };
        yield return new[] { 0, 0, 0 };
        yield return new[] { 50, 50, 0 };
        yield return new[] { 50, 10, 40 };
        yield return new[] { 10, 80, 10 };
        yield return new[] { 33, 33, 34 };
        yield return new[] { 0, 0, 0, 0 };
        yield return new[] { 0, 0, 50, 40 };
        yield return new[] { 25, 25, 25, 25 };
    }

    public static IEnumerable<int[]> ClassActivityInvalidWeightsLists()
    {
        yield return new[] { 100, 1 };
        yield return new[] { 100, 1 };
        yield return new[] { 50, 51 };
        yield return new[] { 100, 100 };
        yield return new[] { 99, 2 };
        yield return new[] { 0, 80, 21 };
        yield return new[] { 50, 50, 1 };
        yield return new[] { 90, 5, 6 };
        yield return new[] { 90, 5, 5, 1 };
        yield return new[] { 0, 50, 45, 6 };
    }
}
