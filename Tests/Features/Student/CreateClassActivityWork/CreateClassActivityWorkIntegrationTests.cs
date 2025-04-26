namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_class_activity_work()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        CreateClassActivityOut work = await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await teacherClient.CreateClassActivity(data.AdsClasses.IntroToWebDev.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 80);

        // Act
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);
        var activity = (await studentClient.CreateClassActivityWork(work.Id, "https://github.com/ZaqueuCavalcante/syki")).GetSuccess();

        // Assert
        activity.Status.Should().Be(ClassActivityWorkStatus.Delivered);
        activity.Link.Should().Be("https://github.com/ZaqueuCavalcante/syki");
    }

    [Test]
    public async Task Should_not_create_student_class_activity_work_when_activity_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        await teacherClient.CreateClassActivity(data.AdsClasses.IntroToWebDev.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 80);

        // Act
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);
        var response = await studentClient.CreateClassActivityWork(Guid.NewGuid(), "https://github.com/ZaqueuCavalcante/syki");

        // Assert
        response.ShouldBeError(new ClassActivityNotFound());
    }
}
