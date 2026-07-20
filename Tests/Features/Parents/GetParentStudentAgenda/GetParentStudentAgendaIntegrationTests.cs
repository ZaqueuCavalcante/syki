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

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var studentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;
        await director.AssignStudentToClass(studentId, @class.Id);

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParentStudentAgenda(studentId);

        // Assert
        result.ShouldBeSuccess();
        var days = result.Success.Days;
        days.Should().HaveCount(1);
        days[0].Day.Should().Be(Day.Monday);
        days[0].Disciplines.Should().HaveCount(1);
        days[0].Disciplines[0].ClassId.Should().Be(@class.Id);
        days[0].Disciplines[0].Name.Should().Be("Geometria");
        days[0].Disciplines[0].Start.Should().Be(Hour.H07_00);
        days[0].Disciplines[0].End.Should().Be(Hour.H09_00);
    }

    #endregion
}
