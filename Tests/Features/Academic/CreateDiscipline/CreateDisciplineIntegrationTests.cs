namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_a_new_discipline_without_course()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

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
        var client = await _back.LoggedAsAcademic();
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [course.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([course.Id]);
    }

    [Test]
    public async Task Should_create_a_new_discipline_with_many_courses()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var cc = await client.CreateCourse("Ciência da Computação");

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [ads.Id, cc.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([cc.Id, ads.Id]);
    }

    [Test]
    public async Task Should_not_link_other_institution_course_to_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateCourse("ADS");

        var otherClient = await _back.LoggedAsAcademic();
        var otherCourse = await otherClient.CreateCourse("ADS");
        
        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [otherCourse.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo(new List<Guid>());
    }
}
