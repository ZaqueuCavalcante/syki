namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_release_classes_for_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id);

        TeacherOut chico = await client.CreateTeacher("Chico");
        ClassOut mathClass = await client.CreateClass(
            data.Disciplines.DiscreteMath.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );

        // Act
        var response = await client.ReleaseClassesForEnrollment(data.AcademicPeriod2.Id, [mathClass.Id]);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.ReleaseClassesForEnrollment("2024.1", []);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodNotFound());
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_not_started()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id, 2, 4);

        // Act
        var response = await client.ReleaseClassesForEnrollment(period.Id, []);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodNotStarted());
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_finalized()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id, -4, -2);

        // Act
        var response = await client.ReleaseClassesForEnrollment(period.Id, []);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodFinalized());
    }

    [Test, Ignore("TODO: FIX THIS")]
    public async Task Should_not_release_classes_for_enrollment_when_class_status_is_not_on_pre_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        TeacherOut chico = await client.CreateTeacher("Chico");
        ClassOut mathClass = await client.CreateClass(
            data.Disciplines.DiscreteMath.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );

        // Act
        var response = await client.ReleaseClassesForEnrollment(data.AcademicPeriod2.Id, [mathClass.Id]);

        // Assert
        response.ShouldBeError(new AllClassesMustHaveOnPreEnrollmentStatus());
    }
}
