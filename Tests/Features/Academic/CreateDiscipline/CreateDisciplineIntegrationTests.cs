namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_new_discipline_without_course()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados");

        // Assert
        discipline.Id.Should().NotBeEmpty();
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Code.Should().NotBeEmpty();
        discipline.Courses.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Should_create_a_new_discipline_with_only_one_course()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var curso = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [curso.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([curso.Id]);
    }

    [Test]
    public async Task Should_create_a_new_discipline_with_many_courses()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCourse("Ciência da Computação");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [ads.Id, cc.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Should_create_many_disciplines()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateDiscipline("Banco de Dados");
        await client.CreateDiscipline("Estrutura de Dados");
        await client.CreateDiscipline("Programação Orientada a Objetos");

        // Assert
        var disciplines = await client.GetDisciplines();
        disciplines.Should().HaveCount(3);
    }
}
