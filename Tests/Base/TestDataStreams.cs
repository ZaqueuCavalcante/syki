namespace Syki.Tests.Base;

public static class TestDataStreams
{
    public static IEnumerable<object[]> InvalidNamesStream()
    {
        foreach (var name in new List<string>() { null, "", "a", "", " ", "  ", "     ", "La", })
        {
            yield return new object[] { name };
        }
    }
}
