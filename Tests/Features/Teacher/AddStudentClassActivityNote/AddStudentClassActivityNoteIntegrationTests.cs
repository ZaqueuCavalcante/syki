namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_add_student_class_activity_note()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        CreateClassActivityOut work = await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await teacherClient.CreateClassActivity(data.AdsClasses.IntroToWebDev.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 80);

        var studentClient = await _api.LoggedAsStudent(data.Student.Email);
        await studentClient.CreateClassActivityWork(work.Id, "https://github.com/ZaqueuCavalcante/syki");

        // Act
        var response = await teacherClient.AddStudentClassActivityNote(work.Id, data.Student.Id, 8.12M);

        // Assert
        response.ShouldBeSuccess();
        var activity = (await teacherClient.GetTeacherClassActivity(data.AdsClasses.DiscreteMath.Id, work.Id)).GetSuccess();
        activity.Works[0].Note.Should().Be(8.12M);
    }
}
