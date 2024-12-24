namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_course_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CourseOut ads = await client.CreateCourse(
            "Análise e Desenvolvimento de Sistemas",
            CourseType.Tecnologo,
            ["Banco de Dados", "Informática e Sociedade"]
        );
        CourseOut cc = await client.CreateCourse(
            "Ciência da Computação",
            CourseType.Bacharelado,
            ["Programação Orientada a Objetos", "Projeto Integrador II: Modelagem de Banco de Dados"]
        );

        // Act
        var disciplines = await client.GetCourseDisciplines(ads.Id);

        // Assert
        disciplines.Should().HaveCount(2);
        disciplines[0].Name.Should().Be("Banco de Dados");
        disciplines[1].Name.Should().Be("Informática e Sociedade");
    }
}
