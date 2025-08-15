namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_not_return_teacher_on_pre_enrollment_classes()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_teacher_on_enrollment_classes()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        TeacherOut chico = await client.CreateTeacher("Chico");
        ClassOut discreteMathClass = await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await client.ReleaseClassesForEnrollment([discreteMathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_other_teacher_classes()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        TeacherOut chico = await client.CreateTeacher("Chico");
        TeacherOut ana = await client.CreateTeacher("Ana");

        await client.CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, data.Campus.Id, ana.Id, period.Id, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().BeEmpty();
    }
}
