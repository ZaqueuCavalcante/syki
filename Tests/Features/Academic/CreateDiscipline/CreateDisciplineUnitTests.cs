using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Tests.Features.Academic.CreateDiscipline;

public class CreateDisciplineUnitTests
{
    [Test]
    public void Should_convert_discipline_to_out()
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
    public void Should_return_true_when_is_the_same_discipline()
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
    public void Should_return_false_when_is_not_the_same_discipline()
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
    public void Should_return_false_when_other_discipline_is_null()
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
    public void Should_return_the_hash_code()
    {
        // Arrange
        var disciplineOut = new DisciplineOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = disciplineOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Should_return_the_discipline_name_as_to_string_representation()
    {
        // Arrange
        var disciplineOut = new DisciplineOut { Name = "Banco de Dados" };

        // Act
        var name = disciplineOut.ToString();

        // Assert
        name.Should().Be("Banco de Dados");
    }
}
