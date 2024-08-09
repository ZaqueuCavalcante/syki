namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_start_class()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.Disciplines.DiscreteMath.Id, chico.Id, data.AcademicPeriod.Id, 40, [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

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
}
