using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;

namespace Syki.Tests.Unit;

public class PeriodosDeMatriculaUnitTests
{
    [Test]
    public void Deve_criar_um_periodo_de_matricula_com_id()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var pm = new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        pm.Id.Should().Be(id);
    }

    [Test]
    public void Deve_criar_um_periodo_de_matricula_com_faculdade_id_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var pm = new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        pm.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_periodo_de_matricula_com_start_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var pm = new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        pm.Start.Should().Be(start);
    }

    [Test]
    public void Deve_criar_um_periodo_de_matricula_com_end_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var pm = new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        pm.End.Should().Be(end);
    }

    [Test]
    public void Nao_deve_criar_um_periodo_de_matricula_com_datas_iguais()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        Action act = () => new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE1101);
    }

    [Test]
    public void Nao_deve_criar_um_periodo_de_matricula_com_datas_invalidas()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 06, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        Action act = () => new PeriodoDeMatricula(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE1101);
    }
}
