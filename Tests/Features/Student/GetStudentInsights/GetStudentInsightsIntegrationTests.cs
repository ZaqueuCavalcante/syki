namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_student_insights_just_after_student_creation()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(12);
        insights.Frequency.Should().Be(0);
        insights.Average.Should().Be(0);
        insights.YieldCoefficient.Should().Be(0);
    }

    [Test]
    public async Task Should_return_student_insights_after_first_lesson_attendance()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut teacher = await academicClient.CreateTeacher();
        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, teacher.Id, period, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, teacher.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        await academicClient.CreateClassLessons(discreteMathClass.Id);

        await academicClient.ReleaseClassesForEnrollment([discreteMathClass.Id]);

        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToWebDevClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([discreteMathClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var teacherMathClass = await teacherClient.GetTeacherClass(discreteMathClass.Id);
        var firstLesson = teacherMathClass.Lessons.First();

        await teacherClient.CreateLessonAttendance(firstLesson.Id, [student.Id]);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.Status.Should().Be(StudentStatus.Enrolled);
        insights.FinishedDisciplines.Should().Be(0);
        insights.TotalDisciplines.Should().Be(12);
        insights.Frequency.Should().Be(100);
        insights.Average.Should().Be(0);
        insights.YieldCoefficient.Should().Be(0);
    }
}
