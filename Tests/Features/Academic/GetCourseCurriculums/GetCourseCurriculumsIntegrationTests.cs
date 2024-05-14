namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_institution_course_curriculums()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var courseNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var gradeNovaRoma = await clientNovaRoma.CreateCourseCurriculum("NR - Grade de ADS - 1.0", courseNovaRoma.Id);

        var courseUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas");
        await clientUfpe.CreateCourseCurriculum("UFPE - Grade de ADS - 1.0", courseUfpe.Id);

        // Act
        var courseCurriculums = await clientNovaRoma.GetCourseCurriculums();

        // Assert
        courseCurriculums.Should().HaveCount(1);
        courseCurriculums[0].Id.Should().Be(gradeNovaRoma.Id);
        courseCurriculums[0].Name.Should().Be(gradeNovaRoma.Name);
    }

    [Test]
    public async Task Deve_retornar_todas_as_grades_ordenadas_por_nome()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var direito = await client.CreateCourse("Direito");
        var direitoCourseCurriculum = await client.CreateCourseCurriculum("Grade de Direito 1.0", direito.Id);

        var ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas");
        var adsCourseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", ads.Id);

        // Act
        var courseCurriculums = await client.GetCourseCurriculums();

        // Assert
        courseCurriculums.Should().HaveCount(2);
        courseCurriculums[0].Id.Should().Be(adsCourseCurriculum.Id);
        courseCurriculums[0].Name.Should().Be(adsCourseCurriculum.Name);
        courseCurriculums[1].Id.Should().Be(direitoCourseCurriculum.Id);
        courseCurriculums[1].Name.Should().Be(direitoCourseCurriculum.Name);
    }
}
