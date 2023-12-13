using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class DisciplinasUnitTests
{
    [Test]
    public void Deve_criar_uma_disciplina_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var disciplina = new Disciplina(faculdadeId, "Banco de Dados", 72);

        // Assert
        disciplina.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_disciplina_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var disciplina = new Disciplina(faculdadeId, "Banco de Dados", 72);

        // Assert
        disciplina.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_uma_disciplina_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Banco de Dados";

        // Act
        var disciplina = new Disciplina(faculdadeId, nome, 72);

        // Assert
        disciplina.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_uma_disciplina_com_carga_horaria_correta()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Banco de Dados";

        // Act
        var disciplina = new Disciplina(faculdadeId, nome, 72);

        // Assert
        disciplina.CargaHoraria.Should().Be(72);
    }

    [Test]
    public void Deve_converter_corretamente_pro_out()
    {
        // Arrange
        var disciplina = new Disciplina(Guid.NewGuid(), "Banco de Dados", 72);
        disciplina.Vinculos.Add(new() { CursoId = Guid.NewGuid() });
        disciplina.Vinculos.Add(new() { CursoId = Guid.NewGuid() });

        // Act
        var disciplinaOut = disciplina.ToOut();

        // Assert
        disciplinaOut.Id.Should().Be(disciplina.Id);
        disciplinaOut.Nome.Should().Be(disciplina.Nome);
        disciplinaOut.CargaHoraria.Should().Be(disciplina.CargaHoraria);
        disciplinaOut.Cursos.Should().BeEquivalentTo(disciplina.Vinculos.ConvertAll(x => x.CursoId));
    }
}
