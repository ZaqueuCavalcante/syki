using static Syki.Shared.CourseType;
using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Tests.Features.Academic.CreateCourse;

public class CreateCourseUnitTests
{
    [Test]
    public void Should_convert_course_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";
        var curso = new Course(institutionId, name, Bacharelado);

        // Act
        var cursoOut = curso.ToOut();

        // Assert
        cursoOut.Id.Should().Be(curso.Id);
        cursoOut.Name.Should().Be(curso.Name);
        cursoOut.Type.Should().Be(curso.Type);
    }

    [Test]
    public void Should_return_true_when_is_the_same_course()
    {
        // Arrange
        var curso = new Course(Guid.NewGuid(), "Curso", Bacharelado);
        var cursoOut1 = curso.ToOut();
        var cursoOut2 = curso.ToOut();

        // Act
        var equals = cursoOut1.Equals(cursoOut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Should_return_false_when_is_not_the_same_course()
    {
        // Arrange
        var curso1 = new Course(Guid.NewGuid(), "Curso1", Bacharelado);
        var curso2 = new Course(Guid.NewGuid(), "Curso2", Bacharelado);
        var cursoOut1 = curso1.ToOut();
        var cursoOut2 = curso2.ToOut();

        // Act
        var equals = cursoOut1.Equals(cursoOut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_other_course_is_null()
    {
        // Arrange
        var curso = new Course(Guid.NewGuid(), "Curso1", Bacharelado);
        var cursoOut = curso.ToOut();

        // Act
        var equals = cursoOut.Equals(null);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Should_return_the_hash_code()
    {
        // Arrange
        var cursoOut = new CourseOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = cursoOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Should_return_the_course_name_as_to_string_representation()
    {
        // Arrange
        var cursoOut = new CourseOut { Name = "Curso" };

        // Act
        var name = cursoOut.ToString();

        // Assert
        name.Should().Be("Curso");
    }
}
