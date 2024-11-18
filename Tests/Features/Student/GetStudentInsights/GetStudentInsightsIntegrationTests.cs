namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_insights_just_after_student_creation()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(12);
        insights.Frequency.Should().Be(0);
        insights.Average.Should().Be(0);
        insights.YieldCoefficient.Should().Be(0);
    }

    [Test]
    public async Task Should_return_student_insights_for_sequence_VVX()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(12);
        insights.Frequency.Should().Be(66.67M);
        insights.Average.Should().Be(0);
        insights.YieldCoefficient.Should().Be(0);
    }
}
