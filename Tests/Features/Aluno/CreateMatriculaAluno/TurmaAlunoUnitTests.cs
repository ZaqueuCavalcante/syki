namespace Syki.Tests.Unit;

public class TurmaAlunoaUnitTests
{
    [Test]
    public void Deve_criar_um_turma_aluno_com_turma_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = Situacao.Matriculado;

        // Act
        var aluno = new TurmaAluno(turmaId, alunoId, situacao);

        // Assert
        aluno.TurmaId.Should().Be(turmaId);
    }

    [Test]
    public void Deve_criar_um_turma_aluno_com_aluno_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = Situacao.Matriculado;

        // Act
        var aluno = new TurmaAluno(turmaId, alunoId, situacao);

        // Assert
        aluno.AlunoId.Should().Be(alunoId);
    }

    [Test]
    public void Deve_criar_um_turma_aluno_com_situacao_correta()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = Situacao.Matriculado;

        // Act
        var aluno = new TurmaAluno(turmaId, alunoId, situacao);

        // Assert
        aluno.Situacao.Should().Be(situacao);
    }
}
