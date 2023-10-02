using Syki.Domain;
using Syki.Exceptions;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class AlunosUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidNamesStream))]
    public void Nao_deve_criar_um_aluno_com_nome_invalido(string nome)
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        Action act = () => new Aluno(faculdadeId, nome);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
