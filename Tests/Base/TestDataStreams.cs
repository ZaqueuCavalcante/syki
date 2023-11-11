namespace Syki.Tests.Base;

public static class TestDataStreams
{
    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", " ", "  ", "     ", "La", })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> CamelCaseNames()
    {
        foreach (var name in new List<(string, string)>()
        {
            ("AspNetUsers", "asp_net_users"),
            ("AspNetUserRoles", "asp_net_user_roles"),
        })
        {
            yield return new object[] { name };
        }
    }

    public static IEnumerable<object[]> FormatedStrings()
    {
        foreach (var text in new List<(string, string)>()
        {
            ("629.219.140-00", "62921914000"),
            ("18.297.767/0001-90", "18297767000190"),
            ("yu2v34y1434u6b54u6b", "23414346546"),
        })
        {
            yield return new object[] { text };
        }
    }
}
