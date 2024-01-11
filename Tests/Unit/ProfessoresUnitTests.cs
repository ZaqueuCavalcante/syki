using Bogus;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class ProfessoresUnitTests
{
    [Test]
    public void Deve_criar_um_professor_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string nome = "Chico Science";

        // Act
        var professor = new Professor(faculdadeId, userId, nome);

        // Assert
        professor.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_professor_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string nome = "Chico Science";

        // Act
        var professor = new Professor(faculdadeId, userId, nome);

        // Assert
        professor.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_professor_com_user_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string nome = "Chico Science";

        // Act
        var professor = new Professor(faculdadeId, userId, nome);

        // Assert
        professor.UserId.Should().Be(userId);
    }

    [Test]
    public void Deve_criar_um_professor_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string nome = "Chico Science";

        // Act
        var professor = new Professor(faculdadeId, userId, nome);

        // Assert
        professor.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_varios_professores()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var faker = new Faker<Professor>()
            .CustomInstantiator(f => new Professor(faculdadeId, userId, f.Person.FirstName));

        // Act
        var professores = faker.Generate(5);

        // Assert
        professores.ConvertAll(x => x.Id).Should().OnlyHaveUniqueItems();
    }

    [Test]
    public void Deve_converter_o_professor_corretamente_pro_out()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string nome = "Chico Science";
        var professor = new Professor(faculdadeId, userId, nome);

        // Act
        var professorOut = professor.ToOut();

        // Assert
        professorOut.Id.Should().Be(professor.Id);
        professorOut.Nome.Should().Be(professor.Nome);
    }
}
