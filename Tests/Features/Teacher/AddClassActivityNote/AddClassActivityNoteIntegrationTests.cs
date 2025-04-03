using Syki.Back.Features.Teacher.AddClassActivityNote;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidNotes))]
    public async Task Should_add_valid_class_activity_note(decimal note)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var noteId = @class.Students.First().Notes.First().Id;

        // Act
        var response = await teacherClient.AddClassActivityNote(noteId, note);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var classAfter = await teacherClient.GetTeacherClass(mathClass.Id);
        var notes = classAfter.Students.First().Notes;
        notes.First(x => x.Type == ClassNoteType.N1).Note.Should().Be(note);
        notes.First(x => x.Type == ClassNoteType.N2).Note.Should().Be(0);
        notes.First(x => x.Type == ClassNoteType.N3).Note.Should().Be(0);

        await AssertDomainEvent<StudentClassNoteAddedDomainEvent>(@class.Id.ToString());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidNotes))]
    public async Task Should_not_add_invalid_class_activity_note(decimal note)
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var noteId = @class.Students.First().Notes.First().Id;

        // Act
        var response = await teacherClient.AddClassActivityNote(noteId, note);

        // Assert
        await response.AssertBadRequest(new InvalidStudentClassNote());
    }

    [Test]
    public async Task Should_not_add_class_activity_note_when_not_found()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _api.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.AddClassActivityNote(Guid.NewGuid(), 5.67M);

        // Assert
        await response.AssertBadRequest(new ClassActivityNoteNotFound());
    }

    [Test]
    public async Task Should_send_notification_to_user_when_teacher_add_class_activity_note()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _api.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _api.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var noteId = @class.Students.First().Notes.First().Id;

        // Act
        await teacherClient.AddClassActivityNote(noteId, 7.89M);
        
        await _daemon.AwaitEventsProcessing();
        await _daemon.AwaitCommandsProcessing();

        // Assert
        var response = await studentClient.Http.GetUserNotifications();
        response.Should().HaveCount(1);
        response[0].Title.Should().Be("Nota adicionada");
        response[0].Description.Should().Be($"Confira sua nota na disciplina: {@class.Discipline}");
    }
}
