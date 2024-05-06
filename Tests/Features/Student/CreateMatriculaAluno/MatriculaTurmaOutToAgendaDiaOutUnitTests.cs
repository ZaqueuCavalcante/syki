namespace Syki.Tests.Unit;

public class MatriculaTurmaOutToAgendaDayOutUnitTests
{
    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_um_horario()
    {
        // Arrange
        var turmas = new List<MatriculaTurmaOut>
        {
            new() { Disciplina = "Banco de Dados", Horarios = [ new(Day.Segunda, Hora.H07_00, Hora.H10_00) ] }
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hora.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hora.H10_00);
    }

    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_dois_horarios_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<MatriculaTurmaOut>
        {
            new() { Disciplina = "Banco de Dados", Horarios =
            [
                new(Day.Segunda, Hora.H10_15, Hora.H12_00),
                new(Day.Segunda, Hora.H07_00, Hora.H10_00),
            ]}
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hora.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hora.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[1].Start.Should().Be(Hora.H10_15);
        agendas[0].Disciplines[1].End.Should().Be(Hora.H12_00);
    }

    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_tres_horarios_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<MatriculaTurmaOut>
        {
            new() { Disciplina = "Banco de Dados", Horarios =
            [
                new(Day.Segunda, Hora.H10_15, Hora.H12_00),
                new(Day.Segunda, Hora.H07_00, Hora.H10_00),
                new(Day.Segunda, Hora.H15_00, Hora.H17_00),
            ]}
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hora.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hora.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[1].Start.Should().Be(Hora.H10_15);
        agendas[0].Disciplines[1].End.Should().Be(Hora.H12_00);
        agendas[0].Disciplines[2].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[2].Start.Should().Be(Hora.H15_00);
        agendas[0].Disciplines[2].End.Should().Be(Hora.H17_00);
    }

    [Test]
    public void Deve_converter_quando_existem_duas_turmas_com_um_horario_cada_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<MatriculaTurmaOut>
        {
            new() { Disciplina = "POO", Horarios = [ new(Day.Segunda, Hora.H10_00, Hora.H12_00) ] },
            new() { Disciplina = "Banco de Dados", Horarios = [ new(Day.Segunda, Hora.H07_00, Hora.H10_00) ] },
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hora.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hora.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("POO");
        agendas[0].Disciplines[1].Start.Should().Be(Hora.H10_00);
        agendas[0].Disciplines[1].End.Should().Be(Hora.H12_00);
    }
}
