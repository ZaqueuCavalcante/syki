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
        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id);

        // Assert
        student.Id.Should().NotBeEmpty(); 
        student.CourseOfferingId.Should().Be(data.AdsCourseOffering.Id); 
        student.Name.Should().Be("Zezin"); 
    }

    [Test]
    public async Task Should_not_create_student_without_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateStudent(Guid.NewGuid());

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
        var response = await client.CreateStudent(data.AdsCourseOffering.Id, "Zezin", email);

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
        var firstResponse = await client.CreateStudent(data.AdsCourseOffering.Id, "Zezin", email);
        var secondResponse = await client.CreateStudent(data.AdsCourseOffering.Id, "Zezin", email);

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
        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id);

        // Assert
        using var userManager = _back.GetUserManager();
        var user = await userManager.FindByEmailAsync(student.Email);
        var isOnlyInStudentRole = await userManager.IsOnlyInRole(user!, UserRole.Student);
        isOnlyInStudentRole.Should().BeTrue();
    }
}
