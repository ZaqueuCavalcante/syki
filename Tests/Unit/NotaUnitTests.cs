using NUnit.Framework;
using Syki.Tests.Base;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class NotaUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.Notas))]
    public void Deve_retornar_a_nota((decimal nota1, decimal nota2, decimal notaFinal, decimal output) data)
    {
        // Arrange / Act
        var result = Nota.Get(data.nota1, data.nota2, data.notaFinal);

        // Assert
        result.Should().Be(data.output);
    }
}
