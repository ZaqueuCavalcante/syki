namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_disciplines()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.GetStudentDisciplines();

        // Assert
        response.Count.Should().Be(12);
        response.Should().BeEquivalentTo(data.AdsCourse.Disciplines);
    }
}
