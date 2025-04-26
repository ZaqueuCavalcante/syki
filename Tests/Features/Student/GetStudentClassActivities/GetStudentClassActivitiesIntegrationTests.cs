namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_class_activities()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 10);
        await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N2, type: ClassActivityType.Work, weight: 30);
        await teacherClient.CreateClassActivity(data.AdsClasses.IntroToWebDev.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 80);

        // Act
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        // Assert
        var activities = (await studentClient.GetStudentClassActivities(data.AdsClasses.DiscreteMath.Id)).GetSuccess();
        activities.Should().HaveCount(3);
    }

    [Test]
    public async Task Should_not_get_student_class_activities_when_class_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        await teacherClient.CreateClassActivity(data.AdsClasses.DiscreteMath.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);

        // Act
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        // Assert
        var response = await studentClient.GetStudentClassActivities(Guid.NewGuid());
        response.ShouldBeError(new ClassNotFound());
    }
}
