namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_discipline_without_course()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados");

        // Assert
        discipline.Id.Should().NotBeEmpty();
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Code.Should().NotBeEmpty();
        discipline.Courses.Should().BeEquivalentTo(new List<Guid>());
    }

    [Test]
    public async Task Should_create_discipline_with_only_one_course()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CreateCourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);

        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [course.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo([course.Id]);
    }

    [Test]
    public async Task Should_create_discipline_with_many_courses()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CreateCourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);
        CreateCourseOut cc = await client.CreateCourse("Ciência da Computação", CourseType.Bacharelado, []);

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
        var client = await _api.LoggedAsAcademic();
        await client.CreateCourse("ADS", CourseType.Tecnologo, []);

        var otherClient = await _api.LoggedAsAcademic();
        CreateCourseOut otherCourse = await otherClient.CreateCourse("ADS", CourseType.Tecnologo, []);
        
        // Act
        var discipline = await client.CreateDiscipline("Banco de Dados", [otherCourse.Id]);

        // Assert
        discipline.Name.Should().Be("Banco de Dados");
        discipline.Courses.Should().BeEquivalentTo(new List<Guid>());
    }
}
