using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using AngleSharp.Common;

namespace Syki.Tests.Unit;

public class EnumExtensionsUnitTests
{
    private enum TestEnum
    {
        WithoutDescription
    }

    [Test]
    public void Shoud_get_enum_description_when_null()
    {
        // Arrange / Act
        var result = ((Enum)null!).GetDescription();

        // Assert
        result.Should().Be("");
    }

    [Test]
    public void Shoud_get_enum_description_when_has_no_description_attribute()
    {
        // Arrange / Act
        var result = TestEnum.WithoutDescription.GetDescription();

        // Assert
        result.Should().Be("WithoutDescription");
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.TipoDeCursoEnumToDescription))]
    public void Shoud_get_enum_description((TipoDeCurso tipo, string description) data)
    {
        // Arrange / Act
        var result = data.tipo.GetDescription();

        // Assert
        result.Should().Be(data.description);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.TipoDeCursoEnumForIsIn))]
    public void Shoud_get_if_value_is_in_list((Enum value, bool isIn) data)
    {
        // Arrange / Act
        var result = data.value.IsIn(TipoDeCurso.Bacharelado, TipoDeCurso.Tecnologo);

        // Assert
        result.Should().Be(data.isIn);
    }

    [Test]
    public void Shoud_return_false_when_value_is_null()
    {
        // Arrange / Act
        var result = TipoDeCurso.Bacharelado.IsIn(null);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Shoud_return_false_when_value_is_empty()
    {
        // Arrange / Act
        var result = TipoDeCurso.Bacharelado.IsIn([]);

        // Assert
        result.Should().BeFalse();
    }
}
