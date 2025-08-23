namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_only_institution_course_curriculums()
    {
        // Arrange
        var clientNovaRoma = await _api.LoggedAsAcademic();
        var clientUfpe = await _api.LoggedAsAcademic();

        CreateCourseOut courseNovaRoma = await clientNovaRoma.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);
        CourseCurriculumOut courseCurriculumNovaRoma = await clientNovaRoma.CreateCourseCurriculum("NR - Grade de ADS - 1.0", courseNovaRoma.Id);

        CreateCourseOut courseUfpe = await clientUfpe.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);
        await clientUfpe.CreateCourseCurriculum("UFPE - Grade de ADS - 1.0", courseUfpe.Id);

        // Act
        var courseCurriculums = await clientNovaRoma.GetCourseCurriculums();

        // Assert
        courseCurriculums.Should().HaveCount(1);
        courseCurriculums[0].Id.Should().Be(courseCurriculumNovaRoma.Id);
        courseCurriculums[0].Name.Should().Be(courseCurriculumNovaRoma.Name);
    }

    [Test]
    public async Task Should_return_all_course_curriculums_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        CreateCourseOut direito = await client.CreateCourse("Direito", CourseType.Bacharelado, []);
        CourseCurriculumOut direitoCourseCurriculum = await client.CreateCourseCurriculum("Grade de Direito 1.0", direito.Id);

        CreateCourseOut ads = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, []);
        CourseCurriculumOut adsCourseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", ads.Id);

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
