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
        act.Should().Throw<DomainException>().WithMessage(Throw.DE0018);
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
        act.Should().Throw<DomainException>().WithMessage(Throw.DE0018);
    }
}
