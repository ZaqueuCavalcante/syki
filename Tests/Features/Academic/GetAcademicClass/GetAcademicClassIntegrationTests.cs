namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_academic_class()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        ClassOut createdClass = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        GetAcademicClassOut @class = await client.GetAcademicClass(createdClass.Id);

        // Assert
        @class.Discipline.Should().Be(discipline.Name);
    }

    [Test]
    public async Task Should_return_academic_class_with_awaiting_start_status_when_enrollment_period_ends()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;
        await academicClient.CreateEnrollmentPeriod(period.Id);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut createdClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        await academicClient.ReleaseClassesForEnrollment([createdClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period.Id, -4, -1);

        // Act
        GetAcademicClassOut @class = await academicClient.GetAcademicClass(createdClass.Id);

        // Assert
        @class.Status.Should().Be(ClassStatus.AwaitingStart);
    }

    [Test]
    public async Task Should_return_academic_class_with_fill_ratio()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        // Act
        GetAcademicClassOut @class = await academicClient.GetAcademicClass(mathClass.Id);

        // Assert
        @class.FillRatio.Should().Be("1/40");
    }

    [Test]
    public async Task Should_not_get_random_academic_class()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.GetAcademicClass(Guid.CreateVersion7());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }
}
