namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_exam_grades_just_after_enrollment()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        // Act
        var response = await studentClient.GetStudentExamGrades();

        // Assert
        response.Count.Should().Be(6);

        response.ForEach(x =>
        {
            x.Period.Should().Be(1);
            x.AverageNote.Should().Be(0);
            x.ExamGrades.Should().HaveCount(3);
            x.ExamGrades.Should().AllSatisfy(x => x.Note.Should().Be(0));
            x.StudentDisciplineStatus.Should().Be(StudentDisciplineStatus.Matriculado);
        });
    }

    [Test]
    public async Task Should_get_student_exam_grades_after_teacher_add_notes()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _back.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddExamGradeNotes(data.AdsClasses.DiscreteMath.Id, data.Student.Id, 1.67M, 8.50M, 5.23M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, 7.58M, 1.28M, 7.43M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.HumanMachineInteractionDesign.Id, data.Student.Id, 0.00M, 1.75M, 0.90M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntroToComputerNetworks.Id, data.Student.Id, 3.42M, 3.34M, 6.14M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, 2.84M, 8.61M, 0.86M);
        await teacherClient.AddExamGradeNotes(data.AdsClasses.IntegratorProjectOne.Id, data.Student.Id, 8.77M, 1.21M, 10.0M);

        // Act
        var response = await studentClient.GetStudentExamGrades();

        // Assert
        response.Count.Should().Be(6);

        AssertCorrectNotes(response, data.AdsClasses.DiscreteMath.Id, 6.86M, 1.67M, 8.50M, 5.23M);
        AssertCorrectNotes(response, data.AdsClasses.IntroToWebDev.Id, 7.50M, 7.58M, 1.28M, 7.43M);
        AssertCorrectNotes(response, data.AdsClasses.HumanMachineInteractionDesign.Id, 1.32M, 0.00M, 1.75M, 0.90M);
        AssertCorrectNotes(response, data.AdsClasses.IntroToComputerNetworks.Id, 4.78M, 3.42M, 3.34M, 6.14M);
        AssertCorrectNotes(response, data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, 5.72M, 2.84M, 8.61M, 0.86M);
        AssertCorrectNotes(response, data.AdsClasses.IntegratorProjectOne.Id, 9.38M, 8.77M, 1.21M, 10.0M);
    }

    private static void AssertCorrectNotes(List<StudentExamGradeOut> list, Guid classId, decimal averageNote, decimal n1, decimal n2, decimal n3)
    {
        var discreteMath = list.First(x => x.ClassId == classId);
        discreteMath.AverageNote.Should().Be(averageNote);
        discreteMath.ExamGrades.First(x => x.ExamType == ExamType.N1).Note.Should().Be(n1);
        discreteMath.ExamGrades.First(x => x.ExamType == ExamType.N2).Note.Should().Be(n2);
        discreteMath.ExamGrades.First(x => x.ExamType == ExamType.N3).Note.Should().Be(n3);
    }
}
