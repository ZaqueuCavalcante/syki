using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Tests.Unit;

public class TurmaAlunoaUnitTests
{
    [Test]
    public void Deve_criar_um_turma_aluno_com_turma_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = StudentDisciplineStatus.Matriculado;

        // Act
        var aluno = new ClassStudent(turmaId, alunoId, situacao);

        // Assert
        aluno.TurmaId.Should().Be(turmaId);
    }

    [Test]
    public void Deve_criar_um_turma_aluno_com_aluno_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = StudentDisciplineStatus.Matriculado;

        // Act
        var aluno = new ClassStudent(turmaId, alunoId, situacao);

        // Assert
        aluno.StudentId.Should().Be(alunoId);
    }

    [Test]
    public void Deve_criar_um_turma_aluno_com_situacao_correta()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();
        var situacao = StudentDisciplineStatus.Matriculado;

        // Act
        var aluno = new ClassStudent(turmaId, alunoId, situacao);

        // Assert
        aluno.StudentDisciplineStatus.Should().Be(situacao);
    }
}
