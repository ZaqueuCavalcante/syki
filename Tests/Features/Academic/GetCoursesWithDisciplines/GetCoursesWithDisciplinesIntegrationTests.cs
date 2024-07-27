using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_all_courses_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateCourse("Pedagogia", Mestrado);
        await client.CreateCourse("Ciência da Computação", Especializacao);
        await client.CreateCourse("Administração", Doutorado);
        var direito = await client.CreateCourse("Direito", Tecnologo);
        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);

        await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        await client.CreateDiscipline("Penal", [direito.Id]);

        // Assert
        var courses = await client.GetCoursesWithDisciplines();
        courses.Should().HaveCount(2);
        courses[0].Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        courses[1].Name.Should().Be("Direito");
    }
}
