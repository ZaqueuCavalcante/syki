namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_notes_just_after_enrollment()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        // Act
        var response = await studentClient.GetStudentNotes();

        // Assert
        response.Count.Should().Be(6);

        response.ForEach(x =>
        {
            x.Period.Should().Be(1);
            x.AverageNote.Should().Be(0);
            x.Notes.Should().HaveCount(3);
            x.Notes.Should().AllSatisfy(y => y.Note.Should().Be(0));
            x.StudentDisciplineStatus.Should().Be(StudentDisciplineStatus.Matriculado);
        });
    }

    [Test]
    public async Task Should_get_student_notes_after_teacher_add_notes()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);
        var studentClient = await _api.LoggedAsStudent(data.Student.Email);

        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 8);
        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 10);
        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N2, 100, 7);
        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N3, 100, 10);

        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N1, 100, 0);
        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N2, 100, 7);
        await teacherClient.AddClassActivityNote(data.AdsClasses.IntroToWebDev.Id, data.Student.Id, ClassNoteType.N3, 100, 8);

        await teacherClient.AddClassActivityNote(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, ClassNoteType.N1, 50, 5);
        await teacherClient.AddClassActivityNote(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, ClassNoteType.N2, 20, 7);
        await teacherClient.AddClassActivityNote(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, ClassNoteType.N2, 30, 8);
        await teacherClient.AddClassActivityNote(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, ClassNoteType.N2, 50, 9);
        await teacherClient.AddClassActivityNote(data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, data.Student.Id, ClassNoteType.N3, 100, 10);

        // Act
        var response = await studentClient.GetStudentNotes();

        // Assert
        response.Count.Should().Be(6);

        AssertCorrectNotes(response, data.AdsClasses.DiscreteMath.Id, 9.00M, 7.00M, 10.00M, 9.50M);
        AssertCorrectNotes(response, data.AdsClasses.IntroToWebDev.Id, 0.00M, 7.00M, 8.00M, 7.50M);
        AssertCorrectNotes(response, data.AdsClasses.ComputationalThinkingAndAlgorithms.Id, 2.50M, 8.30M, 10.00M, 9.15M);
    }

    private static void AssertCorrectNotes(List<StudentNoteOut> list, Guid classId, decimal n1, decimal n2, decimal n3, decimal averageNote)
    {
        var discreteMath = list.First(x => x.ClassId == classId);
        discreteMath.Notes.First(x => x.Type == ClassNoteType.N1).Note.Should().Be(n1);
        discreteMath.Notes.First(x => x.Type == ClassNoteType.N2).Note.Should().Be(n2);
        discreteMath.Notes.First(x => x.Type == ClassNoteType.N3).Note.Should().Be(n3);
        discreteMath.AverageNote.Should().Be(averageNote);
    }
}
