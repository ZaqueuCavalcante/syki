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
    public async Task Should_get_student_average_notes_after_teacher_add_1_note()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 8);
        
        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(0.33M);
    }

    [Test]
    public async Task Should_get_student_average_notes_after_teacher_add_2_notes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 10);
        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N1, 30, 6);
        
        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(0.57M);
    }

    [Test]
    public async Task Should_get_student_average_notes_after_teacher_add_3_notes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 10, 10);
        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N1, 80, 8);
        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N2, 100, 9);

        // Act
        var response = await studentClient.GetStudentAverageNote();

        // Assert
        response.AverageNote.Should().Be(1.37M);
    }
}
