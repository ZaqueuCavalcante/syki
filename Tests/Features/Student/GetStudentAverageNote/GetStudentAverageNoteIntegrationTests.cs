namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_average_note_just_after_enrollment()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(0);
    }

    [Test]
    public async Task Should_get_student_average_notes_after_teacher_add_notes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddExamGradeNotes(data.AdsClasses.DiscreteMath.Id, data.Student.Id, 1.67M, 8.50M, 5.23M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, 7.58M, 1.28M, 7.43M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.HumanMachineInteractionDesign.Id, data.Student.Id, 0.00M, 1.75M, 0.90M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntroToComputerNetworks.Id, data.Student.Id, 3.42M, 3.34M, 6.14M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, 2.84M, 8.61M, 0.86M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntegratorProjectOne.Id, data.Student.Id, 8.77M, 1.21M, 10.0M);

        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(0);
    }
}
