namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_GetClass_Should_not_get_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_GetClass_Should_not_get_class_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_GetClass_Should_not_get_class_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetClass(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_GetClass_Should_get_class_details()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.GetClass(@class.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(@class.Id);
        details.Discipline.Should().Be("Geometria");
        details.Period.Should().Be("2024.1");
        details.Status.Should().Be(ClassStatus.OnPreEnrollment);
        details.Schedules.Should().NotBeEmpty();
        details.Students.Should().BeEmpty();
    }

    [Test]
    public async Task Classes_GetClass_Should_get_class_with_enrolled_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var classId = await CreateOnEnrollmentClass(client);
        await client.AssignStudentToClass(student.Id, classId);

        // Act
        var result = await client.GetClass(classId);

        // Assert
        var details = result.Success;
        details.Status.Should().Be(ClassStatus.OnEnrollment);
        details.Students.Should().ContainSingle();
        details.Students[0].Id.Should().Be(student.Id);
        details.Students[0].Status.Should().Be(StudentClassStatus.Matriculado);
    }

    #endregion
}
