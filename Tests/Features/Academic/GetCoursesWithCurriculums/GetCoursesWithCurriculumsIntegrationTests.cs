using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_all_courses_with_curriculums()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        await client.CreateCourse("Pedagogia", Mestrado, []);
        await client.CreateCourse("Ciência da Computação", Especializacao, []);
        await client.CreateCourse("Administração", Doutorado, []);
        CourseOut ads = await client.CreateCourse("Direito", Tecnologo, ["Grade de Direito 1.0"]);
        CourseOut direito = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado, ["Grade de ADS 1.0"]);

        await client.CreateCourseCurriculum("Grade de ADS 1.0", ads.Id);
        await client.CreateCourseCurriculum("Grade de Direito 1.0", direito.Id);

        // Assert
        var courses = await client.GetCoursesWithCurriculums();
        courses.Should().HaveCount(2);
        courses[0].Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        courses[1].Name.Should().Be("Direito");
    }
}
