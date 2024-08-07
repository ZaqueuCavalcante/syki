namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_insights_just_after_student_creation()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(6);
        insights.Average.Should().Be(0);
        insights.CR.Should().Be(0);
    }

    [Test]
    public async Task Should_return_student_insights_with_average_and_cr()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut discreteMathClass = await academicClient.CreateClass(data.Disciplines.DiscreteMath.Id, teacher.Id, data.AcademicPeriod.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.Disciplines.IntroToWebDev.Id, teacher.Id, data.AcademicPeriod.Id, 45, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);

        StudentOut student = await academicClient.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToWebDevClass.Id]);



        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var teacherMathClass = await teacherClient.GetTeacherClass(discreteMathClass.Id);
        var examGradeId = teacherMathClass.Students.First().ExamGrades.First().Id;
        await teacherClient.AddExamGradeNote(examGradeId, 5.67M);

        // TODO: preciso ser aprovado na disciplina pra que ela passe a contar nos cálculos de média e CR
        


        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(6);
        insights.Average.Should().Be(0);
        insights.CR.Should().Be(0);
    }
}
