namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_add_exam_grade_note()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        var mathClass = await academicClient.CreateClass(data.Disciplines.DiscreteMath.Id, chico.Id, data.AcademicPeriod.Id, 40, [ new(Day.Segunda, Hour.H07_00, Hour.H10_00) ]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([ mathClass.Id ]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGradeId = @class.Students.First().ExamGrades.First().Id;

        // Act
        var response = await teacherClient.AddExamGradeNote(examGradeId, 5.67M);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var classAfter = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGrades = classAfter.Students.First().ExamGrades;
        examGrades.First(x => x.ExamType == ExamType.N1).Note.Should().Be(5.67M);
        examGrades.First(x => x.ExamType == ExamType.N2).Note.Should().Be(0);
        examGrades.First(x => x.ExamType == ExamType.N3).Note.Should().Be(0);
    }
}
