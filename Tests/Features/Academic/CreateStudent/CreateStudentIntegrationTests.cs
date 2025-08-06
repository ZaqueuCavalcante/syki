using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_student_with_course_offering()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id);

        // Assert
        student.Id.Should().NotBeEmpty(); 
        student.CourseOfferingId.Should().Be(data.AdsCourseOffering.Id); 
        student.Name.Should().Be("Zezin");

        await AssertDomainEvent<StudentCreatedDomainEvent>(student.Id.ToString());
    }

    [Test]
    public async Task Should_not_create_student_without_course_offering()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateStudent(Guid.CreateVersion7());

        // Assert
        response.ShouldBeError(new CourseOfferingNotFound());
    }

    [Test]
    public async Task Should_not_create_student_with_invalid_email()
    {
        // Arrange
        var email = TestData.InvalidEmails().PickRandom().First().ToString()!;
        var client = await _api.LoggedAsAcademic();
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
        var client = await _api.LoggedAsAcademic();
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
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        // Act
        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id);

        // Assert
        using var userManager = _api.GetUserManager();
        var user = await userManager.FindByEmailAsync(student.Email);
        var isOnlyInStudentRole = await userManager.IsOnlyInRoleAsync(user!, UserRole.Student);
        isOnlyInStudentRole.Should().BeTrue();
    }
}
