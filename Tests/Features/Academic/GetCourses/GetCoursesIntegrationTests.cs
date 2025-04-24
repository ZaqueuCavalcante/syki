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
}
