using Syki.Shared;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class AlunoDisciplinaUnitTests
{
    [Test]
    public void Deve_criar_um_aluno_disciplina_com_id()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var situacao = Situacao.Cursando;

        // Act
        var aluno = new AlunoDisciplina(alunoId, disciplinaId, situacao);

        // Assert
        aluno.Id.Should().BeEmpty();
    }

    [Test]
    public void Deve_criar_um_aluno_disciplina_com_aluno_id_correto()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var situacao = Situacao.Cursando;

        // Act
        var aluno = new AlunoDisciplina(alunoId, disciplinaId, situacao);

        // Assert
        aluno.AlunoId.Should().Be(alunoId);
    }

    [Test]
    public void Deve_criar_um_aluno_disciplina_com_disciplina_id_correto()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var situacao = Situacao.Cursando;

        // Act
        var aluno = new AlunoDisciplina(alunoId, disciplinaId, situacao);

        // Assert
        aluno.DisciplinaId.Should().Be(disciplinaId);
    }

    [Test]
    public void Deve_criar_um_aluno_disciplina_com_situacao_correta()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var situacao = Situacao.Cursando;

        // Act
        var aluno = new AlunoDisciplina(alunoId, disciplinaId, situacao);

        // Assert
        aluno.Situacao.Should().Be(situacao);
    }
}
