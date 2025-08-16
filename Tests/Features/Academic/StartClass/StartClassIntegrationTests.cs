namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_start_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);

        // Act
        await academicClient.StartClasses([mathClass.Id]);

        // Assert
        await using var ctx = _api.GetDbContext();
        var @class = await ctx.Classes.Where(x => x.Id == mathClass.Id).FirstAsync();

        @class.Status.Should().Be(ClassStatus.Started);
    }

    [Test]
    public async Task Should_not_start_invalid_classes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        // Act
        var response = await academicClient.StartClasses([Guid.CreateVersion7()]);

        // Assert
        response.ShouldBeError(new InvalidClassesList());
    }

    [Test]
    public async Task Should_not_start_on_pre_enrollment_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        // Act
        var response = await academicClient.StartClasses([mathClass.Id]);

        // Assert
        response.ShouldBeError(new ClassMustHaveOnEnrollmentStatus());
    }

    [Test]
    public async Task Should_not_start_on_enrollment_class_when_enrollment_period_is_not_finalized()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        await academicClient.AssignCampiToTeacher(chico.Id, [data.Campus.Id]);
        await academicClient.AssignDisciplinesToTeacher(chico.Id, [data.AdsDisciplines.DiscreteMath.Id]);

        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        // Act
        var response = await academicClient.StartClasses([mathClass.Id]);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodMustBeFinalized());
    }
}
