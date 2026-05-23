namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_return_empty_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.Total.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_return_curriculums()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = (await client.CreateCourse()).Success;
        await client.CreateCourseCurriculum(course.Id, "Grade 2024");

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.Total.Should().Be(1);
        result.Items[0].Name.Should().Be("Grade 2024");
        result.Items[0].CourseId.Should().Be(course.Id);
    }
}
