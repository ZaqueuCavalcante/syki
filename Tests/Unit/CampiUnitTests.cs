using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class CampiUnitTests
{
    [Test]
    public void Deve_criar_um_campus_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var campus = new Campus(faculdadeId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_campus_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var campus = new Campus(faculdadeId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_campus_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Agreste I";

        // Act
        var campus = new Campus(faculdadeId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_um_campus_com_cidade_correta()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string cidade = "Caruaru - PE";

        // Act
        var campus = new Campus(faculdadeId, "Agreste I", cidade);

        // Assert
        campus.Cidade.Should().Be(cidade);
    }

    [Test]
    public void Deve_atualizar_os_dados_do_campus_corretamente()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Agreste II";
        const string cidade = "Bonito - PE";

        var campus = new Campus(faculdadeId, "Agreste I", "Caruaru - PE");

        // Act
        campus.Update(nome, cidade);

        // Assert
        campus.Nome.Should().Be(nome);
        campus.Cidade.Should().Be(cidade);
    }

    [Test]
    public void Deve_converter_o_campus_corretamente_pro_out()
    {
        // Arrange
        var campus = new Campus(Guid.NewGuid(), "Agreste II", "Bonito - PE");

        // Act
        var campusOut = campus.ToOut();

        // Assert
        campusOut.Id.Should().Be(campus.Id);
        campusOut.Nome.Should().Be(campus.Nome);
        campusOut.Cidade.Should().Be(campus.Cidade);
    }
}
