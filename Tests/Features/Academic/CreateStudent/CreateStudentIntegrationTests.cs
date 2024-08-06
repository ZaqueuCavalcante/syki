namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_with_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.CreateStudent(data.CourseOffering.Id);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.CourseOfferingId.Should().Be(data.CourseOffering.Id); 
        response.Name.Should().Be("Zezin"); 
    }

    [Test]
    public async Task Should_not_create_student_without_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateStudent2(Guid.NewGuid());

        // Assert
        response.ShouldBeError(new CourseOfferingNotFound());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidEmails))]
    public async Task Should_not_create_student_with_invalid_email(string email)
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var response = await client.CreateStudent2(data.CourseOffering.Id, "Zezin", email);

        // Assert
        response.ShouldBeError(new InvalidEmail()); 
    }

    [Test]
    public async Task Should_not_create_student_with_duplicated_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var email = TestData.Email;
        var firstResponse = await client.CreateStudent2(data.CourseOffering.Id, "Zezin", email);
        var secondResponse = await client.CreateStudent2(data.CourseOffering.Id, "Zezin", email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_create_student_only_with_student_role()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        var student = await client.CreateStudent(data.CourseOffering.Id);

        // Assert
        using var userManager = _back.GetUserManager();
        var user = await userManager.FindByEmailAsync(student.Email);
        var isOnlyInStudentRole = await userManager.IsOnlyInRole(user!, UserRole.Student);
        isOnlyInStudentRole.Should().BeTrue();
    }
}
