namespace Syki.Tests.Extensions;

public class EnumExtensionsUnitTests
{
    [Test]
    public void Should_get_enum_description_when_null()
    {
        // Arrange / Act
        var result = ((Enum)null!).GetDescription();

        // Assert
        result.Should().Be("");
    }

    [Test]
    public void Should_get_enum_description_when_has_no_description_attribute()
    {
        // Arrange / Act
        var result = TestEnum.WithoutDescription.GetDescription();

        // Assert
        result.Should().Be("WithoutDescription");
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.CourseTypeEnumToDescription))]
    public void Should_get_enum_description((CourseType tipo, string description) data)
    {
        // Arrange / Act
        var result = data.tipo.GetDescription();

        // Assert
        result.Should().Be(data.description);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.CourseTypeEnumForIsIn))]
    public void Should_get_if_value_is_in_list((Enum value, bool isIn) data)
    {
        // Arrange / Act
        var result = data.value.IsIn(CourseType.Bacharelado, CourseType.Tecnologo);

        // Assert
        result.Should().Be(data.isIn);
    }

    [Test]
    public void Should_return_false_when_value_is_null()
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn(null);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_value_is_empty()
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn([]);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.EnumsInvalidValues))]
    public void Should_return_false_when_value_is_out_of_range(Enum value)
    {
        // Arrange / Act
        var result = CourseType.Bacharelado.IsIn(value);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.EnumsInvalidValues))]
    public void Should_return_false_when_value_is_invalid(Enum value)
    {
        // Arrange / Act
        var result = value.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.HoursDiffsInMinutes))]
    public void Should_get_hours_diff((Hour hourA, Hour hourB, int diff) data)
    {
        // Arrange / Act
        var result = data.hourA.DiffInMinutes(data.hourB);

        // Assert
        result.Should().Be(data.diff);
    }

    private enum TestEnum
    {
        WithoutDescription
    }
}
