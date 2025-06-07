using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_all_courses_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CourseOut direito = await client.CreateCourse("Direito", Tecnologo, []);
        CourseOut pedagogia = await client.CreateCourse("Pedagogia", Mestrado, []);
        CourseOut cc = await client.CreateCourse("Ciência da Computação", Especializacao, []);
        CourseOut adm = await client.CreateCourse("Administração", Doutorado, []);
        CourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado, []);

        // Act
        var courses = await client.GetCourses();

        // Assert
        courses.Should().HaveCount(5);
        courses[0].Should().BeEquivalentTo(adm);
        courses[1].Should().BeEquivalentTo(ads);
        courses[2].Should().BeEquivalentTo(cc);
        courses[3].Should().BeEquivalentTo(direito);
        courses[4].Should().BeEquivalentTo(pedagogia);
    }

    [Test]
    public async Task Should_get_courses_in_parallel_requests_to_test_cache_stampede()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        await client.CreateCourse("Direito", Tecnologo, []);
        await client.CreateCourse("Pedagogia", Mestrado, []);
        await client.CreateCourse("Administração", Doutorado, []);
        await client.CreateCourse("Ciência da Computação", Especializacao, []);
        await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado, []);

        // Act
        var coursesA = client.GetCourses();
        var coursesB = client.GetCourses();
        var coursesC = client.GetCourses();

        await Task.WhenAll(coursesA, coursesB, coursesC);

        // Assert
        coursesA.Result.Should().HaveCount(5);
        coursesB.Result.Should().HaveCount(5);
        coursesC.Result.Should().HaveCount(5);
    }
}
