namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_class_students()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);

        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 8);
        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N1, 50, 10);
        await teacherClient.AddClassActivityNote(data.AdsClasses.DiscreteMath.Id, data.Student.Id, ClassNoteType.N2, 100, 7);

        var lessons = data.AdsClasses.DiscreteMath.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[1].Id, [data.Student.Id]);
        await teacherClient.CreateLessonAttendance(lessons[2].Id, []);

        // Act
        var response = await teacherClient.GetTeacherClassStudents(data.AdsClasses.DiscreteMath.Id);

        // Assert
        response.ShouldBeSuccess();
        var students = response.Success;
        students[0].Name.Should().Be(data.Student.Name);
        students[0].Frequency.Should().Be(66.67M);
        students[0].Notes.First(x => x.Type == ClassNoteType.N1).Note.Should().Be(9.00M);
        students[0].Notes.First(x => x.Type == ClassNoteType.N2).Note.Should().Be(7.00M);
        students[0].Notes.First(x => x.Type == ClassNoteType.N3).Note.Should().Be(0.00M);
        students[0].AverageNote.Should().Be(8.00M);
    }
}
