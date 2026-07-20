namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetParentDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetParentDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_when_user_is_a_student()
    {
        // Arrange
        var directorClient = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        await directorClient.CreateStudent(DataGen.UserName, email);
        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetParentDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_when_user_is_a_parent()
    {
        // Arrange
        var directorClient = await _back.LoggedAsDirector();
        var studentId = (await directorClient.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;
        var email = DataGen.Email;
        var parent = (await directorClient.CreateParent(DataGen.UserName, email,
            [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }])).Success;
        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetParentDetails(parent.Id);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_when_parent_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetParentDetails(999999);

        // Assert
        result.ShouldBeError(ParentNotFound.I);
    }

    [Test]
    public async Task Parents_GetParentDetails_Should_not_get_parent_details_of_another_institution()
    {
        // Arrange
        var otherClient = await _back.LoggedAsDirector();
        var studentId = (await otherClient.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;
        var parent = (await otherClient.CreateParent(DataGen.UserName, DataGen.Email,
            [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }])).Success;

        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetParentDetails(parent.Id);

        // Assert
        result.ShouldBeError(ParentNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Parents_GetParentDetails_Should_get_parent_details()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent("Maria Souza", DataGen.Email).Success()).Id;
        var email = DataGen.Email;
        var parent = (await client.CreateParent("Ana Souza", email,
            [new() { StudentId = studentId, Relationship = ParentRelationship.Mother }], "82988887777")).Success;

        // Act
        var result = await client.GetParentDetails(parent.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(parent.Id);
        details.Name.Should().Be("Ana Souza");
        details.Email.Should().Be(email);
        details.PhoneNumber.Should().Be("82988887777");
        details.Students.Should().ContainSingle();
        details.Students[0].Id.Should().Be(studentId);
        details.Students[0].Name.Should().Be("Maria Souza");
        details.Students[0].EnrollmentCode.Should().NotBeEmpty();
        details.Students[0].Status.Should().Be(StudentStatus.Enrolled);
        details.Students[0].Relationship.Should().Be(ParentRelationship.Mother);
        details.Students[0].LinkStatus.Should().Be(ParentStudentStatus.Active);
        details.Students[0].RevokedByStudent.Should().BeFalse();
        details.Students[0].Course.Should().BeNull();
    }

    [Test]
    public async Task Parents_GetParentDetails_Should_get_parent_details_with_all_linked_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var first = (await client.CreateStudent("Ana Lima", DataGen.Email).Success()).Id;
        var second = (await client.CreateStudent("Bruno Lima", DataGen.Email).Success()).Id;
        var parent = (await client.CreateParent(DataGen.UserName, DataGen.Email,
        [
            new() { StudentId = first, Relationship = ParentRelationship.Mother },
            new() { StudentId = second, Relationship = ParentRelationship.Guardian },
        ])).Success;

        // Act
        var result = await client.GetParentDetails(parent.Id);

        // Assert
        var details = result.Success;
        details.Students.Should().HaveCount(2);
        details.Students[0].Name.Should().Be("Ana Lima");
        details.Students[0].Relationship.Should().Be(ParentRelationship.Mother);
        details.Students[1].Name.Should().Be("Bruno Lima");
        details.Students[1].Relationship.Should().Be(ParentRelationship.Guardian);
    }

    [Test]
    public async Task Parents_GetParentDetails_Should_get_parent_details_with_student_current_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentId = (await client.CreateStudent(DataGen.UserName, DataGen.Email).Success()).Id;
        var campus = await client.CreateCampus().Success();
        var course = await client.CreateCourse().Success();
        var curriculum = await client.CreateCourseCurriculum(course.Id).Success();
        var period = await client.CreateAcademicPeriod().Success();
        var offering = await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id).Success();
        await client.EnrollStudentInCourseOffering(studentId, offering.Id);

        var parent = (await client.CreateParent(DataGen.UserName, DataGen.Email,
            [new() { StudentId = studentId, Relationship = ParentRelationship.Father }])).Success;

        // Act
        var result = await client.GetParentDetails(parent.Id);

        // Assert
        var details = result.Success;
        details.Students.Should().ContainSingle();
        details.Students[0].Course.Should().NotBeNull();
        details.Students[0].Campus.Should().NotBeNull();
        details.Students[0].Period.Should().Be("2024.1");
    }

    #endregion
}
