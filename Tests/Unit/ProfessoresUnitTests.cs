using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using Bogus;

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
        professores.Should().HaveCount(5);
    }
}
