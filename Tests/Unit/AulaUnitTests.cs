using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class AulaUnitTests
{
    [Test]
    public void Deve_criar_um_aula_com_id()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now.AddHours(2);

        // Act
        var aluno = new Aula(turmaId, start, end);

        // Assert
        aluno.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_aula_com_turma_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now.AddHours(2);

        // Act
        var aluno = new Aula(turmaId, start, end);

        // Assert
        aluno.TurmaId.Should().Be(turmaId);
    }

    [Test]
    public void Deve_criar_um_aula_com_start_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now.AddHours(2);

        // Act
        var aluno = new Aula(turmaId, start, end);

        // Assert
        aluno.Start.Should().BeCloseTo(start, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Deve_criar_um_aula_com_end_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var start = DateTime.Now;
        var end = DateTime.Now.AddHours(2);

        // Act
        var aluno = new Aula(turmaId, start, end);

        // Assert
        aluno.End.Should().BeCloseTo(end, TimeSpan.FromSeconds(1));
    }
}
