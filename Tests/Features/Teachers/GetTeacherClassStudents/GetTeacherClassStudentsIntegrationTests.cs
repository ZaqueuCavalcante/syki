namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_not_get_students_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherClassStudents(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_not_get_students_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherClassStudents(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_not_get_students_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClassStudents(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_not_get_students_of_class_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClassStudents(@class.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_not_get_students_of_class_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);
        var otherTeacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassStudents(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_get_empty_list_when_class_has_no_students()
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
        var result = await client.GetTeacherClassStudents(@class.Id);

        // Assert
        result.Success.Students.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_get_class_students()
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

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = await director.CreateStudent("Zaqueu do Vale", DataGen.Email).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassStudents(@class.Id);

        // Assert
        var students = result.Success.Students;
        students.Should().ContainSingle();

        var item = students[0];
        item.Id.Should().Be(student.Id);
        item.Name.Should().Be("Zaqueu do Vale");
        item.Status.Should().Be(StudentClassStatus.Matriculado);
    }

    [Test]
    public async Task Teachers_GetTeacherClassStudents_Should_get_class_students_ordered_by_name()
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

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var carlos = await director.CreateStudent("Carlos Andrade", DataGen.Email).Success();
        var ana = await director.CreateStudent("Ana Beatriz", DataGen.Email).Success();
        var bruno = await director.CreateStudent("Bruno Silva", DataGen.Email).Success();
        await director.AssignStudentToClass(carlos.Id, @class.Id);
        await director.AssignStudentToClass(ana.Id, @class.Id);
        await director.AssignStudentToClass(bruno.Id, @class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassStudents(@class.Id);

        // Assert
        var students = result.Success.Students;
        students.Should().HaveCount(3);
        students.Select(x => x.Name).Should().ContainInOrder("Ana Beatriz", "Bruno Silva", "Carlos Andrade");
    }

    #endregion
}
