namespace Syki.Tests.Unit;

public class AulaUnitTests
{
    [Test]
    public void Deve_criar_um_aula_com_id()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var day = DateOnly.FromDateTime(DateTime.Now);
        var start = Hora.H07_00;
        var end = Hora.H10_00;

        // Act
        var aluno = new Aula(turmaId, day, start, end);

        // Assert
        aluno.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_aula_com_turma_id_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var day = DateOnly.FromDateTime(DateTime.Now);
        var start = Hora.H07_00;
        var end = Hora.H10_00;

        // Act
        var aluno = new Aula(turmaId, day, start, end);

        // Assert
        aluno.TurmaId.Should().Be(turmaId);
    }

    [Test]
    public void Deve_criar_um_aula_com_start_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var day = DateOnly.FromDateTime(DateTime.Now);
        var start = Hora.H07_00;
        var end = Hora.H10_00;

        // Act
        var aluno = new Aula(turmaId, day, start, end);

        // Assert
        aluno.Start.Should().Be(start);
    }

    [Test]
    public void Deve_criar_um_aula_com_end_correto()
    {
        // Arrange
        var turmaId = Guid.NewGuid();
        var day = DateOnly.FromDateTime(DateTime.Now);
        var start = Hora.H07_00;
        var end = Hora.H10_00;

        // Act
        var aluno = new Aula(turmaId, day, start, end);

        // Assert
        aluno.End.Should().Be(end);
    }
}
