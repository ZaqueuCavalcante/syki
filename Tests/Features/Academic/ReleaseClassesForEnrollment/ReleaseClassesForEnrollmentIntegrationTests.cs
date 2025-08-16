namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_release_classes_for_enrollment()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id);

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await client.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await client.CreateClass(
            data.AdsDisciplines.DiscreteMath.Id,
            data.Campus.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );

        // Act
        var response = await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_not_found()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await client.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await client.CreateClass(
            data.AdsDisciplines.DiscreteMath.Id,
            data.Campus.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );
        
        // Act
        var response = await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodNotFound());
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_not_started()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await client.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await client.CreateClass(
            data.AdsDisciplines.DiscreteMath.Id,
            data.Campus.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );
        
        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, 2, 4);

        // Act
        var response = await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodNotStarted());
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_enrollment_period_finalized()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await client.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await client.CreateClass(
            data.AdsDisciplines.DiscreteMath.Id,
            data.Campus.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );
        
        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, -4, -2);

        // Act
        var response = await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodFinalized());
    }

    [Test]
    public async Task Should_not_release_classes_for_enrollment_when_classes_status_is_not_on_pre_enrollment()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        
        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, -4, 4);

        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await client.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await client.CreateClass(
            data.AdsDisciplines.DiscreteMath.Id,
            data.Campus.Id,
            chico.Id,
            data.AcademicPeriod2.Id,
            40,
            [new(Day.Monday, Hour.H07_00, Hour.H10_00)]
        );
        
        await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Act
        var response = await client.ReleaseClassesForEnrollment([mathClass.Id]);

        // Assert
        response.ShouldBeError(new AllClassesMustHaveOnPreEnrollmentStatus());
    }
}
