namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Parents_GetParents_Should_not_get_parents_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetParents();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Parents_GetParents_Should_not_get_parents_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetParents();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Parents_GetParents_Should_not_get_parents_when_user_is_a_parent()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var studentId = (await director.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        var parentEmail = DataGen.Email;
        await director.CreateParent(DataGen.UserName, parentEmail, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoginAs(parentEmail);

        // Act
        var result = await client.GetParents();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Parents_GetParents_Should_get_parents()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent("Maria Souza", DataGen.Email)).Success.Id;

        await client.CreateParent("Carlos Souza", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Father }]);
        await client.CreateParent("Ana Souza", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Act
        var result = await client.GetParents();

        // Assert
        var parents = result.Success;
        parents.Total.Should().Be(2);
        parents.Page.Should().Be(1);
        parents.PageSize.Should().Be(10);
        parents.Items.First().Name.Should().Be("Ana Souza");
        parents.Items.Last().Name.Should().Be("Carlos Souza");
        parents.Items.Should().OnlyContain(x => x.Students.Contains("Maria Souza"));
    }

    [Test]
    public async Task Parents_GetParents_Should_get_parents_from_the_second_page()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        for (var i = 1; i <= 12; i++)
            await client.CreateParent($"Responsável {i:00}", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Guardian }]);

        // Act
        var result = await client.GetParents(page: 2);

        // Assert
        var parents = result.Success;
        parents.Total.Should().Be(12);
        parents.Page.Should().Be(2);
        parents.Items.Should().HaveCount(2);
        parents.Items.First().Name.Should().Be("Responsável 11");
        parents.Items.Last().Name.Should().Be("Responsável 12");
    }

    [Test]
    public async Task Parents_GetParents_Should_get_parents_filtered_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        await client.CreateParent("Carlos Souza", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Father }]);
        await client.CreateParent("Ana Lima", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Act
        var result = await client.GetParents("ana");

        // Assert
        var parents = result.Success;
        parents.Total.Should().Be(1);
        parents.Items.Single().Name.Should().Be("Ana Lima");
    }

    [Test]
    public async Task Parents_GetParents_Should_get_parents_filtered_by_email()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;

        var email = DataGen.Email;
        await client.CreateParent("Carlos Souza", email, [new() { StudentId = studentId, Relationship = ParentRelationship.Father }]);
        await client.CreateParent("Ana Lima", DataGen.Email, [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }]);

        // Act
        var result = await client.GetParents(email);

        // Assert
        var parents = result.Success;
        parents.Total.Should().Be(1);
        parents.Items.Single().Name.Should().Be("Carlos Souza");
    }

    [Test]
    public async Task Parents_GetParents_Should_not_get_parents_from_other_institutions()
    {
        // Arrange
        var otherClient = await _back.LoggedAsDirector();
        var otherStudentId = (await otherClient.CreateStudent(DataGen.UserName, DataGen.Email)).Success.Id;
        await otherClient.CreateParent(DataGen.UserName, DataGen.Email, [new() { StudentId = otherStudentId, Relationship = ParentRelationship.Mother }]);

        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetParents();

        // Assert
        result.Success.Total.Should().Be(0);
        result.Success.Items.Should().BeEmpty();
    }

    #endregion
}
