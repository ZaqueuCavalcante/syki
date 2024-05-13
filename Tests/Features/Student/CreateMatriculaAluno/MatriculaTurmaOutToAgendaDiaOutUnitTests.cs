namespace Syki.Tests.Unit;

public class MatriculaClassOutToAgendaDayOutUnitTests
{
    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_um_schedule()
    {
        // Arrange
        var turmas = new List<EnrollmentClassOut>
        {
            new() { Discipline = "Banco de Dados", Schedules = [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ] }
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hour.H10_00);
    }

    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_dois_schedules_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<EnrollmentClassOut>
        {
            new() { Discipline = "Banco de Dados", Schedules =
            [
                new(Day.Segunda, Hour.H10_15, Hour.H12_00),
                new(Day.Segunda, Hour.H07_00, Hour.H10_00),
            ]}
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hour.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[1].Start.Should().Be(Hour.H10_15);
        agendas[0].Disciplines[1].End.Should().Be(Hour.H12_00);
    }

    [Test]
    public void Deve_converter_quando_so_existe_uma_turma_com_tres_schedules_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<EnrollmentClassOut>
        {
            new() { Discipline = "Banco de Dados", Schedules =
            [
                new(Day.Segunda, Hour.H10_15, Hour.H12_00),
                new(Day.Segunda, Hour.H07_00, Hour.H10_00),
                new(Day.Segunda, Hour.H15_00, Hour.H17_00),
            ]}
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hour.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[1].Start.Should().Be(Hour.H10_15);
        agendas[0].Disciplines[1].End.Should().Be(Hour.H12_00);
        agendas[0].Disciplines[2].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[2].Start.Should().Be(Hour.H15_00);
        agendas[0].Disciplines[2].End.Should().Be(Hour.H17_00);
    }

    [Test]
    public void Deve_converter_quando_existem_duas_turmas_com_um_schedule_cada_no_mesmo_dia()
    {
        // Arrange
        var turmas = new List<EnrollmentClassOut>
        {
            new() { Discipline = "POO", Schedules = [ new(Day.Segunda, Hour.H10_00, Hour.H12_00) ] },
            new() { Discipline = "Banco de Dados", Schedules = [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ] },
        };

        // Act
        var agendas = turmas.ToAgendas();

        // Assert
        agendas.Should().ContainSingle();
        agendas[0].Day.Should().Be(Day.Segunda);
        agendas[0].Disciplines[0].Name.Should().Be("Banco de Dados");
        agendas[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        agendas[0].Disciplines[0].End.Should().Be(Hour.H10_00);
        agendas[0].Disciplines[1].Name.Should().Be("POO");
        agendas[0].Disciplines[1].Start.Should().Be(Hour.H10_00);
        agendas[0].Disciplines[1].End.Should().Be(Hour.H12_00);
    }
}
