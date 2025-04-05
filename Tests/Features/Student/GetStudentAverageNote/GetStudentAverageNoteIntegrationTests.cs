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
        
        CreateClassActivityOut activity = await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, note: ClassNoteType.N1, weight: 50);
        await teacherClient.AddStudentClassActivityNote(activity.Id, data.Student.Id, 8);
        
        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(2);
    }
}
