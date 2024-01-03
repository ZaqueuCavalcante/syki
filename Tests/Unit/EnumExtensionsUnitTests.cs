using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class EnumExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.TipoDeCursoEnumToDescription))]
    public void Shoud_get_enum_description((TipoDeCurso tipo, string description) data)
    {
        // Arrange / Act
        var result = data.tipo.GetDescription();

        // Assert
        result.Should().Be(data.description);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.TipoDeCursoEnumForIsIn))]
    public void Shoud_get_if_value_is_in_list((Enum value, bool isIn) data)
    {
        // Arrange / Act
        var result = data.value.IsIn(TipoDeCurso.Bacharelado, TipoDeCurso.Tecnologo);

        // Assert
        result.Should().Be(data.isIn);
    }
}
