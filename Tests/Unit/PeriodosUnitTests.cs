using NUnit.Framework;
using Syki.Tests.Base;
using Syki.Back.Domain;
using FluentAssertions;
using Syki.Back.Exceptions;

namespace Syki.Tests.Unit;

public class PeriodosUnitTests
{
    [Test]
    public void Deve_criar_um_periodo_com_id()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var periodo = new Periodo(id, faculdadeId, start, end);

        // Assert
        periodo.Id.Should().Be(id);
    }

    [Test]
    public void Deve_criar_um_periodo_com_faculdade_id_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var periodo = new Periodo(id, faculdadeId, start, end);

        // Assert
        periodo.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_periodo_com_start_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var periodo = new Periodo(id, faculdadeId, start, end);

        // Assert
        periodo.Start.Should().Be(start);
    }

    [Test]
    public void Deve_criar_um_periodo_com_end_correto()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var periodo = new Periodo(id, faculdadeId, start, end);

        // Assert
        periodo.End.Should().Be(end);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPeriods))]
    public void Nao_deve_criar_um_periodo_com_id_invalido(string id)
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0003);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidPeriods))]
    public void Deve_criar_um_periodo_com_id_valido(string id)
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var year = int.Parse(id.Substring(0, 4));
        var start = new DateOnly(year, 02, 01);
        var end = new DateOnly(year, 06, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().NotThrow<DomainException>();
    }

    [Test]
    public void Nao_deve_criar_um_periodo_com_start_invalido()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2022, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0004);
    }

    [Test]
    public void Nao_deve_criar_um_periodo_com_end_invalido()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2024, 06, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0005);
    }

    [Test]
    public void Nao_deve_criar_um_periodo_com_datas_iguais()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0006);
    }

    [Test]
    public void Nao_deve_criar_um_periodo_com_datas_invalidas()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 06, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        Action act = () => new Periodo(id, faculdadeId, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(ExceptionMessages.DE0006);
    }

    [Test]
    public void Deve_converter_o_periodo_corretamente_pro_out()
    {
        // Arrange
        const string id = "2023.1";
        var faculdadeId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        var periodo = new Periodo(id, faculdadeId, start, end);

        // Act
        var periodoOut = periodo.ToOut();

        // Assert
        periodoOut.Id.Should().Be(periodo.Id);
        periodoOut.Start.Should().Be(start);
        periodoOut.End.Should().Be(end);
    }
}
