namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_not_get_lessons_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherClassLessons(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_not_get_lessons_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherClassLessons(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_not_get_lessons_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClassLessons(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_not_get_lessons_of_class_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);

        var teacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassLessons(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_get_lessons()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;

        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassLessons(@class.Id);

        // Assert
        var lessons = result.Success.Lessons;
        lessons.Should().NotBeEmpty();
        lessons.Should().BeInAscendingOrder(l => l.Number);
        lessons.Should().AllSatisfy(l =>
        {
            l.Status.Should().Be(ClassLessonStatus.Pending);
            l.PresentStudents.Should().BeEmpty();
        });
    }

    [Test]
    public async Task Teachers_GetTeacherClassLessons_Should_get_lessons_with_present_students()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;

        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var students = await EnrollStudentsInClass(director, @class.Id, 2);
        var lessonId = await _back.GetFirstLessonId(@class.Id);

        var client = await _back.LoginAs(email);
        await client.CreateLessonAttendance(lessonId, [students[0]]);

        // Act
        var result = await client.GetTeacherClassLessons(@class.Id);

        // Assert
        var lesson = result.Success.Lessons.First(l => l.Id == lessonId);
        lesson.Status.Should().Be(ClassLessonStatus.Finalized);
        lesson.PresentStudents.Should().BeEquivalentTo([students[0]]);
    }

    #endregion
}
