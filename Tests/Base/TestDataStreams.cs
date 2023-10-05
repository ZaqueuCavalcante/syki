namespace Syki.Tests.Base;

public static class TestDataStreams
{
    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", "", " ", "  ", "     ", "La", })
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
}
