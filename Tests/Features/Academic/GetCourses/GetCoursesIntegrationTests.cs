using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_all_courses_ordered_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateCourse("Direito", Tecnologo);
        await client.CreateCourse("Pedagogia", Mestrado);
        await client.CreateCourse("Ciência da Computação", Especializacao);
        await client.CreateCourse("Administração", Doutorado);
        await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        var courses = await client.GetCourses();
        courses.Should().HaveCount(5);
        courses[0].Name.Should().Be("Administração");
        courses[1].Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        courses[2].Name.Should().Be("Ciência da Computação");
        courses[3].Name.Should().Be("Direito");
        courses[4].Name.Should().Be("Pedagogia");
    }
}
