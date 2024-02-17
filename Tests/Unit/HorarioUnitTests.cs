using Syki.Shared;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;

namespace Syki.Tests.Unit;

public class HorarioUnitTests
{
    [Test]
    public void Deve_criar_um_horario_com_id()
    {
        // Arrange
        var dia = Dia.Segunda;
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        // Act
        var horario = new Horario(dia, start, end);

        // Assert
        horario.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_horario_com_dia_correto()
    {
        // Arrange
        var dia = Dia.Terca;
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        // Act
        var horario = new Horario(dia, start, end);

        // Assert
        horario.Dia.Should().Be(dia);
    }

    [Test]
    public void Deve_criar_um_horario_com_start_correto()
    {
        // Arrange
        var dia = Dia.Terca;
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        // Act
        var horario = new Horario(dia, start, end);

        // Assert
        horario.Start.Should().Be(start);
    }

    [Test]
    public void Deve_criar_um_horario_com_end_correto()
    {
        // Arrange
        var dia = Dia.Terca;
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        // Act
        var horario = new Horario(dia, start, end);

        // Assert
        horario.End.Should().Be(end);
    }

    [Test]
    public void Nao_deve_criar_um_horario_quando_start_e_end_forem_iguais()
    {
        // Arrange
        var dia = Dia.Terca;
        var start = Hora.H07_00;
        var end = Hora.H07_00;

        // Act
        Action act = () => new Horario(dia, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE021);
    }

    [Test]
    public void Nao_deve_criar_um_horario_quando_end_for_menor_que_start()
    {
        // Arrange
        var dia = Dia.Terca;
        var start = Hora.H10_00;
        var end = Hora.H07_00;

        // Act
        Action act = () => new Horario(dia, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE021);
    }

    [Test]
    public void Horarios_em_dias_diferentes_nao_devem_conflitar()
    {
        // Arrange
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        var horarioA = new Horario(Dia.Segunda, start, end);
        var horarioB = new Horario(Dia.Terca, start, end);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Horarios_validos_nao_devem_conflitar()
    {
        // Arrange
        var horarioA = new Horario(Dia.Segunda, Hora.H07_00, Hora.H08_00);
        var horarioB = new Horario(Dia.Segunda, Hora.H08_00, Hora.H08_30);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Horarios_exatamente_iguais_devem_conflitar()
    {
        // Arrange
        var start = Hora.H07_00;
        var end = Hora.H08_00;

        var horarioA = new Horario(Dia.Segunda, start, end);
        var horarioB = new Horario(Dia.Segunda, start, end);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Horarios_parcialmente_iguais_devem_conflitar()
    {
        // Arrange
        var horarioA = new Horario(Dia.Segunda, Hora.H07_00, Hora.H08_00);
        var horarioB = new Horario(Dia.Segunda, Hora.H07_30, Hora.H08_30);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Horarios_cujo_segundo_esta_contido_no_primeiro_devem_conflitar()
    {
        // Arrange
        var horarioA = new Horario(Dia.Segunda, Hora.H07_00, Hora.H08_00);
        var horarioB = new Horario(Dia.Segunda, Hora.H07_30, Hora.H07_45);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Horarios_cujo_primeiro_esta_contido_no_segundo_devem_conflitar()
    {
        // Arrange
        var horarioA = new Horario(Dia.Segunda, Hora.H10_00, Hora.H11_00);
        var horarioB = new Horario(Dia.Segunda, Hora.H09_30, Hora.H12_15);

        // Act
        var result = horarioA.Conflict(horarioB);

        // Assert
        result.Should().BeTrue();
    }
}
