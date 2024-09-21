namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_exam_grades_after_enrollment()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        TeacherOut ana = await academicClient.CreateTeacher("Ana");

        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToWebDevClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToWebDev.Id, chico.Id, period, 40, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut humanMachineInteractionDesignClass = await academicClient.CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, ana.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        ClassOut introToComputerNetworksClass = await academicClient.CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, ana.Id, period, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);

        await academicClient.ReleaseClassesForEnrollment([discreteMathClass.Id, introToWebDevClass.Id, humanMachineInteractionDesignClass.Id, introToComputerNetworksClass.Id]);

        StudentOut zaqueu = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id, introToWebDevClass.Id, introToComputerNetworksClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([discreteMathClass.Id]);
        await academicClient.StartClasses([introToWebDevClass.Id]);
        await academicClient.StartClasses([introToComputerNetworksClass.Id]);

        // Act
        var response = await studentClient.GetStudentExamGrades();

        // Assert
        response.Count.Should().Be(3);
        response[0].Period.Should().Be(1);
        response[0].Discipline.Should().Be(data.AdsDisciplines.IntroToComputerNetworks.Name);
        response[0].ExamGrades.Should().HaveCount(3);
        response[0].ExamGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));

        response[1].Period.Should().Be(1);
        response[1].Discipline.Should().Be(data.AdsDisciplines.IntroToWebDev.Name);
        response[1].ExamGrades.Should().HaveCount(3);
        response[1].ExamGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));

        response[2].Period.Should().Be(1);
        response[2].Discipline.Should().Be(data.AdsDisciplines.DiscreteMath.Name);
        response[2].ExamGrades.Should().HaveCount(3);
        response[2].ExamGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
    }

    [Test]
    public async Task Should_get_student_exam_grades_after_teacher_add_notes()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);
   
        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut zaqueu = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut discreteMathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        await academicClient.ReleaseClassesForEnrollment([discreteMathClass.Id]);

        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);
        await studentClient.CreateStudentEnrollment([discreteMathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([discreteMathClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var teacherMathClass = await teacherClient.GetTeacherClass(discreteMathClass.Id);
        var examGradeId = teacherMathClass.Students.First().ExamGrades.First().Id;
        await teacherClient.AddExamGradeNote(examGradeId, 5.67M);
        
        // Act
        var response = await studentClient.GetStudentExamGrades();

        // Assert
        response.Count.Should().Be(1);
        response[0].Period.Should().Be(1);
        response[0].Discipline.Should().Be(data.AdsDisciplines.DiscreteMath.Name);
        var examGrades = response[0].ExamGrades;
        examGrades.Should().HaveCount(3);
        examGrades.First(x => x.ExamType == ExamType.N1).Note.Should().Be(5.67M);
        examGrades.First(x => x.ExamType == ExamType.N2).Note.Should().Be(0);
        examGrades.First(x => x.ExamType == ExamType.N3).Note.Should().Be(0);
    }
}
