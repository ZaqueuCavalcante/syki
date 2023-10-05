using Syki.Tests.Base;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;

namespace Syki.Tests.Unit;

public class AlunosUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidNames))]
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
