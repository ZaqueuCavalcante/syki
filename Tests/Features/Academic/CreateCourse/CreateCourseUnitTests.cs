using static Syki.Shared.CourseType;
using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Tests.Features.Academic.CreateCourse;

public class CreateCourseUnitTests
{
    [Test]
    public void Should_convert_course_to_out()
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        const string name = "Análise e Desenvolvimento de Sistemas";
        var course = new Course(institutionId, name, Bacharelado);

        // Act
        var courseOut = course.ToOut();

        // Assert
        courseOut.Id.Should().Be(course.Id);
        courseOut.Name.Should().Be(course.Name);
        courseOut.Type.Should().Be(course.Type);
    }

    [Test]
    public void Should_return_true_when_is_the_same_course()
    {
        // Arrange
        var course = new Course(Guid.CreateVersion7(), "Curso", Bacharelado);
        var courseOut1 = course.ToOut();
        var courseOut2 = course.ToOut();

        // Act
        var equals = courseOut1.Equals(courseOut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Should_return_false_when_is_not_the_same_course()
    {
        // Arrange
        var course1 = new Course(Guid.CreateVersion7(), "Course1", Bacharelado);
        var course2 = new Course(Guid.CreateVersion7(), "Course2", Bacharelado);
        var courseOut1 = course1.ToOut();
        var courseOut2 = course2.ToOut();

        // Act
        var equals = courseOut1.Equals(courseOut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_other_course_is_null()
    {
        // Arrange
        var course = new Course(Guid.CreateVersion7(), "Course1", Bacharelado);
        var courseOut = course.ToOut();

        // Act
        var equals = courseOut.Equals(null);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Should_return_the_hash_code()
    {
        // Arrange
        var courseOut = new CourseOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = courseOut.GetHashCode();

        // Assert
        hashCode.Should().Be(9473_3118);
    }

    [Test]
    public void Should_return_the_course_name_as_to_string_representation()
    {
        // Arrange
        var courseOut = new CourseOut { Name = "Direito" };

        // Act
        var name = courseOut.ToString();

        // Assert
        name.Should().Be("Direito");
    }
}
