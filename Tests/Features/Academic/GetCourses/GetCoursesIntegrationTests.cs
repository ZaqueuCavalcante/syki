using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_all_courses_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CreateCourseOut direito = await client.CreateCourse("Direito", Tecnologo, []);
        CreateCourseOut pedagogia = await client.CreateCourse("Pedagogia", Mestrado, []);
        CreateCourseOut cc = await client.CreateCourse("Ciência da Computação", Especializacao, []);
        CreateCourseOut adm = await client.CreateCourse("Administração", Doutorado, []);
        CreateCourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado, []);

        // Act
        var courses = await client.GetCourses();

        // Assert
        courses.Total.Should().Be(5);
        courses.Items[0].Should().BeEquivalentTo(new { adm.Id, adm.Name, adm.Type });
        courses.Items[1].Should().BeEquivalentTo(new { ads.Id, ads.Name, ads.Type });
        courses.Items[2].Should().BeEquivalentTo(new { cc.Id, cc.Name, cc.Type });
        courses.Items[3].Should().BeEquivalentTo(new { direito.Id, direito.Name, direito.Type });
        courses.Items[4].Should().BeEquivalentTo(new { pedagogia.Id, pedagogia.Name, pedagogia.Type });
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
        coursesA.Result.Total.Should().Be(5);
        coursesB.Result.Total.Should().Be(5);
        coursesC.Result.Total.Should().Be(5);
    }
}
