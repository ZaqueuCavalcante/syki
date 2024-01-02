using Bogus;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class ProfessoresUnitTests
{
    [Test]
    public void Deve_criar_um_professor()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Chico Science";

        // Act
        var professor = new Professor(faculdadeId, nome);

        // Assert
        professor.FaculdadeId.Should().Be(faculdadeId);
        professor.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_varios_professores()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        var faker = new Faker<Professor>()
            .CustomInstantiator(f => new Professor(faculdadeId, f.Person.FirstName));

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
        const string nome = "Chico Science";
        var professor = new Professor(faculdadeId, nome);

        // Act
        var professorOut = professor.ToOut();

        // Assert
        professorOut.Id.Should().Be(professor.Id);
        professorOut.Nome.Should().Be(professor.Nome);
    }
}
