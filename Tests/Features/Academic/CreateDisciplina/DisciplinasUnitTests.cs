using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Tests.Unit;

public class DisciplinesUnitTests
{
    [Test]
    public void Deve_criar_uma_discipline_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var discipline = new Discipline(institutionId, "Banco de Dados");

        // Assert
        discipline.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_discipline_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var discipline = new Discipline(institutionId, "Banco de Dados");

        // Assert
        discipline.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_uma_discipline_com_nome_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Banco de Dados";

        // Act
        var discipline = new Discipline(institutionId, name);

        // Assert
        discipline.Name.Should().Be(name);
    }

    [Test]
    public void Deve_converter_a_discipline_corretamente_pro_out()
    {
        // Arrange
        var discipline = new Discipline(Guid.NewGuid(), "Banco de Dados");
        discipline.Links.Add(new() { CourseId = Guid.NewGuid() });
        discipline.Links.Add(new() { CourseId = Guid.NewGuid() });

        // Act
        var disciplineOut = discipline.ToOut();

        // Assert
        disciplineOut.Id.Should().Be(discipline.Id);
        disciplineOut.Name.Should().Be(discipline.Name);
        disciplineOut.Courses.Should().BeEquivalentTo(discipline.Links.ConvertAll(x => x.CourseId));
    }

    [Test]
    public void Deve_retornar_true_quando_for_a_mesma_discipline()
    {
        // Arrange
        var discipline = new Discipline(Guid.NewGuid(), "Banco de Dados");
        var disciplineOut1 = discipline.ToOut();
        var disciplineOut2 = discipline.ToOut();

        // Act
        var equals = disciplineOut1.Equals(disciplineOut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Deve_retornar_false_quando_nao_for_a_mesma_discipline()
    {
        // Arrange
        var discipline1 = new Discipline(Guid.NewGuid(), "Banco de Dados");
        var discipline2 = new Discipline(Guid.NewGuid(), "Banco de Dados");
        var disciplineOut1 = discipline1.ToOut();
        var disciplineOut2 = discipline2.ToOut();

        // Act
        var equals = disciplineOut1.Equals(disciplineOut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_false_quando_a_outra_discipline_for_nula()
    {
        // Arrange
        var discipline = new Discipline(Guid.NewGuid(), "Banco de Dados");
        var disciplineOut = discipline.ToOut();

        // Act
        var equals = disciplineOut.Equals(null);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_o_hash_code_correto()
    {
        // Arrange
        var disciplineOut = new DisciplineOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = disciplineOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Deve_retornar_o_nome_da_discipline_como_to_string()
    {
        // Arrange
        var disciplineOut = new DisciplineOut { Name = "Banco de Dados" };

        // Act
        var name = disciplineOut.ToString();

        // Assert
        name.Should().Be("Banco de Dados");
    }
}
