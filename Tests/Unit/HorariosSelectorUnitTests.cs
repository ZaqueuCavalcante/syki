using Syki.Shared;
using NUnit.Framework;
using FluentAssertions;
using Syki.Front.Domain;

namespace Syki.Tests.Unit;

public class HorariosSelectorUnitTests
{
    [Test]
    public void Deve_criar_as_opcoes_de_horas_com_valores_corretos()
    {
        // Arrange / Act
        var options = new HorasOptions();

        // Assert
        options.Starts.Should().NotContain(Hora.H23_00);
        options.Ends.Should().NotContain(Hora.H07_00);
    }

    [Test]
    public void Deve_criar_o_horarios_selector_com_valores_corretos()
    {
        // Arrange / Act
        var selector = new HorariosSelector();

        // Assert
        selector.Options.Should().ContainKey(Dia.Segunda);
        selector.Options.Should().ContainKey(Dia.Terca);
        selector.Options.Should().ContainKey(Dia.Quarta);
        selector.Options.Should().ContainKey(Dia.Quinta);
        selector.Options.Should().ContainKey(Dia.Sexta);
        selector.Options.Should().ContainKey(Dia.Sabado);
    }

    [Test]
    public void Deve_selecionar_todos_os_horarios_de_um_dia()
    {
        // Arrange
        var selector = new HorariosSelector();
        var horario = new HorarioIn(Dia.Terca, Hora.H07_00, Hora.H23_00);

        // Act
        var result = selector.Select(horario);

        // Assert
        result.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horario);
        selector.Options[horario.Dia].Starts.Should().BeEmpty();
        selector.Options[horario.Dia].Ends.Should().BeEmpty();
    }

    [Test]
    public void Deve_retornar_todos_os_dias()
    {
        // Arrange
        var selector = new HorariosSelector();
        var dias = Enum.GetValues<Dia>();

        // Act
        var result = selector.GetDias();

        // Assert
        result.Should().BeEquivalentTo(dias);
    }

    [Test]
    public void Nao_deve_retornar_um_dia_sem_opcoes_de_hora()
    {
        // Arrange
        var selector = new HorariosSelector();
        var horario = new HorarioIn(Dia.Terca, Hora.H07_00, Hora.H23_00);
        selector.Select(horario);

        // Act
        var result = selector.GetDias();

        // Assert
        result.Should().NotContain(horario.Dia);
    }

    [Test]
    public void Nao_deve_retornar_dias_sem_opcoes_de_hora()
    {
        // Arrange
        var selector = new HorariosSelector();
        var horarioA = new HorarioIn(Dia.Terca, Hora.H07_00, Hora.H23_00);
        var horarioB = new HorarioIn(Dia.Quinta, Hora.H07_00, Hora.H23_00);

        selector.Select(horarioA);
        selector.Select(horarioB);

        // Act
        var result = selector.GetDias();

        // Assert
        result.Should().NotContain([horarioA.Dia, horarioB.Dia]);
    }

    [Test]
    public void Deve_selecionar_um_horario_no_inicio()
    {
        // Arrange
        var selector = new HorariosSelector();
        var dia = Dia.Segunda;
        var start = Hora.H07_00;
        var end = Hora.H08_00;
        var horario = new HorarioIn(dia, start, end);

        // Act
        var result = selector.Select(horario);

        // Assert
        result.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horario);
        selector.Options[dia].Starts.Should().NotContain([start, Hora.H07_15, Hora.H07_30, Hora.H07_45]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H07_15, Hora.H07_30, Hora.H07_45, end]);
    }

    [Test]
    public void Deve_selecionar_um_horario_no_meio()
    {
        // Arrange
        var selector = new HorariosSelector();
        var dia = Dia.Segunda;
        var start = Hora.H09_30;
        var end = Hora.H10_45;
        var horario = new HorarioIn(dia, start, end);

        // Act
        var result = selector.Select(horario);

        // Assert
        result.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horario);
        selector.Options[dia].Starts.Should().NotContain([start, Hora.H09_45, Hora.H10_00, Hora.H10_15, Hora.H10_30]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H09_45, Hora.H10_00, Hora.H10_15, Hora.H10_30, end]);
    }

    [Test]
    public void Deve_selecionar_um_horario_no_final()
    {
        // Arrange
        var selector = new HorariosSelector();
        var dia = Dia.Segunda;
        var start = Hora.H22_00;
        var end = Hora.H23_00;
        var horario = new HorarioIn(dia, start, end);

        // Act
        var result = selector.Select(horario);

        // Assert
        result.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horario);
        selector.Options[dia].Starts.Should().NotContain([start, Hora.H22_15, Hora.H22_30, Hora.H22_45]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H22_15, Hora.H22_30, Hora.H22_45, end]);
    }

    [Test]
    public void Deve_selecionar_dois_horarios_consecutivos_do_inicio()
    {
        // Arrange
        var selector = new HorariosSelector();

        var dia = Dia.Segunda;
        var startA = Hora.H07_00;
        var endA = Hora.H08_00;
        var startB = Hora.H08_00;
        var endB = Hora.H08_30;
        var horarioA = new HorarioIn(dia, startA, endA);
        var horarioB = new HorarioIn(dia, startB, endB);

        // Act
        var resultA = selector.Select(horarioA);
        var resultB = selector.Select(horarioB);

        // Assert
        resultA.Should().BeTrue();
        resultB.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horarioA);
        selector.Values.Should().ContainEquivalentOf(horarioB);
        selector.Options[dia].Starts.Should().NotContain([startA, Hora.H07_15, Hora.H07_30, Hora.H07_45, Hora.H08_00, Hora.H08_15]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H07_15, Hora.H07_30, Hora.H07_45, Hora.H08_00, Hora.H08_15, endB]);
    }

    [Test]
    public void Deve_selecionar_dois_horarios_consecutivos_do_meio()
    {
        // Arrange
        var selector = new HorariosSelector();

        var dia = Dia.Segunda;
        var startA = Hora.H07_15;
        var endA = Hora.H08_00;
        var startB = Hora.H08_00;
        var endB = Hora.H08_15;
        var horarioA = new HorarioIn(dia, startA, endA);
        var horarioB = new HorarioIn(dia, startB, endB);

        // Act
        var resultA = selector.Select(horarioA);
        var resultB = selector.Select(horarioB);

        // Assert
        resultA.Should().BeTrue();
        resultB.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horarioA);
        selector.Values.Should().ContainEquivalentOf(horarioB);
        selector.Options[dia].Starts.Should().NotContain([startA, Hora.H07_30, Hora.H07_45, startB]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H07_30, Hora.H07_45, Hora.H08_00, endB]);
    }

    [Test]
    public void Deve_selecionar_dois_horarios_consecutivos_do_final()
    {
        // Arrange
        var selector = new HorariosSelector();

        var dia = Dia.Segunda;
        var startA = Hora.H21_30;
        var endA = Hora.H22_30;
        var startB = Hora.H22_30;
        var endB = Hora.H23_00;
        var horarioA = new HorarioIn(dia, startA, endA);
        var horarioB = new HorarioIn(dia, startB, endB);

        // Act
        var resultA = selector.Select(horarioA);
        var resultB = selector.Select(horarioB);

        // Assert
        resultA.Should().BeTrue();
        resultB.Should().BeTrue();
        selector.Values.Should().ContainEquivalentOf(horarioA);
        selector.Values.Should().ContainEquivalentOf(horarioB);
        selector.Options[dia].Starts.Should().NotContain([startA, Hora.H21_45, Hora.H22_00, Hora.H22_15, startB, Hora.H22_45]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H21_45, Hora.H22_00, Hora.H22_15, startB, Hora.H22_45, endB]);
    }

    [Test]
    public void Nao_deve_selecionar_dois_horarios_conflitantes_do_inicio()
    {
        // Arrange
        var selector = new HorariosSelector();

        var dia = Dia.Segunda;
        var startA = Hora.H07_00;
        var endA = Hora.H08_00;
        var startB = Hora.H07_30;
        var endB = Hora.H08_30;
        var horarioA = new HorarioIn(dia, startA, endA);
        var horarioB = new HorarioIn(dia, startB, endB);

        // Act
        var resultA = selector.Select(horarioA);
        var resultB = selector.Select(horarioB);

        // Assert
        resultA.Should().BeTrue();
        resultB.Should().BeFalse();
        selector.Values.Should().ContainEquivalentOf(horarioA);
        selector.Values.Should().NotContainEquivalentOf(horarioB);
        selector.Options[dia].Starts.Should().NotContain([startA, Hora.H07_15, Hora.H07_30, Hora.H07_45]);
        selector.Options[dia].Ends.Should().NotContain([Hora.H07_15, Hora.H07_30, Hora.H07_45, endA]);
    }
}
