namespace Syki.Tests.Base;

public static class TestData
{
    public static IEnumerable<object[]> ValidNames()
    {
        foreach (var name in new List<string>() { "Keu", "Maju", "Maria", "Naldinho Silva", })
        {
            yield return [name];
        }
    }

    public static IEnumerable<object[]> InvalidNames()
    {
        foreach (var name in new List<string>() { null, "", "a", "42", " ", "  ", "     ", "JP", })
        {
            yield return [name];
        }
    }

    public static IEnumerable<object[]> InvalidUserRegisterTokens()
    {
        var empty = Guid.Empty.ToString();
        var random = Guid.CreateVersion7().ToString();
        foreach (var name in new List<string>() { null, "", "a", "42", "qwerty123", " ", "  ", "     ", "JP", empty, random, })
        {
            yield return [name];
        }
    }



    public static IEnumerable<object[]> ValidPeriods()
    {
        foreach (var id in new List<string>() { "1970.1", "1970.2", "2023.1", "2023.2", "2070.1", "2070.2", })
        {
            yield return [id];
        }
    }

    public static IEnumerable<object[]> InvalidPasswords()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "syki",
            "syki123",
            "Syki123",
            "lalal.com",
            "12@3lalala",
            "5816811681816",
        })
        {
            yield return [role];
        }
    }

    public static IEnumerable<object[]> InvalidMfaTokens()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "syki",
            "5464",
            "123456",
            "lalal.com",
            "5816811681816",
        })
        {
            yield return [role];
        }
    }

    public static IEnumerable<ScheduleIn[]> ConflictingSchedules()
    {
        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
            new ScheduleIn(Day.Monday, Hour.H07_30, Hour.H07_45),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H10_00, Hour.H11_00),
            new ScheduleIn(Day.Monday, Hour.H09_30, Hour.H12_15),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
            new ScheduleIn(Day.Monday, Hour.H07_30, Hour.H08_30),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_30, Hour.H08_30),
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
            new ScheduleIn(Day.Tuesday, Hour.H08_00, Hour.H09_00),
            new ScheduleIn(Day.Monday, Hour.H07_15, Hour.H07_45),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Wednesday, Hour.H12_00, Hour.H15_30),
            new ScheduleIn(Day.Wednesday, Hour.H13_00, Hour.H14_15),
        };
    }

    public static IEnumerable<ScheduleIn[]> ValidSchedules()
    {
        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
            new ScheduleIn(Day.Monday, Hour.H08_00, Hour.H09_00),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H08_00, Hour.H09_00),
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Monday, Hour.H07_00, Hour.H08_00),
            new ScheduleIn(Day.Tuesday, Hour.H08_00, Hour.H09_00),
            new ScheduleIn(Day.Monday, Hour.H09_45, Hour.H10_15),
        };

        yield return new[]
        {
            new ScheduleIn(Day.Wednesday, Hour.H12_00, Hour.H15_30),
            new ScheduleIn(Day.Wednesday, Hour.H11_15, Hour.H12_00),
        };
    }

    public static IEnumerable<object[]> ValidNotes()
    {
        foreach (var note in new List<decimal>()
        {
            0.00M,
            5.67M,
            10.00M,
        })
        {
            yield return [note];
        }
    }

    public static IEnumerable<object[]> InvalidNotes()
    {
        foreach (var note in new List<decimal>()
        {
            -0.01M,
            10.01M,
        })
        {
            yield return [note];
        }
    }

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
