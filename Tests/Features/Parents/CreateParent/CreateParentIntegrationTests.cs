namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_email_is_invalid()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, "invalid-email", [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Assert
        result.ShouldBeError(InvalidEmail.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_email_already_used()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        var email = DataGen.Email;
        await client.CreateParent(DataGen.UserName, email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Act
        var result = await client.CreateParent(DataGen.UserName, email, [new() { StudentId = studentId, Relationship = ParentRelationship.Father }]);

        // Assert
        result.ShouldBeError(EmailAlreadyUsed.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_phone_number_is_invalid()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }], phoneNumber: "123456789");

        // Assert
        result.ShouldBeError(InvalidPhoneNumber.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_students_list_is_empty()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, []);

        // Assert
        result.ShouldBeError(InvalidParentStudentsList.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_students_list_has_duplicates()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email,
        [
            new() { StudentId = studentId, Relationship = ParentRelationship.Mother },
            new() { StudentId = studentId, Relationship = ParentRelationship.Guardian },
        ]);

        // Assert
        result.ShouldBeError(InvalidParentStudentsList.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_student_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = 4040404, Relationship = ParentRelationship.Mother }]);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Parents_CreateParent_Should_not_create_parent_when_student_belongs_to_another_institution()
    {
        // Arrange
        var otherInstitutionClient = await _back.LoggedAsDirector();
        var otherStudentId = (await otherInstitutionClient.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = otherStudentId, Relationship = ParentRelationship.Mother }]);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Parents_CreateParent_Should_create_parent()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Assert
        result.ShouldBeSuccess();
        var parentId = result.Success.Id;
        parentId.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();
        var links = await ctx.ParentStudents.Where(x => x.ParentId == parentId).ToListAsync();
        links.Should().HaveCount(1);
        links[0].StudentId.Should().Be(studentId);
        links[0].Relationship.Should().Be(ParentRelationship.Mother);
        links[0].Status.Should().Be(ParentStudentStatus.Active);
        links[0].RevokedByStudent.Should().BeFalse();
    }

    [Test]
    public async Task Parents_CreateParent_Should_create_parent_with_multiple_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var firstStudentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;
        var secondStudentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email,
        [
            new() { StudentId = firstStudentId, Relationship = ParentRelationship.Father },
            new() { StudentId = secondStudentId, Relationship = ParentRelationship.Father },
        ]);

        // Assert
        result.ShouldBeSuccess();
        var parentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var links = await ctx.ParentStudents.Where(x => x.ParentId == parentId).ToListAsync();
        links.Should().HaveCount(2);
        links.Select(x => x.StudentId).Should().BeEquivalentTo(new[] { firstStudentId, secondStudentId });
        links.Should().OnlyContain(x => x.Status == ParentStudentStatus.Active);
    }

    [Test]
    public async Task Parents_CreateParent_Should_create_parent_with_parent_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        // Act
        var result = await client.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Guardian }]);

        // Assert
        result.ShouldBeSuccess();
        var parentId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Parents.Where(p => p.Id == parentId).Select(p => p.User!).FirstAsync();
        var role = await ctx.GetUserRole(user.Id, user.InstitutionId);
        role.NormalizedName.Should().Be("RESPONSAVEL");
        role.BaseType.Should().Be(UserType.Parent);
    }

    #endregion
}
