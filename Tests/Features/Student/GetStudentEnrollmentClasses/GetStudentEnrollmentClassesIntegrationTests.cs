namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_only_classes_of_student_course_curriculum_with_enrollment_status()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        await client.CreateEnrollmentPeriod(period.Id, -2, 2);

        TeacherOut chico = await client.CreateTeacher("Chico");
        TeacherOut ana = await client.CreateTeacher("Ana");

        ClassOut discreteMathClass = await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut databasesClass = await client.CreateClass(data.AdsDisciplines.Databases.Id, chico.Id, period.Id, 40, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut manSocietyAndLawClass = await client.CreateClass(data.DireitoDisciplines.ManSocietyAndLaw.Id, ana.Id, period.Id, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H08_00)]);

        await client.ReleaseClassesForEnrollment([discreteMathClass.Id]);

        StudentOut zaqueu = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        StudentOut maju = await client.CreateStudent(data.DireitoCourseOffering.Id, "Maju");

        var studentClient = await _api.LoggedAsStudent(zaqueu.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().ContainSingle();
        classes.Should().Contain(t => t.Id == discreteMathClass.Id);
    }

    [Test]
    public async Task Should_not_return_any_class_without_enrollment_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        var period = data.AcademicPeriod2;
        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_class_before_enrollment_period_start()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        var period = data.AcademicPeriod2;
        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, 2, 4);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_on_pre_enrollment_class_after_enrollment_period_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        var period = data.AcademicPeriod2;
        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, -4, -2);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_on_enrollment_class_after_enrollment_period_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        var period = data.AcademicPeriod2;
        TeacherOut chico = await client.CreateTeacher("Chico");
        ClassOut discreteMathClass = await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, -4, 4);
        await client.ReleaseClassesForEnrollment([discreteMathClass.Id]);
        await client.UpdateEnrollmentPeriod(data.AcademicPeriod2.Id, -4, -1);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_not_return_any_student_on_pre_enrollment_class()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        var period = data.AcademicPeriod2;
        TeacherOut chico = await client.CreateTeacher("Chico");
        await client.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await client.CreateEnrollmentPeriod(data.AcademicPeriod2.Id, -2, 2);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var classes = await studentClient.GetStudentEnrollmentClasses();

        // Assert
        classes.Should().BeEmpty();
    }
}
