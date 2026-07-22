namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetParentStudentAgenda(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_when_user_is_a_manager()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetParentStudentAgenda(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_when_user_is_a_student()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var studentEmail = DataGen.Email;
        var studentId = (await director.CreateStudent(DataGen.UserName, studentEmail).Success()).Id;

        var client = await _back.LoginAs(studentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(studentId);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_of_a_student_not_linked_to_the_parent()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var linkedStudentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;
        var otherStudentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = linkedStudentId, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(otherStudentId);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_when_link_is_revoked()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var studentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;

        var parentEmail = DataGen.Email;
        var parentId = (await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]).Success()).Id;

        await using (var ctx = _back.GetDbContext())
        {
            var link = await ctx.ParentStudents.FirstAsync(x => x.ParentId == parentId);
            link.Status = ParentStudentStatus.Revoked;
            await ctx.SaveChangesAsync();
        }

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(studentId);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_not_get_agenda_when_link_was_revoked_by_student()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var studentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;

        var parentEmail = DataGen.Email;
        var parentId = (await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]).Success()).Id;

        await using (var ctx = _back.GetDbContext())
        {
            var link = await ctx.ParentStudents.FirstAsync(x => x.ParentId == parentId);
            link.RevokedByStudent = true;
            await ctx.SaveChangesAsync();
        }

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(studentId);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_get_empty_agenda_when_student_has_no_classes()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var studentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(studentId).Success();

        // Assert
        result.Days.Should().BeEmpty();
    }

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_get_student_agenda()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var teacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = await director.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        await director.StartClass(@class.Id);

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = student.Id, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(student.Id);

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(1);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines.Should().HaveCount(1);
        days[0].Disciplines[0].ClassId.Should().Be(@class.Id);
        days[0].Disciplines[0].Name.Should().Be("Geometria");
        days[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        days[0].Disciplines[0].End.Should().Be(Hour.H10_00);
    }

    [Test]
    public async Task Parents_GetParentStudentAgenda_Should_show_the_classroom_name_when_the_schedule_has_a_classroom_and_null_when_online()
    {
        // Arrange — turma com dois horários: segunda numa sala física, quarta sem sala (online).
        var director = await _back.LoggedAsDirector();
        var campus = await director.CreateCampus().Success();
        var classroom = await director.CreateClassroom(campus.Id, "Sala 07").Success();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();

        var teacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, null),
        ]);
        await director.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = await director.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        await director.StartClass(@class.Id);

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = student.Id, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(student.Id);

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(2);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines[0].ClassroomName.Should().Be("Sala 07");
        days[1].Day.Should().Be(Day.Wednesday);
        days[1].Disciplines[0].ClassroomName.Should().BeNull();
    }

    #endregion
}
