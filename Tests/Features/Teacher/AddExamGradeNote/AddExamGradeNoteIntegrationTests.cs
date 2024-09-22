namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidExamGradeNotes))]
    public async Task Should_add_valid_exam_grade_note(decimal note)
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGradeId = @class.Students.First().ExamGrades.First().Id;

        // Act
        var response = await teacherClient.AddExamGradeNote(examGradeId, note);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var classAfter = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGrades = classAfter.Students.First().ExamGrades;
        examGrades.First(x => x.ExamType == ExamType.N1).Note.Should().Be(note);
        examGrades.First(x => x.ExamType == ExamType.N2).Note.Should().Be(0);
        examGrades.First(x => x.ExamType == ExamType.N3).Note.Should().Be(0);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidExamGradeNotes))]
    public async Task Should_add_invalid_exam_grade_note(decimal note)
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGradeId = @class.Students.First().ExamGrades.First().Id;

        // Act
        var response = await teacherClient.AddExamGradeNote(examGradeId, note);

        // Assert
        await response.AssertBadRequest(new InvalidExamGradeNote());
    }

    [Test]
    public async Task Should_not_add_exam_grade_note_when_not_found()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var response = await teacherClient.AddExamGradeNote(Guid.NewGuid(), 5.67M);

        // Assert
        await response.AssertBadRequest(new ExamGradeNotFound());
    }

    [Test]
    public async Task Should_send_notification_to_user_when_teacher_add_exam_grade_note()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2.Id;

        await academicClient.CreateEnrollmentPeriod(period, -2, 2);

        TeacherOut chico = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        ClassOut mathClass = await academicClient.CreateClass(data.AdsDisciplines.DiscreteMath.Id, chico.Id, period, 40, [ new(Day.Monday, Hour.H07_00, Hour.H10_00) ]);

        await academicClient.ReleaseClassesForEnrollment([mathClass.Id]);

        var studentClient = await _back.LoggedAsStudent(student.Email);
        await studentClient.CreateStudentEnrollment([mathClass.Id]);

        await academicClient.UpdateEnrollmentPeriod(period, -2, -1);
        await academicClient.StartClasses([mathClass.Id]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);
        var @class = await teacherClient.GetTeacherClass(mathClass.Id);
        var examGradeId = @class.Students.First().ExamGrades.First().Id;

        await teacherClient.AddExamGradeNote(examGradeId, 7.89M);
        await _back.AwaitTasksProcessing();

        // Act
        var response = await studentClient.Cross.GetUserNotifications();

        // Assert
        response.Should().HaveCount(1);
        response[0].Title.Should().Be("Nota adicionada");
        response[0].Description.Should().Be($"Confira sua nota na disciplina: {@class.Discipline}");
    }
}
