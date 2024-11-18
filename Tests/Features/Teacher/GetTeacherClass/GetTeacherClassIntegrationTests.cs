namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_class()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;
        var math = data.AdsDisciplines.DiscreteMath;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(math.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);

        // Assert
        @class.Code.Should().Be(math.Code);
        @class.Period.Should().Be(period);
        @class.Discipline.Should().Be(math.Name);
        @class.Status.Should().Be(ClassStatus.Started);

        @class.Students.Should().HaveCount(1);
        var examGrades = @class.Students.First().ExamGrades;
        examGrades.Should().AllSatisfy(x => x.StudentId.Should().Be(student.Id));
        examGrades.Count(x => x.ExamType == ExamType.N1).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N2).Should().Be(1);
        examGrades.Count(x => x.ExamType == ExamType.N3).Should().Be(1);
        examGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }
}
