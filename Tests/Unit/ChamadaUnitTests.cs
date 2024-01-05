using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class ChamadaUnitTests
{
    [Test]
    public void Deve_criar_uma_chamada_com_aula_id_correto()
    {
        // Arrange
        var aulaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var presente = true;

        // Act
        var chamada = new Chamada(aulaId, alunoId, presente);

        // Assert
        chamada.AulaId.Should().Be(aulaId);
    }

    [Test]
    public void Deve_criar_uma_chamada_com_aluno_id_correto()
    {
        // Arrange
        var aulaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var presente = true;

        // Act
        var chamada = new Chamada(aulaId, alunoId, presente);

        // Assert
        chamada.AlunoId.Should().Be(alunoId);
    }

    [Test]
    public void Deve_criar_uma_chamada_com_presente_correto()
    {
        // Arrange
        var aulaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var presente = true;

        // Act
        var chamada = new Chamada(aulaId, alunoId, presente);

        // Assert
        chamada.Presente.Should().Be(presente);
    }
}
