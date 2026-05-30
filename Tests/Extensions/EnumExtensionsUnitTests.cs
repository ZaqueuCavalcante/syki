namespace Syki.Tests.Extensions;

public class EnumExtensionsUnitTests
{
    [Test]
    public void EnumExtensions_Should_get_enum_description_when_null()
    {
        // Arrange / Act
        var result = ((Enum)null!).GetDescription();

        // Assert
        result.Should().Be("");
    }

    [Test]
    public void EnumExtensions_Should_get_enum_description_when_has_no_description_attribute()
    {
        // Arrange / Act
        var result = TestEnum.WithoutDescription.GetDescription();

        // Assert
        result.Should().Be("WithoutDescription");
    }

    [Test]
    [TestCaseSource(nameof(CourseTypeEnumToDescription))]
    public void EnumExtensions_Should_get_enum_description(CourseType type, string description)
    {
        // Arrange / Act
        var result = type.GetDescription();

        // Assert
        result.Should().Be(description);
    }

    [Test]
    [TestCaseSource(nameof(CourseTypeEnumForIsIn))]
    public void EnumExtensions_Should_get_if_value_is_in_list(Enum value, bool isIn)
    {
        // Arrange / Act
        var result = value.IsIn(CourseType.Bacharelado, CourseType.Tecnologo);

        // Assert
        result.Should().Be(isIn);
    }

    [Test]
    public void EnumExtensions_Should_return_false_when_value_is_null()
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn(null);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void EnumExtensions_Should_return_false_when_value_is_empty()
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn([]);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(EnumsInvalidValues))]
    public void EnumExtensions_Should_return_false_when_value_is_out_of_range(Enum value)
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn(value);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(EnumsInvalidValues))]
    public void EnumExtensions_Should_return_false_when_value_is_invalid(Enum value)
    {
        // Arrange / Act
        var result = value.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(HoursDiffsInMinutes))]
    public void EnumExtensions_Should_get_hours_diff(Hour hourA, Hour hourB, int diff)
    {
        // Arrange / Act
        var result = hourA.DiffInMinutes(hourB);

        // Assert
        result.Should().Be(diff);
    }

    private enum TestEnum
    {
        WithoutDescription
    }

    private static IEnumerable<object[]> CourseTypeEnumToDescription()
    {
        foreach (var (type, description) in new List<(CourseType, string)>()
        {
            (CourseType.Bacharelado, "Bacharelado"),
            (CourseType.Licenciatura, "Licenciatura"),
            (CourseType.Tecnologo, "Tecnólogo"),
            (CourseType.Especializacao, "Especialização"),
            (CourseType.Mestrado, "Mestrado"),
            (CourseType.Doutorado, "Doutorado"),
            (CourseType.PosDoutorado, "Pós-Doutorado"),
        })
        {
            yield return [type, description];
        }
    }

    private static IEnumerable<object[]> CourseTypeEnumForIsIn()
    {
        foreach (var (value, isIn) in new List<(Enum, bool)>()
        {
            (CourseType.Bacharelado, true),
            (StudentDisciplineStatus.Matriculado, false),
            (CourseType.Tecnologo, true),
            (CourseSession.Afternoon, false),
        })
        {
            yield return [value, isIn];
        }
    }

    private static IEnumerable<object[]> EnumsInvalidValues()
    {
        foreach (var value in new List<Enum>()
        {
            (Day)69,
            (Hour)69,
            (UserRole)69,
            (CourseType)69,
            (ClassStatus)69,
            (StudentDisciplineStatus)(-69),
        })
        {
            yield return [value];
        }
    }

    private static IEnumerable<object[]> HoursDiffsInMinutes()
    {
        foreach (var (hourA, hourB, diff) in new List<(Hour, Hour, int)>()
        {
            (Hour.H07_00, Hour.H07_00, 00),
            (Hour.H07_00, Hour.H07_15, 15),
            (Hour.H07_00, Hour.H07_45, 45),
            (Hour.H07_00, Hour.H08_00, 01*60),
            (Hour.H07_15, Hour.H08_30, 01*60+15),
            (Hour.H07_15, Hour.H08_45, 01*60+30),
            (Hour.H07_30, Hour.H08_30, 01*60),
            (Hour.H07_00, Hour.H12_00, 05*60),
            (Hour.H13_00, Hour.H19_00, 06*60),
            (Hour.H07_15, Hour.H09_45, 02*60+30),
            (Hour.H08_45, Hour.H12_00, 03*60+15),
            (Hour.H08_15, Hour.H15_45, 07*60+30),
            (Hour.H07_15, Hour.H07_00, 15),
            (Hour.H19_00, Hour.H13_00, 06*60),
            (Hour.H08_30, Hour.H07_15, 01*60+15),
            (Hour.H08_45, Hour.H07_15, 01*60+30),
        })
        {
            yield return [hourA, hourB, diff];
        }
    }
}
