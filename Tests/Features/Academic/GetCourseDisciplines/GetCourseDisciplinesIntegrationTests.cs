namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_course_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCourse("Ciência da Computação");

        await client.CreateDiscipline("Banco de Dados", [ads.Id, cc.Id]);
        await client.CreateDiscipline("Informática e Sociedade", [ads.Id]);
        await client.CreateDiscipline("Programação Orientada a Objetos", [cc.Id]);
        await client.CreateDiscipline("Projeto Integrador II: Modelagem de Banco de Dados", [cc.Id]);

        // Act
        var disciplines = await client.GetCourseDisciplines(ads.Id);

        // Assert
        disciplines.Should().HaveCount(2);
        disciplines[0].Name.Should().Be("Banco de Dados");
        disciplines[1].Name.Should().Be("Informática e Sociedade");
    }
}
