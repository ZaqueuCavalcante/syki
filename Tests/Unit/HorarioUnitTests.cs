using Syki.Shared;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class HorarioUnitTests
{
    [Test]
    public void Deve_criar_um_horario_com_id()
    {
        // Arrange
        var dia = Dia.Segunda;

        // Act
        var horario = new Horario(dia, []);

        // Assert
        horario.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_horario_com_dia_correto()
    {
        // Arrange
        var dia = Dia.Terca;

        // Act
        var horario = new Horario(dia, []);

        // Assert
        horario.Dia.Should().Be(dia);
    }

    [Test]
    public void Deve_criar_um_horario_com_horas_corretas()
    {
        // Arrange
        var dia = Dia.Quarta;
        var horas = new List<Hora>() { Hora.H07, Hora.H08, Hora.H09 };

        // Act
        var horario = new Horario(dia, horas);

        // Assert
        horario.Horas.Should().BeEquivalentTo(horas);
    }
}
