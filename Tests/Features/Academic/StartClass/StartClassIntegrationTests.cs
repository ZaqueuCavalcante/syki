namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_start_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment(period, [mathClass.Id]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);

        // Act
        await academicClient.StartClass(mathClass.Id);

        // Assert
        using var ctx = _back.GetDbContext();
        var examGrades = await ctx.ExamGrades.Where(x => x.ClassId == mathClass.Id).ToListAsync();

        examGrades.Should().HaveCount(3);
        examGrades.Should().AllSatisfy(x => x.StudentId.Should().Be(student.Id));
        examGrades.Count(x => x.ExamType == ExamType.N1).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N2).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N3).Should().Be(1);
        examGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }

    [Test]
    public async Task Should_not_start_not_founded_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        // Act
        var response = await academicClient.StartClass(Guid.NewGuid());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }

    [Test]
    public async Task Should_not_start_on_pre_enrollment_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        // Act
        var response = await academicClient.StartClass(mathClass.Id);

        // Assert
        response.ShouldBeError(new ClassMustHaveOnEnrollmentStatus());
    }

    [Test]
    public async Task Should_not_start_on_enrollment_class_when_enrollment_period_is_not_finalized()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment(period, [mathClass.Id]);

        // Act
        var response = await academicClient.StartClass(mathClass.Id);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodMustBeFinalized());
    }
}
